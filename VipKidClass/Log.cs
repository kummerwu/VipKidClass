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
            System.Diagnostics.Debug.WriteLine(fmt, args);
            if(box!=null)
            {
                string inf = string.Format(fmt, args)+"\r\n";
                box.Dispatcher.Invoke(new Action(()=>box.ShowLog(inf)));
                //box.AppendText(inf + "\r\n");
            }
        }
        public static void E(Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e.Message);
            if (box != null)
            {
                box.Dispatcher.Invoke(new Action(() => box.ShowLog(e.Message + "\r\n")));
                
            }
        }
        public static void E(string fmt, params object[] args)
        {
            D(fmt, args);
        }
    }
}
