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
        public static void D(string fmt,params object[] args)
        {
            if(box!=null)
            {
                string inf =DateTime.Now.ToLongTimeString()+ string.Format(fmt, args)+"\r\n";
                box.Dispatcher.Invoke(new Action(()=>box.ShowLog(inf)));
                //box.AppendText(inf + "\r\n");
            }
        }
        public static void E(Exception e)
        {
            string inf = "Exception Happend======================\r\n" + e.Message + "\r\n" + e.StackTrace + "\r\nException End===================\r\n";
            if (box != null)
            {
                box.Dispatcher.Invoke(new Action(() => box.ShowLog(DateTime.Now.ToLongTimeString()+ inf)));
            }
        }
        public static void E(string fmt, params object[] args)
        {
            D(fmt, args);
        }
    }
}
