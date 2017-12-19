using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp3
{
    class VIPKid
    {
        const string LOGIN_PAGE = @"https://www.vipkid.com.cn/login";
        const string PARENT_HOME = @"https://www.vipkid.com.cn/parent/home";
        const string PRESCHDULE = @"https://www.vipkid.com.cn/parent/preschedule";
        IWebDriver d;
        WebDriverWait wait;
        DateTime startTime = DateTime.Now;
        Timer timer = null;
        string aUser, aPass, aTeacher;
        enum StateMachine
        {
            NotLogin,Login,Finished
        }
        StateMachine state = StateMachine.NotLogin;
        public VIPKid(string u,string p,string t,DateTime time)
        {
            aUser = u;
            aPass = p;
            aTeacher = t;
            startTime = time;
            
            d = new ChromeDriver("./");
            wait = new WebDriverWait(d, TimeSpan.FromSeconds(100));
            System.Threading.Thread.Sleep(10 * 1000);
            timer = new Timer(TimerCallback,null,1000,1000);
        }
        public void TimerCallback(object arg)
        {
            if (state == StateMachine.NotLogin ) return;
            if(state == StateMachine.Finished)
            {
                //停止定时器
                return;
            }

            DateTime now = DateTime.Now;
            if(now<startTime)
            {
                TimeSpan ts = startTime - now;
                Log.D("离抢课还有 {0}秒", (int)ts.TotalSeconds);
            }
            else
            {
                Log.D("开始抢课！");
                state = StateMachine.Finished;
                FindTeacher();
                ChooseClass();
                
            }
        }
        public void Run()
        {
            
            Login();
            PreSchedule();
            FindTeacher();
            ChooseClass();
            state = StateMachine.Login;
        }
       
        public IWebElement FindElement(By by,string inf)
        {
            Log.D("-- 查找元素:"+inf);
            try
            {
                return d.FindElement(by);
            }
            catch(Exception e)
            {
                Log.E(e);
            }
            Log.D("## 查找元素失败:"+ inf);
            return null;
        }
        public void Login()
        {
            Log.D("进入login");
            string u = aUser, p = aPass;
            d.Url = LOGIN_PAGE;
            wait.Until((drv) => { return FindElement(By.Name("username"),"username") != null; });
            wait.Until((drv) => { return FindElement(By.Name("password"),"password") != null; });
            wait.Until((drv) => { return FindElement(By.Id("js-submit-btn"),"登录按钮") != null; });


            var user = FindElement(By.Name("username"),"username");
            var pass = FindElement(By.Name("password"),"password");
            var bt = FindElement(By.Id("js-submit-btn"),"登录按钮");
            if (user!=null && pass!=null && bt!=null)
            {
                user.SendKeys(u);
                pass.SendKeys(p);
                bt.Click();

                
                wait.Until((drv) => { return drv.Url == PARENT_HOME; });

            }
            else
            {
                Log.E("Find UI Error: user={0} pass={1},bt={2}", user, pass, bt);
            }
            Log.D("退出login");
        }

        public void PreSchedule()
        {
            Log.D("进入预约课程");
            //WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(100));
            wait.Until((drv) => { return drv.Url == PARENT_HOME; });
            wait.Until((drv) => { return FindElement(By.LinkText("预约课程"), "预约课程") != null; });

            var btpre = FindElement(By.LinkText("预约课程"), "预约课程");
            if (btpre != null)
            {
                btpre.Click();
                wait.Until((drv) => { return drv.Url == PRESCHDULE; });
            }
            else
            {
                Log.E("find bt error:bt={0}", btpre);
            }
            Log.D("退出预约课程");
        }

        public void FindTeacher()
        {
            Log.D("开始查找老师");
            string teacher = aTeacher;
            string XPath_TeacherBox = @"//div[@id='js-teacher-input']/div[@class='teacher-input']/input[@placeholder='输入老师名字']";
            string XPath_TeacherBT = "//div[@id='js-teacher-input']/div[@class='search-btn']";
            string XPath_Teacher = string.Format("//div[@id='js-teacher-list']/*/p[text()='{0}']", teacher);


            wait.Until((drv) => { return drv.Url == PRESCHDULE; });

            wait.Until((drv) => { return FindElement(By.XPath(XPath_TeacherBox),"查找老师输入框") != null; });
            var teacherinput = FindElement(By.XPath(XPath_TeacherBox), "查找老师输入框");
            if (teacherinput != null)
            {
                teacherinput.Clear();
                teacherinput.SendKeys(teacher);
            }

            wait.Until((drv) => { return FindElement(By.XPath(XPath_TeacherBT),"查找老师按钮") != null; });
            var teacherbt = FindElement(By.XPath(XPath_TeacherBT),"查找老师按钮");
            if (teacherbt != null)
            {
                teacherbt.Click();
            }

            wait.Until((drv) => { return FindElement(By.XPath(XPath_Teacher),"老师列表-选择") != null; });
            var teacherBox = FindElement(By.XPath(XPath_Teacher), "老师列表-选择");
            if (teacherBox != null)
            {
                teacherBox.Click();
            }
            Log.D("结束查找老师");
        }


        public List<IWebElement> GetClassRows()
        {
            List<IWebElement> rows = new List<IWebElement>();
            string tableXPath = @"//*[@id='js-schedule-table-body']/div/table/tbody";
            wait.Until((drv) => { return FindElement(By.XPath(tableXPath),"课程表") != null; });
            var table = FindElement(By.XPath(tableXPath),"课程表");


            string rowXPath = tableXPath + @"/tr";
            wait.Until((drv) => { return FindElement(By.XPath(rowXPath),"课程表-行") != null; });
            rows.AddRange( d.FindElements(By.XPath(rowXPath)));

            
            return rows;
        }

        public List<IWebElement> GetColByRow(IWebElement row)
        {
            List<IWebElement> cols = new List<IWebElement>();
            cols.AddRange(row.FindElements(By.TagName("td")));
            return cols;
        }

        public void ChooseClass()
        {
            Log.D("开始选课");
            try
            {
                List<IWebElement> rows = GetClassRows();
                bool ok = false;
                for (int day = 0; day < 7; day++)
                {
                    ok|=ChooseOneDayClass(rows, day);
                }
                if (!ok)
                {
                    Log.D("没有找到任何可选时间段！");
                }
                return;
            }catch(Exception ee)
            {
                Log.E(ee);

            }
            Log.D("开始选课失败，再次尝试");
            //有异常的话，再次尝试一次
            try
            {
                List<IWebElement> rows = GetClassRows();
                bool ok = false;
                for (int day = 0; day < 7; day++)
                {
                    ok |= ChooseOneDayClass(rows, day);
                }
                if(!ok)
                {
                    Log.D("没有找到任何可选时间段！");
                }
                return;
            }
            catch (Exception ee)
            {
                Log.E(ee);

            }
            Log.D("结束选课");

            //foreach (IWebElement row in rows)
            //{
            //    List<IWebElement> cols = GetColByRow(row);
            //    foreach(IWebElement col in cols)
            //    {
            //        if(col.Text.Contains("可预约"))
            //        {
            //            col.Click();

            //            string confirmbtxpath = @"//*[@id='js-vipkid-modal']/*/button[@class='confirm-btn']";
            //            wait.Until((drv) => { return drv.FindElement(By.XPath(confirmbtxpath)) != null; });
            //            var confirmbt = d.FindElement(By.XPath(confirmbtxpath));
            //            if(confirmbt!=null)
            //            {
            //                confirmbt.Click();
            //            }
            //            return;
            //            //string txt = row.Text + " " + col.Text;
            //            //MessageBox.Show(txt);
            //        }
            //    }
            //}
        }

        private bool ChooseOneDayClass(List<IWebElement> rows,int day)
        {
            
            string[] times = { "19:30", "20:00", "20:30", "19:00" };
            int[] timeIdx = { 22,23,24,21};
            foreach (string t in times)
            {
                foreach (int ts in timeIdx)
                {
                    IWebElement row = rows[ts];
                    List<IWebElement> cols = GetColByRow(row);

                    if (cols.Count > day &&
                        row.Text.Contains(t) &&
                        cols[day].Text.Contains("可预约"))
                    {
                        Log.D("找到可预约时间段: {0}  TS:{1}-{2}", day, ts, times[ts - 21]);

                        //cols[day].Click();
                        //string confirmbtxpath = @"//*[@id='js-vipkid-modal']/*/button[@class='confirm-btn']";
                        //wait.Until((drv) => { return FindElement(By.XPath(confirmbtxpath)) != null; });
                        //var confirmbt = FindElement(By.XPath(confirmbtxpath));
                        //if (confirmbt != null)
                        //{
                        //    confirmbt.Click();
                        //}
                        Log.D("Choose Day: {0}  TS:{1}-{2}", day, ts, times[ts - 21]);
                        return true;

                    }
                }
            }
            return false;
            
        }
    }
}
