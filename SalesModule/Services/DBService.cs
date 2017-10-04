using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using SalesModule.Models;

namespace SalesModule.Services
{
    internal class DBService
    {
        private enum DBLocation { LocalServer, RemoteServer, Invalid }
        private SqlConnection _conn;
        private SqlCommand _cmd;
        private SqlTransaction _trans;
        private DBLocation _location;

        private bool _wasOpened;

        private DBService(string connStr)
        {
            _conn = new SqlConnection(connStr);
            _trans = null;
            _wasOpened = false;
            _location = DBLocation.Invalid;
        }
        public static DBService GetService()
        {
            try
            {
                DBService temp = new DBService(ConnectionService.StoresConn);
                temp._conn.Open();
                temp._conn.Close();
                temp._location = DBLocation.RemoteServer;
                return temp;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                throw;
            }
        }
        public static DBService GetLocalService()
        {
            ActivityLogService.Logger.LogCall();
            try
            {
                DBService temp = new DBService(ConnectionService.GetLocalConnectionString());
                temp._conn.Open();
                temp._conn.Close();
                temp._location = DBLocation.LocalServer;
                return temp;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                throw;
            }
        }
        public bool SetStoresConnString(string store)
        {
            if (_location != DBLocation.LocalServer) return false;
            ActivityLogService.Logger.LogCall(store);
            try
            {
                string sql = "select IP, UserName, Password, Catalog from Stores where StoreName = @StoreName";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@StoreName", SqlDbType.NVarChar)).Value = store;

                OpenConnection();
                var dr = _cmd.ExecuteReader();
                if (!dr.Read()) return false;
                var connstr = ConnectionService.CreateConnectionString(dr["IP"].ToString(),
                    dr["UserName"].ToString(), dr["Password"].ToString(), dr["Catalog"].ToString());

                ConnectionService.StoresConn = connstr;
                return true;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
        public UserData GetUserData(string empName, string empPass)
        {
            ActivityLogService.Logger.LogCall(empName, empPass);
            if (_location != DBLocation.RemoteServer) return null;
            try
            {
                string sql = "select * from emp where ename = @EName and uid = @Password";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@EName", SqlDbType.NVarChar)).Value = empName;
                _cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar)).Value = empPass;

                OpenConnection();
                object empno = _cmd.ExecuteScalar();
                if (empno == null) return null;

                return new UserData(int.Parse(empno.ToString()), empName, empPass);
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        #region PLU

        public List<IProductM> GetProducts()
        {
            ActivityLogService.Logger.LogCall();
            DataTable dt = new DataTable();
            var list = new List<IProductM>();
            try
            {
                CheckIsRemote();
                var sql = "select pname,pluno,barcode,kind3 from plu";
                _cmd = new SqlCommand(sql, _conn);

                OpenConnection();
                var da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                foreach (DataRow R in dt.Rows)
                    list.Add(new ProductM(R["pluno"].ToString(), R["pname"].ToString(),
                        R["barcode"].ToString(), R["kind3"] as int?));

                sql = "select * from kind3";
                _cmd = new SqlCommand(sql, _conn);

                da = new SqlDataAdapter(_cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow R in dt.Rows)
                    list.Add(new CategoryM(R["KindNo"].ToString(), R["KindName"].ToString(), R["Rem"].ToString()));

                return list;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IProductM GetProduct(string id, bool isPluno)
        {
            ActivityLogService.Logger.LogCall(id, isPluno);
            var dt = new DataTable();
            try
            {
                CheckIsRemote();
                if (isPluno)
                {
                    string sql = "select pname,barcode,kind3 from plu where pluno=@pluno";
                    _cmd = new SqlCommand(sql, _conn);
                    _cmd.Parameters.Add(new SqlParameter("@pluno", SqlDbType.VarChar)).Value = id;

                    OpenConnection();
                    var da = new SqlDataAdapter(_cmd);
                    da.Fill(dt);

                    var R = dt.Rows[0];
                    return new ProductM(id, R["pname"].ToString(),
                        R["barcode"].ToString(), R["kind3"] as int?);
                }
                else
                {
                    string sql = "select KindName, Rem from kind3 where KindNo=@kindno";
                    _cmd = new SqlCommand(sql, _conn);
                    _cmd.Parameters.Add(new SqlParameter("@kindno", SqlDbType.VarChar)).Value = id;

                    OpenConnection();
                    var da = new SqlDataAdapter(_cmd);
                    da.Fill(dt);

                    return new CategoryM(id, dt.Rows[0]["KindName"].ToString(), dt.Rows[0]["Rem"].ToString());
                }
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        private DataTable SearchProducts(string term, string colName, bool isLikable = true)
        {
            ActivityLogService.Logger.LogCall(term);
            DataTable dt = new DataTable();
            try
            {
                CheckIsRemote();
                string sql = "select pname,pluno,barcode,kind3 from plu where " + colName;
                sql += isLikable ? " like '%' + " : " = ";
                sql += "@Term";
                sql += isLikable ? " + '%'" : "";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@Term", SqlDbType.VarChar)).Value = term;

                OpenConnection();
                var da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }
        public DataTable SearchProductsByName(string term) { return SearchProducts(term, "pname"); }
        public DataTable SearchProductsByBarcode(string term) { return SearchProducts(term, "Barcode", false); }
        public DataTable SearchProductsByPluno(string term) { return SearchProducts(term, "pluno"); }

        #endregion

        #region Sales
        public int InsertGroup(SalesGroupM g)
        {
            ActivityLogService.Logger.LogCall(g.GroupID);
            if (g == null || g.Sales.Count == 0)
                return -1;
            try
            {
                CheckIsRemote();
                OpenConnection();
                _trans = _conn.BeginTransaction();

                //1. create new group
                var sql = "insert into salesGroup (Empno) values (@UserID)";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Transaction = _trans;
                _cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = g.Emp.EmpNo;
                _cmd.ExecuteNonQuery();

                //2. get GroupID
                sql = "select max(GroupID) from SalesGroup";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Transaction = _trans;
                var GroupID = int.Parse(_cmd.ExecuteScalar().ToString());

                //3. Insert sales
                for (int i = 0; i < g.Sales.Count; i++)
                    InsertSaleM(g.Sales[i], GroupID, i + 1);

                //### get PCID associations to here

                _trans.Commit();
                return GroupID;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                if (_trans != null) _trans.Rollback();
                return -1;
            }
            finally
            {
                CloseConnection();
            }
        }
        private int InsertSaleM(SaleM sale, int GroupID, int OrderID)
        {
            CheckIsRemote();
            if (_trans == null)
                return -1;

            string sql;
            int SaleID = -1;
            try
            {
                //1. create new sales
                sql = "insert into sales (SaleGroupID, GroupIndex, SaleType, Title, TotalOffPrice, TotalOffType, MinTotalPrice, MaxTotalPrice, AllowMultiple, Recurrences) " +
                    "values (@groupID, @groupIndex, @type, @Title, @disAmount, @disType, @minPrice, @maxPrice, @multiple, @recurrence)";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Transaction = _trans;

                _cmd.Parameters.Add(new SqlParameter("@groupID", SqlDbType.Int)).Value = GroupID;
                _cmd.Parameters.Add(new SqlParameter("@groupIndex", SqlDbType.Int)).Value = OrderID;
                _cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.Int)).Value = (int)sale.Type;
                _cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar)).Value = sale.Title;

                _cmd.Parameters.Add(new SqlParameter("@disAmount", SqlDbType.Real)).Value = sale.Discount.Amount;
                _cmd.Parameters.Add(new SqlParameter("@disType", SqlDbType.Int)).Value = (int)sale.Discount.Type;

                _cmd.Parameters.Add(new SqlParameter("@minPrice", SqlDbType.Real)).Value = sale.Properties.MinPrice;
                _cmd.Parameters.Add(new SqlParameter("@maxPrice", SqlDbType.Real)).Value = sale.Properties.MaxPrice ?? (object)DBNull.Value;
                _cmd.Parameters.Add(new SqlParameter("@multiple", SqlDbType.Int)).Value = sale.Properties.InstanceMultiply;
                _cmd.Parameters.Add(new SqlParameter("@recurrence", SqlDbType.Int)).Value = sale.Properties.RecurrencePerInstance;
                _cmd.ExecuteNonQuery();

                //2. get SaleID
                sql = "select max(SaleID) from Sales";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Transaction = _trans;
                SaleID = int.Parse(_cmd.ExecuteScalar().ToString());

                //3. insert required products
                InsertRequired(sale.ReqProducts, SaleID);

                //4. insert discounted products
                InsertDiscounted(sale.Discounted, SaleID);

                //### error!!! move to InsertGroup()
                //5. associate to branches
                if (sale.Properties.IsBroadSale)
                {
                    sql = "insert into SalesPCID(SaleGroupID, pcid, isEnabled, DateFrom, DateTo, HourFrom, HourTo) " +
                        "select @groupID, bhno, 1, @dateFrom, @dateTo, NULL, NULL from branch";
                    _cmd = new SqlCommand(sql, _conn);
                    _cmd.Transaction = _trans;
                    _cmd.Parameters.Add(new SqlParameter("@groupID", SqlDbType.Int)).Value = GroupID;
                    _cmd.Parameters.Add(new SqlParameter("@dateFrom", SqlDbType.DateTime)).Value = sale.Properties.DateFrom;
                    _cmd.Parameters.Add(new SqlParameter("@dateTo", SqlDbType.DateTime)).Value = sale.Properties.DateTo ?? (object)DBNull.Value;
                    _cmd.ExecuteNonQuery();
                }

                return SaleID;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                throw;
            }
        }
        private void InsertRequired(List<ProdAmountM> reqs, int SaleID)
        {
            string sql;
            SqlParameter pID, pIsID, pQTY;
            if (_trans == null || _conn.State != ConnectionState.Open)
                throw new InvalidOperationException("Transaction must be started before calling InsertRequired function.");
            try
            {
                sql = "insert into PluReqSale (SaleID, pluID, isPluno, qty) values (@SaleID, @PluID, @isPluno, @Amount)";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Transaction = _trans;
                _cmd.Parameters.Add(new SqlParameter("@SaleID", SqlDbType.Int)).Value = SaleID;
                pID = _cmd.Parameters.Add(new SqlParameter("@PluID", SqlDbType.Int));
                pIsID = _cmd.Parameters.Add(new SqlParameter("@isPluno", SqlDbType.Bit));
                pQTY = _cmd.Parameters.Add(new SqlParameter("@Amount", SqlDbType.Real));
                foreach (var p in reqs)
                {
                    pID.Value = p.ID;
                    pIsID.Value = p.isPluno;
                    pQTY.Value = p.Amount;
                    _cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                throw;
            }
        }
        private void InsertDiscounted(List<DiscountedProductM> outs, int SaleID)
        {
            string mainSql, giftSql;
            int maxOutID;
            SqlParameter pOutID, pID, pIsID, pQTY, pMax, pDiscQ, pDiscT;
            if (_trans == null || _conn.State != ConnectionState.Open)
                throw new InvalidOperationException("Transaction must be started before calling InsertDiscounted function.");
            try
            {
                mainSql = "select max(OutID) from PluOutSale";
                _cmd = new SqlCommand(mainSql, _conn);
                _cmd.Transaction = _trans;
                maxOutID = _cmd.ExecuteScalar() as int? ?? 0;

                mainSql = "insert into PluOutSale (OutID, SaleID, pluID, isPluno, MultiUnits, MaxRec, offPrice, offType) " +
                "values (@OutID ,@SaleID, @PluID, @isPluno, @Amount, @Max, @discPrice, @discType)";
                giftSql = "insert into PluGiftedSale (OutID, pluID, isPluno, MultiUnits, offPrice, offType) " +
                "values (@OutID, @PluID, @isPluno, @Amount, @discPrice, @discType)";

                _cmd = new SqlCommand(mainSql, _conn);
                _cmd.Transaction = _trans;
                pOutID = _cmd.Parameters.Add(new SqlParameter("@OutID", SqlDbType.Int));
                _cmd.Parameters.Add(new SqlParameter("@SaleID", SqlDbType.Int)).Value = SaleID;
                pID = _cmd.Parameters.Add(new SqlParameter("@PluID", SqlDbType.Int));
                pIsID = _cmd.Parameters.Add(new SqlParameter("@isPluno", SqlDbType.Bit));
                pQTY = _cmd.Parameters.Add(new SqlParameter("@Amount", SqlDbType.Real));
                pMax = _cmd.Parameters.Add(new SqlParameter("@Max", SqlDbType.Real));
                pDiscQ = _cmd.Parameters.Add(new SqlParameter("@discPrice", SqlDbType.Real));
                pDiscT = _cmd.Parameters.Add(new SqlParameter("@discType", SqlDbType.Int));

                foreach (var p in outs)
                {
                    if (p.Discount == null)
                        throw new ArgumentException("Discounted product must have a discount. (pluno: " + p.ID + ")");
                    pOutID.Value = ++maxOutID;

                    _cmd.CommandText = mainSql;
                    pID.Value = p.ID;
                    pIsID.Value = p.isPluno;
                    pQTY.Value = p.Amount;
                    pMax.Value = p.MaxMultiply;
                    pDiscQ.Value = p.Discount.Amount;
                    pDiscT.Value = (int)p.Discount.Type;
                    _cmd.ExecuteNonQuery();

                    _cmd.CommandText = giftSql;
                    foreach (var gift in p.Discounted)
                    {
                        if (gift.Discount == null)
                            throw new ArgumentException("Gifted product must have a discount. (pluno: " + p.ID + ")");
                        pID.Value = gift.ID;
                        pIsID.Value = gift.isPluno;
                        pQTY.Value = gift.Amount;
                        pDiscQ.Value = gift.Discount.Amount;
                        pDiscT.Value = (int)gift.Discount.Type;
                        _cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                throw;
            }
        }

        public bool EditSale(SaleM sale)
        {
            if (sale == null) return false;
            ActivityLogService.Logger.LogCall(sale.SaleID);
            _trans = null;
            try
            {
                CheckIsRemote();
                OpenConnection();
                _trans = _conn.BeginTransaction();

                //1. Edit core properties
                string sql = "update sales set Title = @Title, TotalOffPrice = @disAmount, TotalOffType = @disType, " +
                    "MinTotalPrice = @minPrice, MaxTotalPrice = @maxPrice, AllowMultiple = @multiple, " +
                    "Recurrences = @recurrence where SaleID = @saleID";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Transaction = _trans;

                _cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar)).Value = sale.Title;
                _cmd.Parameters.Add(new SqlParameter("@disAmount", SqlDbType.Real)).Value = sale.Discount.Amount;
                _cmd.Parameters.Add(new SqlParameter("@disType", SqlDbType.Int)).Value = (int)sale.Discount.Type;

                _cmd.Parameters.Add(new SqlParameter("@minPrice", SqlDbType.Real)).Value = sale.Properties.MinPrice;
                _cmd.Parameters.Add(new SqlParameter("@maxPrice", SqlDbType.Real)).Value = sale.Properties.MaxPrice ?? (object)DBNull.Value;
                _cmd.Parameters.Add(new SqlParameter("@multiple", SqlDbType.Int)).Value = sale.Properties.InstanceMultiply;
                _cmd.Parameters.Add(new SqlParameter("@recurrence", SqlDbType.Int)).Value = sale.Properties.RecurrencePerInstance;

                _cmd.Parameters.Add(new SqlParameter("@saleID", SqlDbType.Int)).Value = sale.SaleID;
                if (_cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException("Something went wrong while updating sale #" + sale.SaleID + ".");

                //2. Re-insert required products
                sql = "delete from PluReqSale where SaleID = @saleID";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Transaction = _trans;
                _cmd.Parameters.Add(new SqlParameter("@saleID", SqlDbType.Int)).Value = sale.SaleID;
                _cmd.ExecuteNonQuery();

                InsertRequired(sale.ReqProducts, sale.SaleID);

                //4. insert discounted products
                sql = "delete from PluOutSale where SaleID = @saleID";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Transaction = _trans;
                _cmd.Parameters.Add(new SqlParameter("@saleID", SqlDbType.Int)).Value = sale.SaleID;
                _cmd.ExecuteNonQuery();

                InsertDiscounted(sale.Discounted, sale.SaleID);

                _trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                if (_trans != null) _trans.Rollback();
                return false;
            }
        }

        public SalesGroupM LoadGroup(int groupID)
        {
            ActivityLogService.Logger.LogCall(groupID);
            DataTable dt;
            var sales = new List<SaleM>();
            try
            {
                CheckIsRemote();
                OpenConnection();

                string sql = "select * from Sales where SaleGroupID = @GroupID order by GroupIndex";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;

                var salesTable = new DataTable();
                var da = new SqlDataAdapter(_cmd);
                da.Fill(salesTable);

                foreach (DataRow Rs in salesTable.Rows)
                {
                    //  Load Sale:
                    var saleId = int.Parse(Rs["SaleID"].ToString());

                    //get requires product from PluReqSale (ProdAmount)
                    //get discounted products from PluOutSale (DiscountedProduct)
                    //get Sale's attributes
                    _cmd = new SqlCommand();
                    _cmd.Connection = _conn;
                    _cmd.Parameters.Add(new SqlParameter("@saleID", SqlDbType.Int)).Value = saleId;

                    //Requires
                    var reqs = new List<ProdAmountM>();
                    _cmd.CommandText = "select * from PluReqSale where SaleID = @saleID";
                    da = new SqlDataAdapter(_cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow Rq in dt.Rows)
                        reqs.Add(new ProdAmountM(Rq["pluID"].ToString(),
                            bool.Parse(Rq["isPluno"].ToString()), double.Parse(Rq["qty"].ToString())));

                    //Discounted
                    var outs = new List<DiscountedProductM>();
                    _cmd.CommandText = "select * from PluOutSale where SaleID = @saleID";
                    da = new SqlDataAdapter(_cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow Ro in dt.Rows)
                    {
                        var outID = int.Parse(Ro["OutID"].ToString());
                        var gifted = new List<GiftedProductM>();

                        sql = "select * from PluGiftedSale where OutID = @outID";
                        _cmd = new SqlCommand(sql, _conn);
                        _cmd.Parameters.Add("@outID", SqlDbType.Int).Value = outID;
                        da = new SqlDataAdapter(_cmd);
                        var tempDt = new DataTable();
                        da.Fill(tempDt);
                        foreach (DataRow Rg in tempDt.Rows)
                            gifted.Add(new GiftedProductM(Rg["pluID"].ToString(),
                                bool.Parse(Rg["isPluno"].ToString()), double.Parse(Rg["MultiUnits"].ToString()),
                                new DiscountM(double.Parse(Rg["offPrice"].ToString()),
                                    (DiscountTypes)int.Parse(Rg["offType"].ToString()))));

                        outs.Add(new DiscountedProductM(Ro["pluID"].ToString(), bool.Parse(Ro["isPluno"].ToString()),
                            double.Parse(Ro["MultiUnits"].ToString()), double.Parse(Ro["MaxRec"].ToString()),
                            new DiscountM(double.Parse(Ro["offPrice"].ToString()),
                                (DiscountTypes)int.Parse(Ro["offType"].ToString())), gifted, outID));
                    }
                    //Attributes
                    var prop = new SalesPropertiesM(Rs["Title"].ToString(),
                        double.Parse(Rs["MinTotalPrice"].ToString()), Rs["MaxTotalPrice"] as double?,
                        int.Parse(Rs["AllowMultiple"].ToString()), int.Parse(Rs["Recurrences"].ToString()));
                    var disc = new DiscountM(double.Parse(Rs["TotalOffPrice"].ToString()),
                        (DiscountTypes)int.Parse(Rs["TotalOffType"].ToString()));

                    sales.Add(new SaleM((SaleTypes)int.Parse(Rs["SaleType"].ToString()), prop, reqs, outs,
                        disc, int.Parse(Rs["GroupIndex"].ToString()), saleId));
                }

                sql = "select g.*, e.ename, e.uid from SalesGroup as g inner join emp as e on g.empno = e.empno where g.GroupID= @groupID";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@groupID", SqlDbType.Int)).Value = groupID;
                da = new SqlDataAdapter(_cmd);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                    throw new InvalidConstraintException("No sale record found for the given GroupID(" + groupID + ").");
                else if (dt.Rows.Count != 1)
                    throw new InvalidConstraintException("More than one instance of sale record found for the given GroupID(" + groupID + ").");
                DataRow Att = dt.Rows[0];

                var emp = new UserData(int.Parse(Att["empno"].ToString()), Att["ename"].ToString(), Att["uid"].ToString());
                return new SalesGroupM(int.Parse(Att["GroupID"].ToString()), emp,
                    DateTime.Parse(Att["DateCreated"].ToString()),
                    bool.Parse(Att["isEnabled"].ToString()), sales);
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        public List<SalesGroupM> GetAvailableSales(string vipid = null)
        {
            if (vipid == "")
				vipid = null;

            ActivityLogService.Logger.LogCall(vipid);
            DataTable dt;
            var Sales = new List<SalesGroupM>();
            string sql = "";
            try
            {
                CheckIsRemote();
                OpenConnection();
                //get all GroupID from SalesPcid that does NOT exist in SalesUser where -
                //  pcid, DateFrom < Date, DateTo != null && Date < DateTo,
                //  HourFrom < DateTime.Hour < HourTo , SalesGroup.isEnabled = true
                _cmd = new SqlCommand();
                _cmd.Connection = _conn;

                _cmd.Parameters.Add(new SqlParameter("@pcid", SqlDbType.Int)).Value = Wrapper.PCID;
                _cmd.Parameters.Add(new SqlParameter("@nowDate", SqlDbType.Date)).Value = DateTime.Now.Date;
                _cmd.Parameters.Add(new SqlParameter("@nowTime", SqlDbType.Time)).Value = DateTime.Now.TimeOfDay;

                string vipOwness = "";
                if (vipid != null)
                {
                    //this sql is a where clause to get all the vip table restrictions that the user does NOT meet
                    string getMyVipGroup = "select ClubNo from vip where vipno = @vip";
                    vipOwness = "g.GroupID in (select u.SaleGroupID from SalesUser as u " +
                        "where (u.isVipno = 1 and u.VipID = @vip) or (u.isVipno = 0 and u.VipID in (" + getMyVipGroup + "))";
                    _cmd.Parameters.Add(new SqlParameter("@vip", SqlDbType.VarChar)).Value = vipid;
                }
                else
                    vipOwness = "(select count(*) from SalesUser as u where u.SaleGroupID=g.GroupID) = 0";

                sql =
                    "select g.GroupID from SalesPcid as p inner join SalesGroup as g on p.SaleGroupID = g.GroupID " +
                    "where p.PCID = @pcid and p.isEnabled = 1 and g.isEnabled = 1 and " +
                    "p.DateFrom <= @nowDate and (p.DateTo is NULL or @nowDate < p.DateTo) and " +
                    "(p.HourFrom is NULL or @nowTime between p.HourFrom and p.HourTo) and " + vipOwness;
                _cmd.CommandText = sql;

                dt = new DataTable();
                var da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                foreach (DataRow R in dt.Rows)
                    Sales.Add(LoadGroup(int.Parse(R[0].ToString())));
                return Sales;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex, "query:" + sql);
                return null;
            }
        }
        public DataTable GetAllSalesTitles()
        {
            //GroupID, Title, ename, isEnabled, DateCreated
            ActivityLogService.Logger.LogCall();
            DataTable dt = new DataTable();
            try
            {
                CheckIsRemote();
                string sqlTitle = "IIF((select Max(GroupIndex) from Sales where SaleGroupID=g.GroupID) = 1, " +
                                  "(select Top 1 Title from Sales where SaleGroupID=g.groupID), " +
                                  "('Sales Group #' + CONVERT(varchar(10), g.GroupID)))";
                string sql = "select g.GroupID, " + sqlTitle + " as Title, e.ename, g.isEnabled, g.DateCreated " +
                    "from SalesGroup as g inner join emp as e on e.empno = g.empno order by g.DateCreated";
                _cmd = new SqlCommand(sql, _conn);

                OpenConnection();
                var da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool DisableSaleGroupM(int groupID, bool isEnabled)
        {
            ActivityLogService.Logger.LogCall(groupID, isEnabled);
            try
            {
                CheckIsRemote();
                string sql = "update SalesGroup set isEnabled = @status where GroupID = @GroupID";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;
                _cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.Bit)).Value = isEnabled;

                OpenConnection();
                _cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        #endregion

        #region VIP

        public DataTable GetVIPSingles(int groupID)
        {
            //vipno, vname
            DataTable dt = new DataTable();
            ActivityLogService.Logger.LogCall(groupID);
            try
            {
                CheckIsRemote();
                var assoc = "select VipID from SalesUser where isVipno = 1 and SaleGroupID = @GroupID";
                var sql = "select vipno, vname from vip where vipno not in (" + assoc + ")";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;

                OpenConnection();
                var da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }
        public DataTable GetVIPGroups(int groupID)
        {
            //clubno, clubName, membersCount
            DataTable dt = new DataTable();
            ActivityLogService.Logger.LogCall(groupID);
            try
            {
                CheckIsRemote();
                string assoc = "select VipID from SalesUser where isVipno = 0 and SaleGroupID = @GroupID";
                string count = "select count(v.vipno) from vip as v where v.ClubNo = c.ClubNo";
                string sql = "select c.ClubNo, c.ClubName, (" + count + ") as membersCount from clubs as c where c.ClubNo not in (" + assoc + ")";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;

                OpenConnection();
                var da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }
        public DataTable GetSalesVIPs(int groupID)
        {
            //isVipno, VipID, name, membersCount
            DataTable dt = new DataTable();
            ActivityLogService.Logger.LogCall(groupID);
            try
            {
                CheckIsRemote();
                string vname = "select v.vname from vip as v where v.vipno = su.VipID";
                string gname = "select c.ClubName from clubs as c where c.ClubNo = su.VipID";
                string count = "select count(v.vipno) from vip as v where v.ClubNo = su.VipID";
                string sql = "select su.isVipno, su.VipID, " +
                    "IIF(su.isVipno = 1, (" + vname + "), (" + gname + ")) as name, " +
                    "IIF(su.isVipno = 1, 0, (" + count + ")) as membersCount " +
                    "from SalesUser as su where su.SaleGroupID = @GroupID";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;

                OpenConnection();
                var da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool AssociateVIP2Sale(int groupID, int vipid, bool isVipno)
        {
            ActivityLogService.Logger.LogCall(groupID, vipid, isVipno);
            try
            {
                CheckIsRemote();
                string sql = "insert into SalesUser(SaleGroupID, VipID, isVipno) values (@GroupID, @vipid, @isvipno)";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;
                _cmd.Parameters.Add(new SqlParameter("@vipid", SqlDbType.Int)).Value = vipid;
                _cmd.Parameters.Add(new SqlParameter("@isvipno", SqlDbType.Bit)).Value = isVipno;

                OpenConnection();
                _cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
        public bool DisassociateVIPfromSale(int groupID, int vipid, bool isVipno)
        {
            ActivityLogService.Logger.LogCall(groupID, vipid, isVipno);
            try
            {
                CheckIsRemote();
                string sql = "delete from SalesUser where SaleGroupID = @GroupID and VipID = @vipid and isVipno = @isvipno";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;
                _cmd.Parameters.Add(new SqlParameter("@vipid", SqlDbType.Int)).Value = vipid;
                _cmd.Parameters.Add(new SqlParameter("@isvipno", SqlDbType.Bit)).Value = isVipno;

                OpenConnection();
                _cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool IsSaleRestricted(int groupID)
        {
            ActivityLogService.Logger.LogCall(groupID);
            try
            {
                CheckIsRemote();
                string sql = "select VipID from SalesUser where SaleGroupID = @GroupID";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;

                OpenConnection();
                return _cmd.ExecuteScalar() == null;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        #endregion

        #region PCID

        public bool AssociatePcid2SaleM(int groupID, int pcid,
            DateTime from, DateTime? to)
        {
            return associatePcid2SaleM(groupID, pcid, from, to, null, null);
        }
        public bool AssociatePcid2SaleM(int groupID, int pcid,
            DateTime from, DateTime? to, TimeSpan start, TimeSpan end)
        {
            return associatePcid2SaleM(groupID, pcid, from, to, start, end);
        }
        private bool associatePcid2SaleM(int groupID, int pcid,
            DateTime from, DateTime? to, TimeSpan? start, TimeSpan? end)
        {
            ActivityLogService.Logger.LogCall(groupID, pcid);
            try
            {
                CheckIsRemote();
                string sql = "insert into SalesPCID(SaleGroupID, pcid, isEnabled, DateFrom, DateTo, HourFrom, HourTo) " +
                    "values (@GroupID, @pcid, 1, @dateFrom, @dateTo, @hourFrom, @hourTo)";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;
                _cmd.Parameters.Add(new SqlParameter("@pcid", SqlDbType.Int)).Value = pcid;
                _cmd.Parameters.Add(new SqlParameter("@dateFrom", SqlDbType.DateTime)).Value = from;
                _cmd.Parameters.Add(new SqlParameter("@dateTo", SqlDbType.DateTime)).Value = to ?? (object)DBNull.Value;
                _cmd.Parameters.Add(new SqlParameter("@hourFrom", SqlDbType.Time)).Value = start ?? (object)DBNull.Value;
                _cmd.Parameters.Add(new SqlParameter("@hourTo", SqlDbType.Time)).Value = end ?? (object)DBNull.Value;

                OpenConnection();
                _cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool DisassociatePcidfromSaleM(int groupID, int pcid)
        {
            ActivityLogService.Logger.LogCall(groupID, pcid);
            try
            {
                CheckIsRemote();
                string sql = "delete from SalesPcid where SaleGroupID = @GroupID and pcid = @pcid";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;
                _cmd.Parameters.Add(new SqlParameter("@pcid", SqlDbType.Int)).Value = pcid;

                OpenConnection();
                _cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool DisablePCID(int groupID, int pcid, bool isEnabled)
        {
            ActivityLogService.Logger.LogCall(groupID, pcid, isEnabled);
            try
            {
                CheckIsRemote();
                string sql = "update SalesPcid set isEnabled = @status where SaleGroupID = @GroupID and pcid = @pcid";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;
                _cmd.Parameters.Add(new SqlParameter("@pcid", SqlDbType.Int)).Value = pcid;
                _cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.Bit)).Value = isEnabled;

                OpenConnection();
                _cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataTable GetUnattachedPcid(int groupID)
        {
            //bhno, bhname
            DataTable dt = new DataTable();
            ActivityLogService.Logger.LogCall(groupID);
            try
            {
                CheckIsRemote();
                string assoc = "select pcid from salespcid where SaleGroupID = @GroupID";
                string sql = "select bhno, bhname from branch where bhno not in (" + assoc + ")";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;

                OpenConnection();
                var da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }
        public DataTable GetSalesBranches(int groupID)
        {
            DataTable dt = new DataTable();
            ActivityLogService.Logger.LogCall(groupID);
            try
            {
                CheckIsRemote();
                string sql = "select p.*, b.bhname from SalesPcid as p inner join branch as b on b.bhno = p.pcid where p.SaleGroupID = @GroupID";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;

                OpenConnection();
                var da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        #endregion

        public DataTable GetDiscountTypes() { return GetDictionaryTable("SalesOffType", "TypeName", "TypeID"); }
        public DataTable GetAllBranches() { return GetDictionaryTable("branch", "bhname", "bhno"); }

        #region Utils

        private DataTable GetDictionaryTable(string table, string TitleCol, string IDCol)
        {
            DataTable dt = new DataTable();
            try
            {
                CheckIsRemote();
                dt.Columns.Add("Title");
                dt.Columns.Add("ID");

                string sql = "select " + TitleCol + ", " + IDCol + " from " + table;
                _cmd = new SqlCommand(sql, _conn);

                OpenConnection();
                var dr = _cmd.ExecuteReader();
                while (dr.Read())
                {
                    var R = dt.NewRow();
                    R["Title"] = dr[TitleCol].ToString();
                    R["ID"] = int.Parse(dr[IDCol].ToString());
                    dt.Rows.Add(R);
                }
                return dt;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        private void OpenConnection()
        {
            _wasOpened = _conn.State == ConnectionState.Open;
            if (!_wasOpened)
                _conn.Open();
        }
        private void CloseConnection()
        {
            if (!_wasOpened)
                _conn.Close();
        }

        private void CheckIsRemote()
        {
            if (_location != DBLocation.RemoteServer)
                throw new InvalidOperationException("This operation is only valid on remote server.");
        }

        #endregion

        public DataTable Test(string table)
        {
            ActivityLogService.Logger.LogCall();
            DataTable dt = new DataTable();
            var list = new List<IProductM>();
            try
            {
                CheckIsRemote();
                //Kind3: REM, KINDNAME, KINDNO
                //VIP: VipType / clubno
                //clubs: clubname,  clubno, isactive
                var sql = "select * from " + table;
                _cmd = new SqlCommand(sql, _conn);

                OpenConnection();
                var da = new SqlDataAdapter(_cmd);
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
