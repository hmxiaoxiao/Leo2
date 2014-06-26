using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Leo2.View;
using Leo2.Controller;
using Leo2.Helper;
using Leo2.Model;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.XtraSplashScreen;
using System.Reflection;

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
            LeoController.InitDatabase();       // 初始化数据库

            //// 设置当前的数据库联接
            //string ConnectionString = AccessConnectionProvider.GetConnectionString("Web.mdb"); //SQLiteConnectionProvider.GetConnectionString("Web.DB");
            ////XpoDefault.DataLayer = XpoDefault.GetDataLayer(ConnectionString, AutoCreateOption.DatabaseAndSchema);


            //// 加入线程安全
            //List<Assembly> dataAssemblies = new List<Assembly>();
            //dataAssemblies.Add(typeof(Leo2.Model.Page).Assembly);
            //dataAssemblies.Add(typeof(Leo2.Model.Web).Assembly);

            ////XpoDefault.ConnectionString = AccessConnectionProvider.GetConnectionString("data.mdb");
            ////var dataStore = XpoDefault.GetConnectionProvider(AutoCreateOption.DatabaseAndSchema);

            ////var dict = new ReflectionDictionary();
            ////dict.CollectClassInfos(dataAssemblies.ToArray());
            ////var dataLayer = new ThreadSafeDataLayer(dict, dataStore);

            //DevExpress.Xpo.Metadata.XPDictionary dict = new DevExpress.Xpo.Metadata.ReflectionDictionary();
            //IDataStore store = XpoDefault.GetConnectionProvider(ConnectionString, AutoCreateOption.SchemaAlreadyExists);
            //dict.GetDataStoreSchema(dataAssemblies.ToArray());
            //ThreadSafeDataLayer datalayer = new ThreadSafeDataLayer(dict, store);
            //XpoDefault.DataLayer = datalayer;

            //  设置皮肤
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DevExpress.Skins.SkinManager.EnableFormSkins();

            // 增加汉化
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");


            // 显示主窗口
            //SplashScreenManager.ShowForm(typeof(frmWelcome), true, true);
            LeoController mainController = new LeoController();
            mainController.View = new frmMain(mainController);
            Application.Run(mainController.View);
           
        }
    }
}
