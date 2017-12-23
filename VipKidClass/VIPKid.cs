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
using VipKidClass;

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
        WebDriverWait shortWait;
        DateTime startTime = DateTime.Now;
        string aUser, aPass, aTeacher;
        int Mode = 1;
        public VIPKid(string u,string p,string t,DateTime time,int mode)
        {
            Mode = mode;
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
                    ChooseClassByMode(Mode);
                    break;
                }
            }
        }
        void FindTeacherSafe()
        {
            while(true)
            {
                try
                {
                    FindTeacher();
                    return;
                }
                catch(Exception ee)
                {
                    Log.E(ee);
                }
            }
        }
        public void Run()
        {
            
            d = new ChromeDriver("./");
            wait = new WebDriverWait(d, TimeSpan.FromSeconds(20));
            shortWait = new WebDriverWait(d, TimeSpan.FromSeconds(4));
            System.Threading.Thread.Sleep(10 * 1000);

            Login();
            PreSchedule();
            FindTeacherSafe();
            Log.D("登录成功，等待开始抢课############");
            //FindTeacher();
            //ChooseClass();
            //state = StateMachine.Login;
            ts = TimeSpan.Zero;
            ChooseTeacherClass();

            Log.D("获得Text总共耗时：{0}毫秒", ts.TotalMilliseconds);
            Log.D("自动选课结束@@@@@@@@@@@@@@@@@@@@@@");
        }
       
        private IWebElement FindElement(By by,string inf)
        {
            Log.D("《-- 查找元素:"+inf);
            try
            {
                var tmp = d.FindElement(by);
                Log.D("返回查找元素:" + inf+"--》");
                return tmp;
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
            //选择老师
            Log.D("选择老师:"+aTeacher+"============================");
            string XPath_Teacher = string.Format("//div[@id='js-teacher-list']/*/p[text()='{0}']", aTeacher);
            shortWait.Until((drv) => { return FindElement(By.XPath(XPath_Teacher), "老师列表-选择") != null; });
            var teacherBox = FindElement(By.XPath(XPath_Teacher), "老师列表-选择");
            if (teacherBox != null)
            {
                teacherBox.Click();
                Log.D("选择老师==结束1");
            }

            //等待Loading完成
            System.Threading.Thread.Sleep(50);
            shortWait.Until((drv) => 
            {
                string loadingXPath = @"//*[@id='js-schedule-table-body']/div/div[@class='loading hide']";
                return FindElement(By.XPath(loadingXPath), "loading界面") != null;
            });
            Log.D("选择老师==结束2");
        }

        private List<IWebElement> GetAvaibleClassRows()
        {
            List<IWebElement> rows = new List<IWebElement>();

            string tableXPath = @"//*[@id='js-schedule-table-body']/div/table/tbody";
            //等待存在可预约的课程
            wait.Until((drv) =>
            {
                try
                {
                    var tmpTable = FindElement(By.XPath(tableXPath), "课程表");
                    if (tmpTable != null)
                    {
                        Log.D("<<BEGIN GetText");
                        string all = GetTxt(tmpTable);
                        Log.D("GetText  END>>");
                        return all.Contains("可预约");
                    }
                }catch(Exception e)
                {
                    Log.E(e);
                }
                return false;
            });
            var table = FindElement(By.XPath(tableXPath),"课程表");


            string rowXPath = tableXPath + @"/tr";
            wait.Until((drv) => { return FindElement(By.XPath(rowXPath),"课程表-行") != null; });
            rows.AddRange( d.FindElements(By.XPath(rowXPath)));

            Log.D("获得所有行："+rows.Count);
            return rows;
        }

        private List<IWebElement> GetColByRow(IWebElement row)
        {
            Log.D("获得所有列：" + GetTxt(row));
            List<IWebElement> cols = new List<IWebElement>();
            cols.AddRange(row.FindElements(By.TagName("td")));
            Log.D("获得所有列结束，总共有：" + cols.Count);
            return cols;
        }

        //获得一个可预约课程
        private IWebElement GetAvaibleClass(int mode)
        {
            Log.D("检查星期{0}的课程",mode+1);
            ClassInf inf = new ClassInf();
            inf.Init(aTeacher, startTime);
            IClassXPath classes = inf.GetClass(mode);
            try
            {
                string[] xpaths = classes.ClassXPaths;
                IWebElement[] elms = new IWebElement[xpaths.Length];
                string[] txts = new string[xpaths.Length];
                for(int i = 0;i<xpaths.Length;i++)
                {
                    elms[i] = FindElement(By.XPath(xpaths[i]),"查找i节课："+i);
                    if (elms[i] != null)
                    {
                        txts[i] = elms[i].Text;
                        if (txts[i].Contains("可预约"))
                        {
                            Log.D("发现可预约课程：时间：星期{0},课时i={1},xpath={2},txt={3}",mode+1,i,xpaths[i],txts[i]);
                            return elms[i];
                        }
                    }
                }
            }
            catch(Exception ee)
            {
                Log.E(ee);
            }
            return null;
        }

        private void ChooseClassByMode(int mode)
        {
            Log.D("开始选课");
            ChooseTeacher();
            IWebElement c = GetAvaibleClass(mode);
            if(c!=null)
            {
                Log.D("发现可预约时间段: 星期{0}  TS:{1}-{2}，开始点击", mode+1,"?","?");
                c.Click();
                Thread.Sleep(50);

                //一路点击确认
                ClickConfirmAllButton();

                
            }
        }
        private void ChooseClass()
        {
            Log.D("开始选课=========================");
            ChooseTeacher();
            
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

            
        }

        private IWebElement GetConfirmButton()
        {
            try
            {
                string confirmbtxpath = @"//*[@id='js-vipkid-modal']/*/button[@class='confirm-btn']";
                var confirmbt = FindElement(By.XPath(confirmbtxpath), "确认选择课程");
                return confirmbt;
            }
            catch(Exception e)
            {
                Log.E(e);
            }
            return null;
        }
        private void ClickConfirmAllButton()
        {
            for (int i = 0; i < 5; i++)
            {
                Log.D("等待弹出确认窗口,第{0}次尝试",i);
                try
                {
                    var confirmbt = GetConfirmButton();
                    if (confirmbt != null)
                    {
                        Log.D("发现确认按钮，点击确认！");
                        confirmbt.Click();
                        Thread.Sleep(100);
                    }
                    else
                    {
                        Thread.Sleep(20);
                    }
                }catch(Exception ee)
                {
                    Log.E(ee);
                }
            }
        }

        TimeSpan ts = TimeSpan.Zero;
        private string GetTxt(IWebElement elm)
        {
            DateTime n1 = DateTime.Now;
            string txt = elm.Text;
            DateTime n2 = DateTime.Now;
            ts += (n2 - n1);
            return txt;
        }
        private bool ChooseOneDayClass(int day)
        {
            Log.D("开始选择星期{0}的课程===========",day+1);
            List<IWebElement> rows = GetAvaibleClassRows();

            
            for(int i = 0;i<TIMES.Length;i++)
            {
                string t = TIMES[i];
                int    ts = TIME_INDEX[i];
                IWebElement row = rows[ts];
                List<IWebElement> cols = GetColByRow(row);
                Log.D("检查时间段的状态：星期{0},时间段：{1}-{2}",day+1,ts,t);
                Log.D("状态为：列数{0},row.Text={1},col.Txt={2}", cols.Count, GetTxt(row), GetTxt(cols[day]));
                if (cols.Count > day &&
                    GetTxt(row).Contains(t) &&
                    GetTxt(cols[day]).Contains("可预约"))
                {
                    if (false)
                    {
                        Log.D("发现可预约时间段: 星期{0}  TS:{1}-{2}，开始点击", day + 1, ts, t);
                        cols[day].Click();
                        Thread.Sleep(50);

                        //一路点击确认
                        ClickConfirmAllButton();

                        Log.D("Choose Day: 星期{0}  TS:{1}-{2}", day + 1, ts, t);

                        //不健壮？？？等待刷新表格状态,状态应该变为==非可预约
                        Log.D("确认结果，等待页面更新");
                        try
                        {
                            shortWait.Until((drv) =>
                            {
                                rows = GetAvaibleClassRows();
                                row = rows[ts];
                                cols = GetColByRow(row);
                                Log.D("状态为：列数{0},row.Text={1},col.Txt={2}", cols.Count, GetTxt(row), GetTxt(cols[day]));
                                return !GetTxt(cols[day]).Contains("可预约");
                            });
                            Log.D("确认结果，页面已更新");
                        }
                        catch (Exception ee)
                        {
                            Log.E(ee);
                            Log.D("确认结果失败，放弃本次操作");
                        }
                    }
                    return true;
                }
                    
            }
            return false;
            
        }
    }
}
