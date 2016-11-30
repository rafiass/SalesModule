
namespace SalesModule.Services
{
    internal class ConnectionService
    {
        public static string StoresConn { get; set; }

        static ConnectionService()
        {
            StoresConn = "";
        }

        public static string CreateConnectionString(string ip, string username, string password, string catalog, string failover = "")
        {
            string modulename = "SalesModule";
            string cs = "Data Source = " + ip + "; Initial Catalog = " + catalog + "; " +
                "User ID = " + username + "; Password = " + password + "; " +
                "Pooling = false; Encrypt = True; Persist Security Info = false; Connect Timeout = 12" + "; TrustServerCertificate = true; ";

            cs += (failover != "" ? "Failover Partner = " + failover + "; " : "");
            cs += (modulename != "" ? "Application Name = " + modulename : "");
            return cs;
        }

        public static string GetLocalConnectionString()
        {
            string srv = "server.neworder.co.il", catalog = "websitedb", failover = "37.19.115.236",
                user = "neworder", password = "R0h3niu123";

            return CreateConnectionString(srv, user, password, catalog, failover);
        }
    }
}
