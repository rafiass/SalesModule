using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesModule
{
    public enum InitResults { Success, F_Credentials, F_Unhandled };

    [Guid(Wrapper.InterfaceId)]
    public interface IWrapper
    {
        InitResults Init(string store, string userName, string password, int pcid);
        InitResults Init(string ip, string Catalog, string username, string password, int pcid, string empName, string empPass);
        bool ChangeUser(string empName, string empPass);
        bool IsInited();

        string Version { get; }
    }

    [Guid(ClassId), ClassInterface(ClassInterfaceType.None)]
    public class Wrapper : IWrapper
    {
        #region COM
#if COM_INTEROP_ENABLED
        public const string ClassId = "d5edab1e-11eb-4625-9d4d-fa4e844c87d2";
        public const string InterfaceId = "58290a7c-0f4b-4cb0-8c6d-2aa88582c76f";

        // These routines perform the additional COM registration needed by ActiveX controls
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComRegisterFunction]
        private static void Register(System.Type t)
        {
            ComRegistration.RegisterControl(t);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComUnregisterFunction]
        private static void Unregister(System.Type t)
        {
            ComRegistration.UnregisterControl(t);
        }

#endif
        #endregion

        internal static UserData User;
        internal static int PCID;

        public string Version
        {
            get
            {
                var v = (new System.Reflection.AssemblyName(
                     System.Reflection.Assembly.GetExecutingAssembly().FullName)).Version;
                return v.Major + "." + v.Minor;
            }
        }

        public Wrapper()
        {
            User = null;
            PCID = -1;
        }

        public InitResults Init(string ip, string Catalog, string username, string password, int pcid, string empName, string empPass)
        {
            ActivityLog.Logger.LogCall();
            string connBackup = "";
            try
            {
                UserData user;
                Connection.StoresConn = connBackup = Connection.CreateConnectionString(ip, username, password, Catalog);
                if ((user = DBService.GetService()
                    .GetUserData(empName, empPass)) != null)
                {
                    User = user;
                    PCID = pcid;
                    return InitResults.Success;
                }
                ActivityLog.Logger.LogMessage("Login failed!");
                return InitResults.F_Credentials;
            }
            catch (Exception ex)
            {
                ActivityLog.Logger.LogError(ex);
                if (connBackup != "")
                    Connection.StoresConn = connBackup;
                return InitResults.F_Unhandled;
            }
        }
        public InitResults Init(string store, string userName, string password, int pcid)
        {
            ActivityLog.Logger.LogCall();
            try
            {
                UserData user;
                var db = DBService.GetLocalService();
                db.SetStoresConnString(store);

                if ((user = (DBService.GetService()).GetUserData(userName, password)) != null)
                {
                    User = user;
                    PCID = pcid;
                    return InitResults.Success;
                }
                ActivityLog.Logger.LogMessage("Login failed!");
                return InitResults.F_Credentials;
            }
            catch (Exception ex)
            {
                ActivityLog.Logger.LogError(ex);
                return InitResults.F_Unhandled;
            }
        }

        public bool ChangeUser(string empName, string empPass)
        {
            ActivityLog.Logger.LogCall(empName, empPass);
            try
            {
                var db = DBService.GetLocalService();
                var user = db.GetUserData(empName, empPass);
                if (user != null)
                {
                    User = user;
                    return true;
                }
                ActivityLog.Logger.LogMessage("Change user failed!");
                MessageBox.Show("אין אפשרות להתחבר למערכת, אנא בדקו את פרטי ההתחברות.");
                return false;
            }
            catch (Exception ex)
            {
                ActivityLog.Logger.LogError(ex);
                MessageBox.Show("אין אפשרות להתחבר למערכת, אנא בדקו את פרטי ההתחברות.");
                return false;
            }
        }

        public bool IsInited()
        {
            return User != null;
        }
    }
}
