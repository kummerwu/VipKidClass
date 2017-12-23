using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp3
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static int Week = 3;
        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                int w = 3;
                if(int.TryParse(e.Args[0],out w))
                {
                    Week = w;

                }
                if (Week < 1) Week = 1;
                if (Week > 7) Week = 7;
            }
            base.OnStartup(e);
        }
    }
}
