using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace WpfApp3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string[] ts = new string[100];
            for(int i = 0;i<ts.Length;i++)
            {
                ts[i] = Log.NOW;
                Thread.Sleep(1);
            }
            //string t = Log.NOW;
            

        }
        public void testbiadu()
        {
            IWebDriver d = new ChromeDriver("./");
            d.Url = @"http://www.baidu.com";
            WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(100));

            var map = d.FindElement(By.LinkText("地图"));
            if(map!=null)
            {
                map.Click();
            }

            wait.Until((drv) => { return drv.Url.Contains(@"map.baidu.com"); });

            wait.Until((drv) => { return drv.FindElement(By.Id("sole-input"))!=null; });
            var input = d.FindElement(By.Id("sole-input"));
            if(input!=null)
            {
                input.SendKeys("chuidaohuacheng");
            }
            var search = d.FindElement(By.Id("search-button"));
            if(search!=null)
            {
                search.Click();
            }
        }
        public void test1()
        {
            IWebDriver d = new ChromeDriver("./");
            d.Url = @"https://www.vipkid.com.cn/login";

            var user = d.FindElement(By.Name("username"));
            var pass = d.FindElement(By.Name("password"));

            var bt = d.FindElement(By.Id("js-submit-btn"));
            if (user != null && pass != null && bt != null)
            {
                user.SendKeys("13655190156");
                pass.SendKeys("asdf1234");

                bt.Click();

                WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(100));
                wait.Until((drv) => { return drv.Url == "https://www.vipkid.com.cn/parent/home"; });

                wait.Until((drv) => { return drv.FindElement(By.LinkText("预约课程"))!=null; });

                var btpre = d.FindElement(By.LinkText("预约课程"));
                if (btpre != null)
                {
                    btpre.Click();
                }

                string urlpre = @"https://www.vipkid.com.cn/parent/preschedule";
                wait.Until((drv) => { return drv.Url == urlpre; });
                
                var teacherinput = d.FindElement(By.XPath(@"//div[@id='js-teacher-input']/div[@class='teacher-input']/input[@placeholder='输入老师名字']"));
                if(teacherinput!=null)
                {
                    teacherinput.SendKeys("Kara H");
                }

                var teacherbt = d.FindElement(By.XPath("//div[@id='js-teacher-input']/div[@class='search-btn']"));
                if(teacherbt!=null)
                {
                    teacherbt.Click();
                }

                var karah = d.FindElement(By.XPath("//div[@id='js-teacher-list']/div[@class='teacher-list-item active']"));
                if(karah!=null)
                {
                    karah.Click();
                }

                var table = d.FindElement(By.XPath("//table[@class='schedule-table-body'/tbody/tr/th[text()=='21:00']"));
                

            }
        }

        public void ShowLog(string inf)
        {
            
            TxtLog.AppendText(inf);
            TxtLog.ScrollToEnd();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Log.box = this;
            //vip.Login("13655190156", "asdf1234");
            //vip.PreSchedule();
            //vip.FindTeacher("Kara ED");
            //vip.ChooseClass();
            string teacher = TxtTeacher.Text;
            DateTime now = DateTime.Now;
            int hour=13, min=0, sec=0;
            string time = TxtTime.Text;
            string[] ts = time.Split(new char[] { ':' ,'：','-','-'});
            if(ts.Length==3)
            {
                hour = int.Parse(ts[0]);
                min = int.Parse(ts[1]);
                sec = int.Parse(ts[2]);
            }
            DateTime chooseTime = new DateTime(now.Year, now.Month, now.Day, hour, min, sec);
            try
            {
                VIPKid vip = new VIPKid("13655190156", "asdf1234", teacher,chooseTime);
                Thread thread = new Thread(() => { vip.Run(); });
                thread.Start();
            }catch(Exception ee)
            {
                Log.E(ee);
            }
        }
    }
}
