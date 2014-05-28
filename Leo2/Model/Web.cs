using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;


namespace Leo2.Model
{
    /// <summary>
    /// 网站的实体类
    /// </summary>
    public class Web : XPObject
    {
        //public Web(Session session)
        //    : base(session)
        //{
        //}


        private string m_name;
        private string m_url;
        private int m_parent_id;
        private bool m_is_search;
        private string m_list_url_xpath;
        private string m_next_url_xpath;
        private string m_page_xpath;
        private int m_unread;


        public string Title
        {
            get { return m_unread > 0 ? m_name + "(" + m_unread.ToString() + ")" : m_name ; }
        }

        /// <summary>
        /// 网站结点名称
        /// </summary>
        [Size(255)]
        public string Name 
        {
            get { return m_name; }
            set { SetPropertyValue<string>("Name", ref m_name, value); }
        }

        /// <summary>
        /// 该结点对应的URL
        /// </summary>
        [Size(1000)]
        [Indexed(Unique=true)]
        public string URL
        {
            get { return m_url; }
            set { SetPropertyValue<string>("URL", ref m_url, value); }
        }


        /// <summary>
        /// 自己的父结点ID
        /// </summary>
        public int Parent_ID
        {
            get { return m_parent_id; }
            set { SetPropertyValue<int>("Parent_ID", ref m_parent_id, value); } 
        }

        /// <summary>
        /// 这个结点是否需要搜索，通常有子结点的，是不需要搜索的
        /// </summary>
        public bool Is_Search
        {
            get { return m_is_search; }
            set { SetPropertyValue<bool>("Is_Search", ref m_is_search, value); }
        }

        /// <summary>
        /// 列表页中，取得每个页面联接的XPath
        /// </summary>
        [Size(1000)]
        public string List_URL_XPath
        {
            get { return m_list_url_xpath; }
            set { SetPropertyValue<string>("List_URL_XPath", ref m_list_url_xpath, value); }
        }

        /// <summary>
        /// 列表页中，用于取得下一页的XPath路径
        /// </summary>
        [Size(1000)]
        public string Next_URL_XPath
        {
            get { return m_next_url_xpath; }
            set { SetPropertyValue<string>("Next_URL_XPath", ref m_next_url_xpath, value); }
        }

        /// <summary>
        /// page页中，用于提取网页的内容有XPath
        /// </summary>
        [Size(1000)]
        public string Page_XPath
        {
            get { return m_page_xpath; }
            set { SetPropertyValue<string>("Page_XPath", ref m_page_xpath, value); }
        }

