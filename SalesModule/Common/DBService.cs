using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace SalesModule
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
                DBService temp = new DBService(Connection.StoresConn);
                temp._conn.Open();
                temp._conn.Close();
                temp._location = DBLocation.RemoteServer;
                return temp;
            }
            catch
            {
                throw new InvalidOperationException("Remote service: connection could not established.");
            }
        }
        public static DBService GetLocalService()
        {
            try
            {
                DBService temp = new DBService(Connection.GetLocalConnectionString());
                temp._conn.Open();
                temp._conn.Close();
                temp._location = DBLocation.LocalServer;
                return temp;
            }
            catch
            {
                throw new InvalidOperationException("Local service: connection could not established.");
            }
        }
        public bool SetStoresConnString(string store)
        {
            if (_location != DBLocation.LocalServer) return false;
            string sql, connstr;
            SqlDataReader dr;
            try
            {
                sql = "select IP, UserName, Password, Catalog from Stores where StoreName = @StoreName";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@StoreName", SqlDbType.NVarChar)).Value = store;

                OpenConnection();
                dr = _cmd.ExecuteReader();
                if (!dr.Read()) return false;
                connstr = Connection.CreateConnectionString(dr["IP"].ToString(),
                    dr["UserName"].ToString(), dr["Password"].ToString(), dr["Catalog"].ToString());

                Connection.StoresConn = connstr;
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
        public UserData GetUserData(string empName, string empPass)
        {
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
            catch
            {
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        #region PLU

        public List<IProduct> GetProducts()
        {
            CheckIsRemote();
            string sql;
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            var list = new List<IProduct>();
            try
            {
                sql = "select pname,pluno,barcode,kind3 from plu";
                _cmd = new SqlCommand(sql, _conn);

                OpenConnection();
                da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                foreach (DataRow R in dt.Rows)
                    list.Add(new Product(R["pluno"].ToString(), R["pname"].ToString(),
                        R["barcode"].ToString(), R["kind3"] as int?));
                return list;
            }
            catch
            {
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        private DataTable SearchProducts(string term, string colName, bool isLikable = true)
        {
            CheckIsRemote();
            string sql;
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            try
            {
                sql = "select pname,pluno,barcode,kind3 from plu where " + colName;
                sql += isLikable ? " like '%' + " : " = ";
                sql += "@Term";
                sql += isLikable ? " + '%'" : "";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@Term", SqlDbType.VarChar)).Value = term;

                OpenConnection();
                da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                return dt;
            }
            catch
            {
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
        public int InsertGroup(SalesGroup g)
        {
            if (g == null || g.Sales.Count == 0)
                return -1;
            CheckIsRemote();
            string sql;
            int GroupID;
            try
            {
                OpenConnection();
                _trans = _conn.BeginTransaction();

                //1. create new group
                sql = "insert into salesGroup (Empno) values (@UserID)";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Transaction = _trans;
                _cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = g.Emp.EmpNo;
                _cmd.ExecuteNonQuery();

                //2. get GroupID
                sql = "select max(GroupID) from SalesGroup";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Transaction = _trans;
                GroupID = int.Parse(_cmd.ExecuteScalar().ToString());

                //3. Insert sales
                for (int i = 0; i < g.Sales.Count; i++)
                    InsertSale(g.Sales[i], GroupID, i + 1);

                //### get PCID associations to here

                _trans.Commit();
                return GroupID;
            }
            catch
            {
                if (_trans != null) _trans.Rollback();
                return -1;
            }
            finally
            {
                CloseConnection();
            }
        }
        private int InsertSale(Sale sale, int GroupID, int OrderID)
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
                        "select @groupID, bhno, 1, @dateFrom, @dateTo, '00:00:00', '23:59:59' from branch";
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
                string msg = "Sale insertion failed. (" +
                    (SaleID != -1 ? "SaleID: " + SaleID :
                    "OrderID=" + OrderID) + ")";
                throw new Exception(msg, ex);
            }
        }
        private bool InsertRequired(List<ProdAmount> reqs, int SaleID)
        {
            string sql;
            SqlParameter pID, pIsID, pQTY;
            if (_trans == null || _conn.State != ConnectionState.Open)
                return false;
            try
            {
                sql = "insert into PluReqSale (SaleID, pluID, isPluno, qty) " +
                    "values (@SaleID, @PluID, @isPluno, @Amount)";
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

                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool InsertDiscounted(List<DiscountedProduct> outs, int SaleID)
        {
            string mainSql, giftSql;
            int maxOutID;
            SqlParameter pOutID, pID, pIsID, pQTY, pMax, pDiscQ, pDiscT;
            if (_trans == null || _conn.State != ConnectionState.Open)
                return false;
            try
            {
                mainSql = "select max(OutID) from PluOutSale";
                _cmd = new SqlCommand(mainSql, _conn);
                _cmd.Transaction = _trans;
                maxOutID = (int)((_cmd.ExecuteScalar() as int?) ?? 0);

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

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EditSale(Sale sale)
        {
            CheckIsRemote();
            _trans = null;

            string sql;
            try
            {
                OpenConnection();
                _trans = _conn.BeginTransaction();

                //1. Edit core properties
                sql = "update sales set Title = @Title, TotalOffPrice = @disAmount, TotalOffType = @disType, " +
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
                if (_cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException("Something went wrong while deleting required products to sale #" + sale.SaleID + ".");

                if (!InsertRequired(sale.ReqProducts, sale.SaleID))
                    throw new InvalidOperationException("Something went wrong while re-inserting required products to sale #" + sale.SaleID + ".");

                //4. insert discounted products
                sql = "delete from PluOutSale where SaleID = @saleID";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Transaction = _trans;
                _cmd.Parameters.Add(new SqlParameter("@saleID", SqlDbType.Int)).Value = sale.SaleID;
                if (_cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException("Something went wrong while deleting discounted products to sale #" + sale.SaleID + ".");

                if (!InsertDiscounted(sale.Discounted, sale.SaleID))
                    throw new InvalidOperationException("Something went wrong while re-inserting discounted products to sale #" + sale.SaleID + ".");

                _trans.Commit();
                return true;
            }
            catch
            {
                _trans.Rollback();
                return false;
            }
        }

        public SalesGroup LoadGroup(int groupID)
        {
            CheckIsRemote();
            List<ProdAmount> Reqs;
            List<DiscountedProduct> Outs;
            List<GiftedProduct> Gifted;
            DataTable SalesTable, dt, tempDt;
            SqlDataAdapter da;
            int SaleID, outID;
            var Sales = new List<Sale>();
            try
            {
                OpenConnection();

                string sql = "select * from Sales where SaleGroupID = @GroupID order by GroupIndex";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;

                SalesTable = new DataTable();
                da = new SqlDataAdapter(_cmd);
                da.Fill(SalesTable);

                foreach (DataRow Rs in SalesTable.Rows)
                {
                    //  Load Sale:
                    SaleID = int.Parse(Rs["SaleID"].ToString());

                    //get requires product from PluReqSale (ProdAmount)
                    //get discounted products from PluOutSale (DiscountedProduct)
                    //get Sale's attributes
                    _cmd = new SqlCommand();
                    _cmd.Connection = _conn;
                    _cmd.Parameters.Add(new SqlParameter("@saleID", SqlDbType.Int)).Value = SaleID;

                    //Requires
                    Reqs = new List<ProdAmount>();
                    _cmd.CommandText = "select * from PluReqSale where SaleID = @saleID";
                    da = new SqlDataAdapter(_cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow Rq in dt.Rows)
                        Reqs.Add(new ProdAmount(Rq["pluID"].ToString(),
                            bool.Parse(Rq["isPluno"].ToString()), double.Parse(Rq["qty"].ToString())));

                    //Discounted
                    Outs = new List<DiscountedProduct>();
                    _cmd.CommandText = "select * from PluOutSale where SaleID = @saleID";
                    da = new SqlDataAdapter(_cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow Ro in dt.Rows)
                    {
                        outID = int.Parse(Ro["OutID"].ToString());
                        Gifted = new List<GiftedProduct>();

                        sql = "select * from PluGiftedSale where OutID = @outID";
                        _cmd = new SqlCommand(sql, _conn);
                        _cmd.Parameters.Add("@outID", SqlDbType.Int).Value = outID;
                        da = new SqlDataAdapter(_cmd);
                        tempDt = new DataTable();
                        da.Fill(tempDt);
                        foreach (DataRow Rg in tempDt.Rows)
                            Gifted.Add(new GiftedProduct(Rg["pluID"].ToString(),
                                bool.Parse(Rg["isPluno"].ToString()), double.Parse(Rg["MultiUnits"].ToString()),
                                new Discount(double.Parse(Rg["offPrice"].ToString()),
                                    (DiscountTypes)int.Parse(Rg["offType"].ToString()))));

                        Outs.Add(new DiscountedProduct(Ro["pluID"].ToString(), bool.Parse(Ro["isPluno"].ToString()),
                            double.Parse(Ro["MultiUnits"].ToString()), double.Parse(Ro["MaxRec"].ToString()),
                            new Discount(double.Parse(Ro["offPrice"].ToString()),
                                (DiscountTypes)int.Parse(Ro["offType"].ToString())), Gifted, outID));
                    }
                    //Attributes
                    var prop = new SalesProperties(Rs["Title"].ToString(),
                        double.Parse(Rs["MinTotalPrice"].ToString()), Rs["MaxTotalPrice"] as double?,
                        int.Parse(Rs["AllowMultiple"].ToString()), int.Parse(Rs["Recurrences"].ToString()));
                    var disc = new Discount(double.Parse(Rs["TotalOffPrice"].ToString()),
                        (DiscountTypes)int.Parse(Rs["TotalOffType"].ToString()));

                    Sales.Add(new Sale((SaleTypes)int.Parse(Rs["SaleType"].ToString()), prop, Reqs, Outs,
                        disc, int.Parse(Rs["GroupIndex"].ToString()), int.Parse(Rs["SaleID"].ToString())));
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
                return new SalesGroup(int.Parse(Att["GroupID"].ToString()), emp,
                    DateTime.Parse(Att["DateCreated"].ToString()),
                    bool.Parse(Att["isEnabled"].ToString()), Sales);
            }
            catch
            {
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        public List<SalesGroup> GetAvailableSales(string vipid = null)
        {
            CheckIsRemote();
            DataTable dt;
            SqlDataAdapter da;
            var Sales = new List<SalesGroup>();
            try
            {
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
                    //### vip group column name
                    //this sql is a where clause to get all the vip table restrictions that the user does NOT meet
                    string getVipGroup = "select VipClub from vip where vipno = @vip";
                    vipOwness = "where (u.isVipno = 1 and u.VipID <> @vip) or (u.isVipno = 0 and u.VipID <> (" + getVipGroup + "))";
                    _cmd.Parameters.Add(new SqlParameter("@vip", SqlDbType.VarChar)).Value = vipid;
                }
                string sql = "select g.GroupID from SalesPcid as p inner join SalesGroup as g on p.SaleGroupID = g.GroupID " +
                    "where p.PCID = @pcid and p.isEnabled = 1 and g.isEnabled = 1 and " +
                    "p.DateFrom <= @nowDate and (p.DateTo is NULL or @nowDate < p.DateTo) and " +
                    "(@nowTime between p.HourFrom and p.HourTo) and " +
                    "p.SaleGroupID not in (select u.SaleGroupID from SalesUser as u " + vipOwness + ")";//###
                _cmd.CommandText = sql;

                dt = new DataTable();
                da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                foreach (DataRow R in dt.Rows)
                    Sales.Add(LoadGroup(int.Parse(R[0].ToString())));
                return Sales;
            }
            catch
            {
                return null;
            }
        }
        public DataTable GetAllSalesTitles()
        {
            //GroupID, Title, ename, isEnabled, DateCreated
            CheckIsRemote();
            string sql;
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            try
            {
                //string sqlTitle = "case when (Max(GroupIndex) from Sales == 1) then " +
                //    "(select the first sale's Title) else ('Sale #' + CONVERT(varchar(10), g.GroupID)) end";
                sql = "select g.GroupID, ('Sale #' + CONVERT(varchar(10), g.GroupID)) as Title, e.ename, g.isEnabled, g.DateCreated " +
                    "from SalesGroup as g inner join emp as e on e.empno = g.empno order by g.DateCreated";
                _cmd = new SqlCommand(sql, _conn);

                OpenConnection();
                da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool DisableSale(int groupID, bool isEnabled)
        {
            CheckIsRemote();
            string sql;
            try
            {
                sql = "update SalesGroup set isEnabled = @status where GroupID = @GroupID";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;
                _cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.Bit)).Value = isEnabled;

                OpenConnection();
                _cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
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
            CheckIsRemote();
            string sql, assoc;
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            try
            {
                assoc = "select VipID from SalesUser where isVipno = 1 and SaleGroupID = @GroupID";
                sql = "select vipno, vname from vip where vipno not in (" + assoc + ")";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;

                OpenConnection();
                da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }
        public DataTable GetVIPGroups(int groupID)
        {
            //groupID, gname, membersCount
            return new DataTable(); //###
            CheckIsRemote();
            string sql, assoc;
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            try
            {
                assoc = "select VipID from SalesUser where isVipno = 0 and SaleGroupID = @SaleID";
                sql = "select groupID, (select count) as membersCount from vipGroups where groupID not in (" + assoc + ")";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;

                OpenConnection();
                da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }
        public DataTable GetSalesVIPs(int groupID)
        {
            //isVipno, VipID, name
            CheckIsRemote();
            string sql;
            string vname = "select v.vname from vip as v where v.vipno = su.VipID";
            string gname = "NULL"; //select g.gname from vGroups as g where g.groupno = su.VipID"; //###
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            try
            {
                sql = "select su.isVipno, su.VipID, " +
                    "(CASE WHEN su.isVipno = 1 THEN (" + vname + ") ELSE (" + gname + ") END) as name " +
                    "from SalesUser as su where su.SaleGroupID = @GroupID";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;

                OpenConnection();
                da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool AssociateVIP2Sale(int groupID, int vipid, bool isVipno)
        {
            CheckIsRemote();
            string sql;
            try
            {
                sql = "insert into SalesUser(SaleGroupID, VipID, isVipno) values (@GroupID, @vipid, @isvipno)";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;
                _cmd.Parameters.Add(new SqlParameter("@vipid", SqlDbType.Int)).Value = vipid;
                _cmd.Parameters.Add(new SqlParameter("@isvipno", SqlDbType.Bit)).Value = isVipno;

                OpenConnection();
                _cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
        public bool DisassociateVIPfromSale(int groupID, int vipid, bool isVipno)
        {
            CheckIsRemote();
            string sql;
            try
            {
                sql = "delete from SalesUser where SaleGroupID = @GroupID and VipID = @vipid and isVipno = @isvipno";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;
                _cmd.Parameters.Add(new SqlParameter("@vipid", SqlDbType.Int)).Value = vipid;
                _cmd.Parameters.Add(new SqlParameter("@isvipno", SqlDbType.Bit)).Value = isVipno;

                OpenConnection();
                _cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool IsSaleRestricted(int groupID)
        {
            CheckIsRemote();
            string sql;
            try
            {
                sql = "select VipID from SalesUser where SaleGroupID = @GroupID";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;

                OpenConnection();
                return _cmd.ExecuteScalar() == null;
            }
            catch
            {
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        #endregion

        #region PCID

        public bool AssociatePcid2Sale(int groupID, int pcid,
            DateTime from, DateTime? to, TimeSpan start, TimeSpan end)
        {
            CheckIsRemote();
            string sql;
            try
            {
                sql = "insert into SalesPCID(SaleGroupID, pcid, isEnabled, DateFrom, DateTo, HourFrom, HourTo) " +
                    "values (@GroupID, @pcid, 1, @dateFrom, @dateTo, @hourFrom, @hourTo)";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;
                _cmd.Parameters.Add(new SqlParameter("@pcid", SqlDbType.Int)).Value = pcid;
                _cmd.Parameters.Add(new SqlParameter("@dateFrom", SqlDbType.DateTime)).Value = from;
                _cmd.Parameters.Add(new SqlParameter("@dateTo", SqlDbType.DateTime)).Value = to ?? (object)DBNull.Value;
                _cmd.Parameters.Add(new SqlParameter("@hourFrom", SqlDbType.Time)).Value = start;
                _cmd.Parameters.Add(new SqlParameter("@hourTo", SqlDbType.Time)).Value = end;

                OpenConnection();
                _cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
        public bool DisassociatePcidfromSale(int groupID, int pcid)
        {
            CheckIsRemote();
            string sql;
            try
            {
                sql = "delete from SalesPcid where SaleGroupID = @GroupID and pcid = @pcid";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;
                _cmd.Parameters.Add(new SqlParameter("@pcid", SqlDbType.Int)).Value = pcid;

                OpenConnection();
                _cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool DisablePCID(int groupID, int pcid, bool isEnabled)
        {
            CheckIsRemote();
            string sql;
            try
            {
                sql = "update SalesPcid set isEnabled = @status where SaleGroupID = @GroupID and pcid = @pcid";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;
                _cmd.Parameters.Add(new SqlParameter("@pcid", SqlDbType.Int)).Value = pcid;
                _cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.Bit)).Value = isEnabled;

                OpenConnection();
                _cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
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
            CheckIsRemote();
            string sql, assoc;
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            try
            {
                assoc = "select pcid from salespcid where SaleGroupID = @GroupID";
                sql = "select bhno, bhname from branch where bhno not in (" + assoc + ")";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;

                OpenConnection();
                da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }
        public DataTable GetSalesBranches(int groupID)
        {
            CheckIsRemote();
            string sql;
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            try
            {
                sql = "select p.*, b.bhname from SalesPcid as p inner join branch as b on b.bhno = p.pcid where p.SaleGroupID = @GroupID";
                _cmd = new SqlCommand(sql, _conn);
                _cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = groupID;

                OpenConnection();
                da = new SqlDataAdapter(_cmd);
                da.Fill(dt);
                return dt;
            }
            catch
            {
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
            CheckIsRemote();
            string sql;
            DataTable dt = new DataTable();
            DataRow R;
            SqlDataReader dr;
            try
            {
                dt.Columns.Add("Title");
                dt.Columns.Add("ID");

                sql = "select " + TitleCol + ", " + IDCol + " from " + table;
                _cmd = new SqlCommand(sql, _conn);

                OpenConnection();
                dr = _cmd.ExecuteReader();
                while (dr.Read())
                {
                    R = dt.NewRow();
                    R["Title"] = dr[TitleCol].ToString();
                    R["ID"] = int.Parse(dr[IDCol].ToString());
                    dt.Rows.Add(R);
                }
                return dt;
            }
            catch
            {
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


        public void ChangeDBDebug()
        {
            return;
            string script = File.ReadAllText(@"C:\Users\Yonatan\Dropbox\Projects\New Order\SalesModule\Documents\DebugScript.sql");

            string[] batches = script.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);
            OpenConnection();
            using (SqlCommand cmd = _conn.CreateCommand())
            {
                foreach (string batch in batches)
                {
                    cmd.CommandText = batch;
                    cmd.ExecuteNonQuery();
                }
            }
            CloseConnection();
        }
    }
}
