﻿using System;
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
            myweb.URL = "http://www.casic.com.cn/n99188/n470321/index.html";
            m_sasac = new www_casic_com_cn(myweb);
            int i = m_sasac.MaxPage;
            Console.WriteLine(@"总共有{0}页", i);

            m_sasac.PageScanComplete += ShowMessage;

            m_sasac.PrepareScan();

            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }

        private static int m_count = 0;
        private static www_casic_com_cn m_sasac;

        public static void ShowMessage(object sender, BaseRule.ScanCompleteEventArgs e)
        {
            m_count ++;
            Console.WriteLine(@"已经下载了{0}页，共{1}页", m_count, m_sasac.MaxPage);
        }
    }
}
