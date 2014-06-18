using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Leo2.Helper;
using Leo2.Model;
using DevExpress.Xpo.DB;
using DevExpress.Xpo;

namespace Leo2
{
    static class ConsoleProgram
    {
        /// <summary>
        /// 测试用的主程序入口
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 设置当前的数据库联接
            string ConnectionString = AccessConnectionProvider.GetConnectionString("Web.mdb"); //SQLiteConnectionProvider.GetConnectionString("Web.DB");
            XpoDefault.DataLayer = XpoDefault.GetDataLayer(ConnectionString, AutoCreateOption.DatabaseAndSchema);


            Web myweb = new Web();
            myweb.URL = "http://www.sasac.gov.cn/n1180/n20240/n20259/index.html";
            m_sasac = new www_sasac_gov_cn(myweb);
            int i = m_sasac.MaxPage;
            Console.WriteLine(@"总共有{0}页", i);

            m_sasac.DownPage += ShowMessage;

            m_sasac.GetPagesOnList();

            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }

        private static int m_count = 0;
        private static www_sasac_gov_cn m_sasac;

        public static void ShowMessage(object sender, BaseWeb.DownPageEventArgs e)
        {
            m_count ++;
            Console.WriteLine(@"已经下载了{0}页，共{1}页", m_count, m_sasac.MaxPage);
        }
    }
}
