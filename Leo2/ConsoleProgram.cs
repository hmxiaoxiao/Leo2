using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Leo2.Helper;
using Leo2.Model;
using DevExpress.Xpo.DB;
using DevExpress.Xpo;
using Leo2.Controller;

namespace Leo2.Rule
{
    static class ConsoleProgram
    {
        /// <summary>
        /// 测试用的主程序入口
        /// </summary>
        [STAThread]
        static void Main()
        {
            LeoController.InitDatabase();       // 初始化数据库

            Web myweb = new Web(XpoDefault.Session);
            //myweb.Encoding = "gb2312";      // 这个根据情况设置
            myweb.URL = "http://www.namkwong.com.mo/enews/leader/";
            BaseRule rule = myweb.Rule;
            int i = rule.MaxPage;
            Console.WriteLine(@"总共有{0}页", i);

            rule.PageScanComplete += ShowMessage;

            rule.PrepareScan();

            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }

        private static int m_count = 0;

        public static void ShowMessage(object sender, BaseRule.ScanCompleteEventArgs e)
        {
            m_count ++;
            Console.WriteLine(@"已经下载了{0}页，共{1}页", m_count, e.web.MaxPage);
        }
    }
}
