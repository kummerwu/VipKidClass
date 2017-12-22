﻿using OpenQA.Selenium;
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
        int[] DAYS = { 2,3,4,6,7 };
        string[] TIMES = { "19:30", "20:00", "20:30", "19:00" };
        int[] TIME_INDEX = { 21, 22, 23, 20 };
        const string LOGIN_PAGE = @"https://www.vipkid.com.cn/login";
        const string PARENT_HOME = @"https://www.vipkid.com.cn/parent/home";
        const string PRESCHDULE = @"https://www.vipkid.com.cn/parent/preschedule";
        IWebDriver d;
        WebDriverWait wait;
        DateTime startTime = DateTime.Now;
        string aUser, aPass, aTeacher;
        public VIPKid(string u,string p,string t,DateTime time)
        {
            for(int i = 0;i<DAYS.Length;i++)
            {
                DAYS[i]--;//索引从0开始，星期一的索引是0，而不是1，调整一下
            }
            aUser = u;
            aPass = p;
            aTeacher = t;
            startTime = time;
            
            
            //timer = new Timer(TimerCallback,null,1000,1000);
        }
        private void ChooseTeacherClass()
        {
            while (true)
            {
                DateTime now = DateTime.Now;
                if (now < startTime)
                {
                    TimeSpan ts = startTime - now;
                    Log.D("离抢课还有 {0}秒", (int)ts.TotalSeconds);
                    if(ts.TotalSeconds<400 && ts.TotalSeconds>5)
                    {
                        ChooseTeacher();
                        Log.D("点击老师，刷新老师课程表");
                        Thread.Sleep(5000);
                    }
                    else if(ts.TotalSeconds>2)
                    {
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Thread.Sleep(200);
                    }
                }
                else
                {
                    Log.D("开始抢课！");
                    ChooseClass();
                    break;
                }
            }
        }
        public void Run()
        {
            d = new ChromeDriver("./");
            wait = new WebDriverWait(d, TimeSpan.FromSeconds(20));
            System.Threading.Thread.Sleep(10 * 1000);

            Login();
            PreSchedule();
            FindTeacher();
            Log.D("登录成功，等待开始抢课############");
            //FindTeacher();
            //ChooseClass();
            //state = StateMachine.Login;
            ChooseTeacherClass();
        }
       
        private IWebElement FindElement(By by,string inf)
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
        private void Login()
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

        private void PreSchedule()
        {
            Log.D("进入预约课程===================");
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

        private void FindTeacher()
        {
            Log.D("开始查找老师====================");
            string XPath_TeacherBox = @"//div[@id='js-teacher-input']/div[@class='teacher-input']/input[@placeholder='输入老师名字']";
            string XPath_TeacherBT = "//div[@id='js-teacher-input']/div[@class='search-btn']";



            wait.Until((drv) => { return drv.Url == PRESCHDULE; });

            wait.Until((drv) => { return FindElement(By.XPath(XPath_TeacherBox), "查找老师输入框") != null; });
            var teacherinput = FindElement(By.XPath(XPath_TeacherBox), "查找老师输入框");
            if (teacherinput != null)
            {
                teacherinput.Clear();
                teacherinput.SendKeys(aTeacher);
                Log.D("输入老师名称");
            }

            wait.Until((drv) => { return FindElement(By.XPath(XPath_TeacherBT), "查找老师按钮") != null; });
            var teacherbt = FindElement(By.XPath(XPath_TeacherBT), "查找老师按钮");
            if (teacherbt != null)
            {
                teacherbt.Click();
                Log.D("点击查找老师");
            }

            ChooseTeacher();
            Log.D("结束查找老师");
        }

        private void ChooseTeacher()
        {
            Log.D("选择老师:"+aTeacher+"============================");
            string XPath_Teacher = string.Format("//div[@id='js-teacher-list']/*/p[text()='{0}']", aTeacher);
            wait.Until((drv) => { return FindElement(By.XPath(XPath_Teacher), "老师列表-选择") != null; });
            var teacherBox = FindElement(By.XPath(XPath_Teacher), "老师列表-选择");
            if (teacherBox != null)
            {
                teacherBox.Click();
                Log.D("选择老师==结束1");
            }
            Log.D("选择老师==结束2");
        }

        private List<IWebElement> GetClassRows()
        {
            List<IWebElement> rows = new List<IWebElement>();

            string tableXPath = @"//*[@id='js-schedule-table-body']/div/table/tbody";
            wait.Until((drv) => { return FindElement(By.XPath(tableXPath),"课程表") != null; });
            var table = FindElement(By.XPath(tableXPath),"课程表");


            string rowXPath = tableXPath + @"/tr";
            wait.Until((drv) => { return FindElement(By.XPath(rowXPath),"课程表-行") != null; });
            rows.AddRange( d.FindElements(By.XPath(rowXPath)));

            Log.D("获得所有行："+rows.Count);
            return rows;
        }

        private List<IWebElement> GetColByRow(IWebElement row)
        {
            Log.D("获得所有列：" + row.Text);
            List<IWebElement> cols = new List<IWebElement>();
            cols.AddRange(row.FindElements(By.TagName("td")));
            Log.D("获得所有列结束，总共有：" + cols.Count);
            return cols;
        }

        private void ChooseClass()
        {
            Log.D("开始选课=========================");
            ChooseTeacher();
            //这个地方有bug？选择老师后，需要等待结果更新
            try
            {
                bool ok = false;
                foreach(int day in DAYS)
                {
                    ok|=ChooseOneDayClass( day);
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
                
                bool ok = false;
                foreach (int day in DAYS)
                {
                    ok |= ChooseOneDayClass( day);
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

        private bool ChooseOneDayClass(int day)
        {
            Log.D("开始选择星期{0}的课程===========",day+1);
            List<IWebElement> rows = GetClassRows();

            //foreach (string t in TIMES)
            //{
            //    foreach (int ts in TIME_INDEX)
            //    {
            for(int i = 0;i<TIMES.Length;i++)
            {
                string t = TIMES[i];
                int    ts = TIME_INDEX[i];
                IWebElement row = rows[ts];
                List<IWebElement> cols = GetColByRow(row);
                Log.D("检查时间段的状态：星期{0},时间段：{1}-{2}",day+1,ts,t);
                Log.D("状态为：列数{0},row.Text={1},col.Txt={2}", cols.Count, row.Text, cols[day].Text);
                if (cols.Count > day &&
                    row.Text.Contains(t) &&
                    cols[day].Text.Contains("可预约"))
                {
                    Log.D("发现可预约时间段: 星期{0}  TS:{1}-{2}，开始点击", day+1, ts, t);
                    cols[day].Click();


                    Log.D("等待弹出确认窗口");
                    string confirmbtxpath = @"//*[@id='js-vipkid-modal']/*/button[@class='confirm-btn']";
                    wait.Until((drv) => { return FindElement(By.XPath(confirmbtxpath), "确认选择课程") != null; });
                    var confirmbt = FindElement(By.XPath(confirmbtxpath), "确认选择课程");
                    if (confirmbt != null)
                    {
                        confirmbt.Click();
                    }

                    Log.D("等待弹出最终结果");
                    wait.Until((drv) => { return FindElement(By.XPath(confirmbtxpath), "确认选课结果：成功 或 失败") != null; });
                    var confirmbt2 = FindElement(By.XPath(confirmbtxpath), "确认选课结果：成功 或 失败");
                    if (confirmbt2 != null)
                    {
                        confirmbt2.Click();
                    }
                    Log.D("Choose Day: 星期{0}  TS:{1}-{2}", day+1, ts, t);

                    //等待刷新表格状态,状态应该变为==非可预约
                    Log.D("确认结果，等待页面更新");
                    wait.Until((drv) =>
                    {
                        rows = GetClassRows();
                        row = rows[ts];
                        cols = GetColByRow(row);
                        Log.D("状态为：列数{0},row.Text={1},col.Txt={2}", cols.Count, row.Text, cols[day].Text);
                        return !cols[day].Text.Contains("可预约");
                    });
                    Log.D("确认结果，页面已更新");
                    return true;
                }
                    
            }
            return false;
            
        }
    }
}
