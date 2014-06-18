using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Leo2.View;
using Leo2.Controller;
using Leo2.Helper;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.XtraSplashScreen;

namespace Leo2
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 设置当前的数据库联接
            string ConnectionString = AccessConnectionProvider.GetConnectionString("Web.mdb"); //SQLiteConnectionProvider.GetConnectionString("Web.DB");
            XpoDefault.DataLayer = XpoDefault.GetDataLayer(ConnectionString, AutoCreateOption.DatabaseAndSchema);

            //  设置皮肤
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DevExpress.Skins.SkinManager.EnableFormSkins();

            // 增加汉化
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");


            // 显示主窗口
            ////SplashScreenManager.ShowForm(typeof(frmWelcome), true, true);
            //LeoController mainController = new LeoController();
            ////mainController.RunTest();
            //mainController.View = new frmMain(mainController);
            //Application.Run(mainController.View);

            www_sasac_gov_cn sasac = new www_sasac_gov_cn("http://www.sasac.gov.cn/n1180/n20240/n20259/index.html");
            int i = sasac.MaxPage;

            Console.ReadKey();
            
        }
    }
}
