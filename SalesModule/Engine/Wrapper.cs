using SalesModule.Services;
using SalesModule.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SalesModule
{
    public enum InitResults { Success, F_Credentials, F_Unhandled };

    [Guid(Wrapper.InterfaceId)]
    public interface IWrapper
    {
        string Version { get; }
        bool IsEnabled { get; }

        InitResults Init(string store, string userName, string password, int pcid);
        InitResults Init(string ip, string Catalog, string username, string password, int pcid, string empName, string empPass);
        bool ChangeUser(string empName, string empPass);

        SalesEngine CreateEngine();
        bool OpenSalesWindow();
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
        
        internal static UserData User { get; private set; }
        internal static int PCID { get; private set; }


        public string Version
        {
            get
            {
                var v = (new System.Reflection.AssemblyName(
                     System.Reflection.Assembly.GetExecutingAssembly().FullName)).Version;
                return v.Major + "." + v.Minor + "." + v.Build;
            }
        }
        public bool IsEnabled { get { return User != null; } }

        //make private for Singleton purpose
        public Wrapper()
        {
        }
        static Wrapper()
        {
            User = null;
            PCID = -1;
        }

        public InitResults Init(string ip, string Catalog, string username, string password, int pcid, string empName, string empPass)
        {
            ActivityLogService.Logger.LogFunctionCall();
            string connBackup = "";
            try
            {
                UserData user;
                ConnectionService.StoresConn = connBackup = ConnectionService.CreateConnectionString(ip, username, password, Catalog);
                if ((user = DBService.GetService()
                    .GetUserData(empName, empPass)) != null)
                {
                    User = user;
                    PCID = pcid;
                    ActivityLogService.Logger.LogMessage("initiated succssfully, Module version: " + Version);
                    return InitResults.Success;
                }
                ActivityLogService.Logger.LogMessage("Login failed!");
                return InitResults.F_Credentials;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                if (connBackup != "")
                    ConnectionService.StoresConn = connBackup;
                return InitResults.F_Unhandled;
            }
        }
        public InitResults Init(string store, string userName, string password, int pcid)
        {
            ActivityLogService.Logger.LogFunctionCall();
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
                ActivityLogService.Logger.LogMessage("Login failed!");
                return InitResults.F_Credentials;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                return InitResults.F_Unhandled;
            }
        }

        public bool ChangeUser(string empName, string empPass)
        {
            ActivityLogService.Logger.LogFunctionCall(empName, empPass);
            try
            {
                var db = DBService.GetLocalService();
                var user = db.GetUserData(empName, empPass);
                if (user != null)
                {
                    User = user;
                    return true;
                }
                ActivityLogService.Logger.LogMessage("Change user failed!");
                MessageBox.Show("אין אפשרות להתחבר למערכת, אנא בדקו את פרטי ההתחברות.");
                return false;
            }
            catch (Exception ex)
            {
                ActivityLogService.Logger.LogError(ex);
                MessageBox.Show("אין אפשרות להתחבר למערכת, אנא בדקו את פרטי ההתחברות.");
                return false;
            }
        }

        public SalesEngine CreateEngine()
        {
            return new SalesEngine();
        }
        public bool OpenSalesWindow()
        {
            if (User == null)
            {
                MessageBox.Show("אין אפשרות להפעיל מודל המבצעים.\nאירעה שגיאה בזמן טעינת המודל");
                return false;
            }
            InteropService.OpenWindow<MainViewModel>();
            return true;
        }
    }
}
