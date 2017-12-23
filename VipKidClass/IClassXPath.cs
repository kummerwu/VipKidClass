using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VipKidClass
{
    class IClassXPath
    {
        public string Teacher;
        public DateTime StartTime;
        public string[] ClassXPaths;
    }

    class ClassInf
    {
        public string Teacher1 = "Kara XA";
        public DateTime Start1 = DateTime.Now;

        static string XPath_1_7_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[21]/td[1]/div";
        static string XPath_1_7_3 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[22]/td[1]/div";
        static string XPath_1_8_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[23]/td[1]/div";
        static string XPath_1_8_3 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[24]/td[1]/div";
        static string XPath_1_9_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[25]/td[1]/div";

        static string XPath_2_7_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[21]/td[2]/div";
        static string XPath_2_7_3 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[22]/td[2]/div";
        static string XPath_2_8_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[23]/td[2]/div";
        static string XPath_2_8_3 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[24]/td[2]/div";
        static string XPath_2_9_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[25]/td[2]/div";

        static string XPath_3_7_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[21]/td[3]/div";
        static string XPath_3_7_3 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[22]/td[3]/div";
        static string XPath_3_8_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[23]/td[3]/div";
        static string XPath_3_8_3 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[24]/td[3]/div";
        static string XPath_3_9_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[25]/td[3]/div";

        static string XPath_4_7_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[21]/td[4]/div";
        static string XPath_4_7_3 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[22]/td[4]/div";
        static string XPath_4_8_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[23]/td[4]/div";
        static string XPath_4_8_3 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[24]/td[4]/div";
        static string XPath_4_9_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[25]/td[4]/div";

        static string XPath_5_7_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[21]/td[5]/div";
        static string XPath_5_7_3 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[22]/td[5]/div";
        static string XPath_5_8_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[23]/td[5]/div";
        static string XPath_5_8_3 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[24]/td[5]/div";
        static string XPath_5_9_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[25]/td[5]/div";

        static string XPath_6_7_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[21]/td[6]/div";
        static string XPath_6_7_3 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[22]/td[6]/div";
        static string XPath_6_8_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[23]/td[6]/div";
        static string XPath_6_8_3 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[24]/td[6]/div";
        static string XPath_6_9_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[25]/td[6]/div";

        static string XPath_7_7_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[21]/td[7]/div";
        static string XPath_7_7_3 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[22]/td[7]/div";
        static string XPath_7_8_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[23]/td[7]/div";
        static string XPath_7_8_3 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[24]/td[7]/div";
        static string XPath_7_9_0 = @"//*[@id='js-schedule-table-body']/div/table/tbody/tr[25]/td[7]/div";


        IClassXPath[] All = null;
        public void Init(string t, DateTime st)
        {
            Teacher1 = t;
            Start1 = st;
            All = new IClassXPath[]
            {
                new IClassXPath(){
                    Teacher =Teacher1,
                    StartTime = Start1,
                    ClassXPaths = new string[]{XPath_1_7_3,XPath_1_8_0,XPath_1_7_0,XPath_1_8_3,XPath_1_9_0 }
                },

                new IClassXPath(){
                    Teacher =Teacher1,
                    StartTime = Start1,
                    ClassXPaths = new string[]{XPath_2_7_3,XPath_2_8_0,XPath_2_7_0,XPath_2_8_3,XPath_2_9_0 }
                },

                new IClassXPath(){
                    Teacher =Teacher1,
                    StartTime = Start1,
                    ClassXPaths = new string[]{XPath_3_7_3,XPath_3_8_0,XPath_3_7_0,XPath_3_8_3,XPath_3_9_0 }
                },

                new IClassXPath(){
                    Teacher =Teacher1,
                    StartTime = Start1,
                    ClassXPaths = new string[]{XPath_4_7_3,XPath_4_8_0,XPath_4_7_0,XPath_4_8_3,XPath_4_9_0 }
                },

                new IClassXPath(){
                    Teacher =Teacher1,
                    StartTime = Start1,
                    ClassXPaths = new string[]{XPath_5_7_3,XPath_5_8_0,XPath_5_7_0,XPath_5_8_3,XPath_5_9_0 }
                },

                new IClassXPath(){
                    Teacher =Teacher1,
                    StartTime = Start1,
                    ClassXPaths = new string[]{XPath_6_7_3,XPath_6_8_0,XPath_6_7_0,XPath_6_8_3,XPath_6_9_0 }
                },

                new IClassXPath(){
                    Teacher =Teacher1,
                    StartTime = Start1,
                    ClassXPaths = new string[]{XPath_7_7_3,XPath_7_8_0,XPath_7_7_0,XPath_7_8_3,XPath_7_9_0 }
                },
          };
        }

        public IClassXPath GetClass(int mode)
        {
            return All[mode];
        }
    }
}