        /// <summary>
        /// 未读数量
        /// </summary>
        public int Unread
        {
            get { return m_unread; }
            set { SetPropertyValue<int>("Unread", ref m_unread, value); }
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="web_oid"></param>
        /// <returns></returns>
        public static Web CreateWebWithOID(int web_oid)
        {
            XPCollection<Web> webs = new XPCollection<Web>(XpoDefault.Session,
                Web.Fields.Oid == web_oid);
            if (webs.Count > 0)
                return webs[0];
            return null;
        }

        /// <summary>
        /// 初始化网站的内容
        /// </summary>
        public static void InitWebData()
        {
            XPCollection<Web> webs = new XPCollection<Web>();
            if (webs.Count == 0)
            {
                Web web0 = new Web();
                web0.Parent_ID = 0;
                web0.Name = "国资委";
                web0.Is_Search = false;
                web0.Save();

                Web web01 = new Web();
                web01.Parent_ID = web0.Oid;
                web01.Name = "国资要闻";
                web01.URL = "http://www.sasac.gov.cn/n1180/n20240/n20259/index.html";
                web01.Is_Search = true;
                web01.List_URL_XPath = "//td[@class='black14']/a";
                web01.Next_URL_XPath = "";
                web01.m_page_xpath = "/html[1]/body[1]/table[2]/tr[1]/td[2]/table[11]/tr[1]/td[1]";
                web01.Save();

                Web web06 = new Web();
                web06.Parent_ID = web0.Oid;
                web06.Name = "国有经济";
                web06.URL = "http://www.sasac.gov.cn/n1180/n1271/n20515/n2697206/gyjj.html";
                web06.Is_Search = true;
                web06.List_URL_XPath = "//td[@class='black14']/a";
                web06.Next_URL_XPath = "";
                web06.m_page_xpath = "/html[1]/body[1]/table[2]/tr[1]/td[2]/table[11]/tr[1]/td[1]";
                web06.Save();

                Web web07 = new Web();
                web07.Parent_ID = web0.Oid;
                web07.Name = "国资监管";
                web07.URL = "http://www.sasac.gov.cn/n1180/n1271/n20515/n2697175/gzjg.html";
                web07.Is_Search = true;
                web07.List_URL_XPath = "//td[@class='black14']/a";
                web07.Next_URL_XPath = "";
                web07.m_page_xpath = "/html[1]/body[1]/table[2]/tr[1]/td[2]/table[11]/tr[1]/td[1]";
                web07.Save();

                Web web03 = new Web();
                web03.Parent_ID = web0.Oid;
                web03.Name = "国资改革";
                web03.URL = "http://www.sasac.gov.cn/n1180/n1271/n20515/n2697190/gqgg.html";
                web03.Is_Search = true;
                web03.List_URL_XPath = "//td[@class='black14']/a";
                web03.Next_URL_XPath = "";
                web03.m_page_xpath = "/html[1]/body[1]/table[2]/tr[1]/td[2]/table[11]/tr[1]/td[1]";
                web03.Save();

                Web web04 = new Web();
                web04.Parent_ID = web0.Oid;
                web04.Name = "中央企业动态";
                web04.URL = "http://www.sasac.gov.cn/n1180/n1226/n2410/index.html";
                web04.Is_Search = true;
                web04.List_URL_XPath = "//td[@class='black14']/a";
                web04.Next_URL_XPath = "";
                web04.m_page_xpath = "/html[1]/body[1]/table[2]/tr[1]/td[2]/table[11]/tr[1]/td[1]";
                web04.Save();

                Web web05 = new Web();
                web05.Parent_ID = web0.Oid;
                web05.Name = "地方国资动态";
                web05.URL = "http://www.sasac.gov.cn/n1180/n1271/n1286/n3891/index.html";
                web05.Is_Search = true;
                web05.List_URL_XPath = "//td[@class='black14']/a";
                web05.Next_URL_XPath = "";
                web05.m_page_xpath = "/html[1]/body[1]/table[2]/tr[1]/td[2]/table[11]/tr[1]/td[1]";
                web05.Save();

                Web web10 = new Web();
                web10.Parent_ID = 0;
                web10.Name = "中核网";
                web10.Is_Search = false;
                web10.Save();

                Web web11 = new Web();
                web11.Parent_ID = web10.Oid;
                web11.Name = "中核要闻";
                web11.URL = "http://www.cnnc.com.cn/tabid/293/Default.aspx";
                web11.Is_Search = true;
                web11.List_URL_XPath = "//td[@id='newslist']/a";
                web11.Next_URL_XPath = "";
                web11.m_page_xpath = "//div[@id='ess_ctr2251_ModuleContent']/table";
                web11.Save();

                Web web12 = new Web();
                web12.Parent_ID = web10.Oid;
                web12.Name = "集团快讯";
                web12.URL = "http://www.cnnc.com.cn/publish/portal0/tab664/module2138/more.htm";
                web12.Is_Search = true;
                web12.List_URL_XPath = "//td[@id='newslist']/a";
                web12.Next_URL_XPath = "";
                web12.m_page_xpath = "//div[@id='ess_ctr2251_ModuleContent']/table";
                web12.Save();

                Web web13 = new Web();
                web13.Parent_ID = web10.Oid;
                web13.Name = "一线动态";
                web13.URL = "http://www.cnnc.com.cn/publish/portal0/tab664/module2125/more.htm";
                web13.Is_Search = true;
                web13.List_URL_XPath = "//td[@id='newslist']/a";
                web13.Next_URL_XPath = "";
                web13.m_page_xpath = "//div[@id='ess_ctr2251_ModuleContent']/table";
                web13.Save();

                Web web14 = new Web();
                web14.Parent_ID = web10.Oid;
                web14.Name = "综合资讯";
                web14.URL = "http://www.cnnc.com.cn/publish/portal0/tab664/module2127/more.htm";
                web14.Is_Search = true;
                web14.List_URL_XPath = "//td[@id='newslist']/a";
                web14.Next_URL_XPath = "";
                web14.m_page_xpath = "//div[@id='ess_ctr2251_ModuleContent']/table";
                web14.Save();

                Web web15 = new Web();
                web15.Parent_ID = web10.Oid;
                web15.Name = "媒体聚焦";
                web15.URL = "http://www.cnnc.com.cn/publish/portal0/tab664/module2126/more.htm";
                web15.Is_Search = true;
                web15.List_URL_XPath = "//td[@id='newslist']/a";
                web15.Next_URL_XPath = "";
                web15.m_page_xpath = "//div[@id='ess_ctr2251_ModuleContent']/table";
                web15.Save();

                Web web16 = new Web();
                web16.Parent_ID = web10.Oid;
                web16.Name = "风采中核";
                web16.URL = "http://www.cnnc.com.cn/publish/portal0/tab664/module2128/more.htm";
                web16.Is_Search = true;
                web16.List_URL_XPath = "//td[@id='newslist']/a";
                web16.Next_URL_XPath = "";
                web16.m_page_xpath = "//div[@id='ess_ctr2251_ModuleContent']/table";
                web16.Save();

                Web web20 = new Web();
                web20.Parent_ID = 0;
                web20.Name = "中核建";
                web20.Is_Search = false;
                web20.Save();

                Web web21 = new Web();
                web21.Parent_ID = web20.Oid;
                web21.Name = "重要新闻";
                web21.URL = "http://www.cnecc.com/g783/m877/mp1.aspx";
                web21.Is_Search = true;
                web21.List_URL_XPath = "//div[@class='xwzx-ywzd-item-title']/a";
                web21.Next_URL_XPath = "//a[@class='i-pager-next']";
                web21.m_page_xpath = "//div[@id='dnn_ctr930_ModuleContent']|//div[@id='dnn_ContentPane']";
                web21.Save();

                Web web22 = new Web();
                web22.Parent_ID = web20.Oid;
                web22.Name = "公司新闻";
                web22.URL = "http://www.cnecc.com/g293.aspx";
                web22.Is_Search = true;
                web22.List_URL_XPath = "//div[@class='xwzx-ywzd-item-title']/a";
                web22.Next_URL_XPath = "//a[@class='i-pager-next']";
                web22.m_page_xpath = "//div[@id='dnn_ctr930_ModuleContent']|//div[@id='dnn_ContentPane']";
                web22.Save();

                Web web24 = new Web();
                web24.Parent_ID = web20.Oid;
                web24.Name = "国资动态";
                web24.URL = "http://www.cnecc.com/g672/m1574.aspx";
                web24.Is_Search = true;
                web24.List_URL_XPath = "//div[@class='xwzx-ywzd-item-title']/a";
                web24.Next_URL_XPath = "//a[@class='i-pager-next']";
                web24.m_page_xpath = "//div[@id='dnn_ctr930_ModuleContent']|//div[@id='dnn_ContentPane']";
                web24.Save();

                Web web25 = new Web();
                web25.Parent_ID = web20.Oid;
                web25.Name = "关注与视野";
                web25.URL = "http://www.cnecc.com/g687/m1575.aspx";
                web25.Is_Search = true;
                web25.List_URL_XPath = "//div[@class='xwzx-ywzd-item-title']/a";
                web25.Next_URL_XPath = "//a[@class='i-pager-next']";
                web25.m_page_xpath = "//div[@id='dnn_ctr930_ModuleContent']|//div[@id='dnn_ContentPane']";
                web25.Save();

                Web web26 = new Web();
                web26.Parent_ID = web20.Oid;
                web26.Name = "工程动态";
                web26.URL = "http://www.cnecc.com/g295.aspx";
                web26.Is_Search = true;
                web26.List_URL_XPath = "//div[@class='xwzx-ywzd-item-title']/a";
                web26.Next_URL_XPath = "//a[@class='i-pager-next']";
                web26.m_page_xpath = "//div[@id='dnn_ctr930_ModuleContent']|//div[@id='dnn_ContentPane']";
                web26.Save();

                Web web27 = new Web();
                web27.Parent_ID = web20.Oid;
                web27.Name = "行业资讯";
                web27.URL = "http://www.cnecc.com/g299.aspx";
                web27.Is_Search = true;
                web27.List_URL_XPath = "//div[@class='xwzx-ywzd-item-title']/a";
                web27.Next_URL_XPath = "//a[@class='i-pager-next']";
                web27.m_page_xpath = "//div[@id='dnn_ctr930_ModuleContent']|//div[@id='dnn_ContentPane']";
                web27.Save();
            }
        }

        public static void AddData()
        {

        }


        /// <summary>
        /// 这个内部类是用来做查询的时候用的。
        /// </summary>
        public new class Fields
        {
            private Fields() { }
            public static OperandProperty Name
            {
                get { return new OperandProperty("Name"); }
            }

            public static OperandParameter Oid
            {
                get { return new OperandParameter("Oid"); }
            }
        }

    }
}
