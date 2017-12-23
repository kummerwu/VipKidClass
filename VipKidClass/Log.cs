using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp3
{
    class Log
    {
        public static MainWindow box = null;
        public static string LogFile = "./vipkid_"+DateTime.Now.ToString("HH_mm_ss_fff")+".log";
        public static string NOW
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss:fff");
            }
        }
        public static void D(string fmt,params object[] args)
        {
            string inf =  NOW + string.Format(fmt, args) + "\r\n";
            LogRec(inf);
        }
        public static void E(Exception e)
        {
            string inf = NOW + "Exception Happend======================\r\n" + e.Message + "\r\n" + e.StackTrace + "\r\nException End===================\r\n";
            LogRec(inf);
        }

        private static void LogRec(string inf)
        {
            if (box != null)
            {
                box.Dispatcher.Invoke(new Action(() => box.ShowLog(inf)));
            }
            System.Diagnostics.Debug.WriteLine(inf);
            System.IO.File.AppendAllText(LogFile, inf);
        }

        public static void E(string fmt, params object[] args)
        {
            D(fmt, args);
        }
    }
}
