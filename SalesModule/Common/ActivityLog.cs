using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SalesModule
{
    internal class ActivityLog
    {
        private static ActivityLog _logger;
        private const string FILE_NAME = "Sales.log.txt";

        private string _path;

        public ActivityLog()
        {
            _path = Common.CurrentDirectory + "\\" + FILE_NAME;
            using (File.Create(_path)) ;
        }
        public static ActivityLog Logger { get { return _logger ?? (_logger = new ActivityLog()); } }

        public void LogCall(params object[] arguments)
        {
            var msg = callerName() + "(";
            if (arguments != null && arguments.Length > 0)
                msg += arguments.Aggregate("", (acc, arg) => acc + arg + ", ",
                    acc => acc.Substring(0, acc.Length - 2));
            msg += ")";
            writeMessage(msg);
        }
        public void LogError(Exception ex, string msg = null)
        {
            writeMessage("An error occurred in function '" + callerName() + "'.");
            if (!string.IsNullOrEmpty(msg)) writeMessage(msg);
            writeMessage("Exception message:");
            writeMessage(ex.Message);
            writeMessage("Full stacktrace:");
            writeMessage(ex.StackTrace);
        }
        public void LogMessage(string msg)
        {
            writeMessage(callerName() + ": " + msg);
        }

        private string callerName()
        {
            var method = (new StackTrace()).GetFrame(2).GetMethod();
            return method.DeclaringType + "." + method.Name;
        }
        private void writeMessage(string s)
        {
            StreamWriter sw;
            using (sw = File.AppendText(_path))
                sw.WriteLine(s);
        }
    }
}
