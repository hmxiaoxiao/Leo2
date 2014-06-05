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
        private string m_encoding;

        public string Title
        {
            get { return m_unread > 0 ? m_name + "(" + m_unread.ToString() + ")" : m_name; }
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
        [Indexed(Unique = true)]
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

        public string Encoding
        {
            get { return m_encoding; }
            set { SetPropertyValue<string>("Encoding", ref m_encoding, value); }
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
                using (UnitOfWork uow = new UnitOfWork())
                {

                    Web web001 = new Web();
                    web001.Parent_ID = 0;
                    web001.Name = "国资委";
                    web001.Is_Search = false;
                    web001.Save();

                    Web web001001 = new Web();
                    web001001.Parent_ID = web001.Oid;
                    web001001.Name = "国资要闻";
                    web001001.URL = "http://www.sasac.gov.cn/n1180/n20240/n20259/index.html";
                    web001001.Is_Search = true;
                    web001001.List_URL_XPath = "//td[@class='black14']/a";
                    web001001.Next_URL_XPath = "";
                    web001001.m_page_xpath = "/html[1]/body[1]/table[2]/tr[1]/td[2]/table[11]/tr[1]/td[1]";
                    web001001.Save();

                    Web web001006 = new Web();
                    web001006.Parent_ID = web001.Oid;
                    web001006.Name = "国有经济";
                    web001006.URL = "http://www.sasac.gov.cn/n1180/n1271/n20515/n2697206/gyjj.html";
                    web001006.Is_Search = true;
                    web001006.List_URL_XPath = "//td[@class='black14']/a";
                    web001006.Next_URL_XPath = "";
                    web001006.m_page_xpath = "/html[1]/body[1]/table[2]/tr[1]/td[2]/table[11]/tr[1]/td[1]";
                    web001006.Save();

                    Web web001007 = new Web();
                    web001007.Parent_ID = web001.Oid;
                    web001007.Name = "国资监管";
                    web001007.URL = "http://www.sasac.gov.cn/n1180/n1271/n20515/n2697175/gzjg.html";
                    web001007.Is_Search = true;
                    web001007.List_URL_XPath = "//td[@class='black14']/a";
                    web001007.Next_URL_XPath = "";
                    web001007.m_page_xpath = "/html[1]/body[1]/table[2]/tr[1]/td[2]/table[11]/tr[1]/td[1]";
                    web001007.Save();

                    Web web001003 = new Web();
                    web001003.Parent_ID = web001.Oid;
                    web001003.Name = "国资改革";
                    web001003.URL = "http://www.sasac.gov.cn/n1180/n1271/n20515/n2697190/gqgg.html";
                    web001003.Is_Search = true;
                    web001003.List_URL_XPath = "//td[@class='black14']/a";
                    web001003.Next_URL_XPath = "";
                    web001003.m_page_xpath = "/html[1]/body[1]/table[2]/tr[1]/td[2]/table[11]/tr[1]/td[1]";
                    web001003.Save();

                    Web web001004 = new Web();
                    web001004.Parent_ID = web001.Oid;
                    web001004.Name = "中央企业动态";
                    web001004.URL = "http://www.sasac.gov.cn/n1180/n1226/n2410/index.html";
                    web001004.Is_Search = true;
                    web001004.List_URL_XPath = "//td[@class='black14']/a";
                    web001004.Next_URL_XPath = "";
                    web001004.m_page_xpath = "/html[1]/body[1]/table[2]/tr[1]/td[2]/table[11]/tr[1]/td[1]";
                    web001004.Save();

                    Web web001005 = new Web();
                    web001005.Parent_ID = web001.Oid;
                    web001005.Name = "地方国资动态";
                    web001005.URL = "http://www.sasac.gov.cn/n1180/n1271/n1286/n3891/index.html";
                    web001005.Is_Search = true;
                    web001005.List_URL_XPath = "//td[@class='black14']/a";
                    web001005.Next_URL_XPath = "";
                    web001005.m_page_xpath = "/html[1]/body[1]/table[2]/tr[1]/td[2]/table[11]/tr[1]/td[1]";
                    web001005.Save();

                    Web web002 = new Web();
                    web002.Parent_ID = 0;
                    web002.Name = "中国核工业集团公司";
                    web002.Is_Search = false;
                    web002.Save();

                    Web web002001 = new Web();
                    web002001.Parent_ID = web002.Oid;
                    web002001.Name = "中核要闻";
                    web002001.URL = "http://www.cnnc.com.cn/tabid/293/Default.aspx";
                    web002001.Is_Search = true;
                    web002001.List_URL_XPath = "//td[@id='newslist']/a";
                    web002001.Next_URL_XPath = "";
                    web002001.m_page_xpath = "//div[@id='ess_ctr2251_ModuleContent']/table";
                    web002001.Save();

                    Web web002002 = new Web();
                    web002002.Parent_ID = web002.Oid;
                    web002002.Name = "集团快讯";
                    web002002.URL = "http://www.cnnc.com.cn/publish/portal0/tab664/module2138/more.htm";
                    web002002.Is_Search = true;
                    web002002.List_URL_XPath = "//td[@id='newslist']/a";
                    web002002.Next_URL_XPath = "";
                    web002002.m_page_xpath = "//div[@id='ess_ctr2251_ModuleContent']/table";
                    web002002.Save();

                    Web web002003 = new Web();
                    web002003.Parent_ID = web002.Oid;
                    web002003.Name = "一线动态";
                    web002003.URL = "http://www.cnnc.com.cn/publish/portal0/tab664/module2125/more.htm";
                    web002003.Is_Search = true;
                    web002003.List_URL_XPath = "//td[@id='newslist']/a";
                    web002003.Next_URL_XPath = "";
                    web002003.m_page_xpath = "//div[@id='ess_ctr2251_ModuleContent']/table";
                    web002003.Save();

                    Web web002004 = new Web();
                    web002004.Parent_ID = web002.Oid;
                    web002004.Name = "综合资讯";
                    web002004.URL = "http://www.cnnc.com.cn/publish/portal0/tab664/module2127/more.htm";
                    web002004.Is_Search = true;
                    web002004.List_URL_XPath = "//td[@id='newslist']/a";
                    web002004.Next_URL_XPath = "";
                    web002004.m_page_xpath = "//div[@id='ess_ctr2251_ModuleContent']/table";
                    web002004.Save();

                    Web web002005 = new Web();
                    web002005.Parent_ID = web002.Oid;
                    web002005.Name = "媒体聚焦";
                    web002005.URL = "http://www.cnnc.com.cn/publish/portal0/tab664/module2126/more.htm";
                    web002005.Is_Search = true;
                    web002005.List_URL_XPath = "//td[@id='newslist']/a";
                    web002005.Next_URL_XPath = "";
                    web002005.m_page_xpath = "//div[@id='ess_ctr2251_ModuleContent']/table";
                    web002005.Save();

                    Web web002006 = new Web();
                    web002006.Parent_ID = web002.Oid;
                    web002006.Name = "风采中核";
                    web002006.URL = "http://www.cnnc.com.cn/publish/portal0/tab664/module2128/more.htm";
                    web002006.Is_Search = true;
                    web002006.List_URL_XPath = "//td[@id='newslist']/a";
                    web002006.Next_URL_XPath = "";
                    web002006.m_page_xpath = "//div[@id='ess_ctr2251_ModuleContent']/table";
                    web002006.Save();

                    Web web003 = new Web();
                    web003.Parent_ID = 0;
                    web003.Name = "中国核工业建设集团公司";
                    web003.Is_Search = false;
                    web003.Save();

                    Web web003001 = new Web();
                    web003001.Parent_ID = web003.Oid;
                    web003001.Name = "重要新闻";
                    web003001.URL = "http://www.cnecc.com/g783/m877/mp1.aspx";
                    web003001.Is_Search = true;
                    web003001.List_URL_XPath = "//div[@class='xwzx-ywzd-item-title']/a";
                    web003001.Next_URL_XPath = "//a[@class='i-pager-next']";
                    web003001.m_page_xpath = "//div[@id='dnn_ctr930_ModuleContent']|//div[@id='dnn_ContentPane']";
                    web003001.Save();

                    Web web003002 = new Web();
                    web003002.Parent_ID = web003.Oid;
                    web003002.Name = "公司新闻";
                    web003002.URL = "http://www.cnecc.com/g293.aspx";
                    web003002.Is_Search = true;
                    web003002.List_URL_XPath = "//div[@class='xwzx-ywzd-item-title']/a";
                    web003002.Next_URL_XPath = "//a[@class='i-pager-next']";
                    web003002.m_page_xpath = "//div[@id='dnn_ctr930_ModuleContent']|//div[@id='dnn_ContentPane']";
                    web003002.Save();

                    Web web003003 = new Web();
                    web003003.Parent_ID = web003.Oid;
                    web003003.Name = "国资动态";
                    web003003.URL = "http://www.cnecc.com/g672/m1574.aspx";
                    web003003.Is_Search = true;
                    web003003.List_URL_XPath = "//div[@class='xwzx-ywzd-item-title']/a";
                    web003003.Next_URL_XPath = "//a[@class='i-pager-next']";
                    web003003.m_page_xpath = "//div[@id='dnn_ctr930_ModuleContent']|//div[@id='dnn_ContentPane']";
                    web003003.Save();

                    Web web003004 = new Web();
                    web003004.Parent_ID = web003.Oid;
                    web003004.Name = "关注与视野";
                    web003004.URL = "http://www.cnecc.com/g687/m1575.aspx";
                    web003004.Is_Search = true;
                    web003004.List_URL_XPath = "//div[@class='xwzx-ywzd-item-title']/a";
                    web003004.Next_URL_XPath = "//a[@class='i-pager-next']";
                    web003004.m_page_xpath = "//div[@id='dnn_ctr930_ModuleContent']|//div[@id='dnn_ContentPane']";
                    web003004.Save();

                    Web web003005 = new Web();
                    web003005.Parent_ID = web003.Oid;
                    web003005.Name = "工程动态";
                    web003005.URL = "http://www.cnecc.com/g295.aspx";
                    web003005.Is_Search = true;
                    web003005.List_URL_XPath = "//div[@class='xwzx-ywzd-item-title']/a";
                    web003005.Next_URL_XPath = "//a[@class='i-pager-next']";
                    web003005.m_page_xpath = "//div[@id='dnn_ctr930_ModuleContent']|//div[@id='dnn_ContentPane']";
                    web003005.Save();

                    Web web003006 = new Web();
                    web003006.Parent_ID = web003.Oid;
                    web003006.Name = "行业资讯";
                    web003006.URL = "http://www.cnecc.com/g299.aspx";
                    web003006.Is_Search = true;
                    web003006.List_URL_XPath = "//div[@class='xwzx-ywzd-item-title']/a";
                    web003006.Next_URL_XPath = "//a[@class='i-pager-next']";
                    web003006.m_page_xpath = "//div[@id='dnn_ctr930_ModuleContent']|//div[@id='dnn_ContentPane']";
                    web003006.Save();

                    Web web004 = new Web();
                    web004.Parent_ID = 0;
                    web004.Name = "中国航天科技集团公司";
                    web004.Is_Search = false;
                    web004.Save();

                    Web web004001 = new Web();
                    web004001.Parent_ID = web004.Oid;
                    web004001.Name = "集团要闻";
                    web004001.URL = "http://www.spacechina.com/n25/n144/n206/n214/index.html";
                    web004001.Is_Search = true;
                    web004001.List_URL_XPath = "//a[@class='hei_x_12_a']";
                    web004001.Next_URL_XPath = "customer1";
                    web004001.m_page_xpath = "//div[@id='mainart']/table/tr/td/table[2]";
                    web004001.Save();

                    Web web004002 = new Web();
                    web004002.Parent_ID = web004.Oid;
                    web004002.Name = "综合新闻";
                    web004002.URL = "http://www.spacechina.com/n25/n144/n206/n216/index.html";
                    web004002.Is_Search = true;
                    web004002.List_URL_XPath = "//a[@class='hei_x_12_a']";
                    web004002.Next_URL_XPath = "customer1";
                    web004002.m_page_xpath = "//div[@id='mainart']/table/tr/td/table[2]";
                    web004002.Save();

                    Web web004003 = new Web();
                    web004003.Parent_ID = web004.Oid;
                    web004003.Name = "产经信息";
                    web004003.URL = "http://www.spacechina.com/n25/n144/n206/n220/index.html";
                    web004003.Is_Search = true;
                    web004003.List_URL_XPath = "//a[@class='hei_x_12_a']";
                    web004003.Next_URL_XPath = "customer1";
                    web004003.m_page_xpath = "//div[@id='mainart']/table/tr/td/table[2]";
                    web004003.Save();

                    Web web004004 = new Web();
                    web004004.Parent_ID = web004.Oid;
                    web004004.Name = "通知公告";
                    web004004.URL = "http://www.spacechina.com/n25/n144/n206/n218/index.html";
                    web004004.Is_Search = true;
                    web004004.List_URL_XPath = "//a[@class='hei_x_12_a']";
                    web004004.Next_URL_XPath = "customer1";
                    web004004.m_page_xpath = "//div[@id='mainart']/table/tr/td/table[2]";
                    web004004.Save();

                    Web web004005 = new Web();
                    web004005.Parent_ID = web004.Oid;
                    web004005.Name = "媒体聚焦";
                    web004005.URL = "http://www.spacechina.com/n25/n144/n206/n224/index.html";
                    web004005.Is_Search = true;
                    web004005.List_URL_XPath = "//a[@class='hei_x_12_a']";
                    web004005.Next_URL_XPath = "customer1";
                    web004005.m_page_xpath = "//div[@id='mainart']/table/tr/td/table[2]";
                    web004005.Save();

                    Web web004006 = new Web();
                    web004006.Parent_ID = web004.Oid;
                    web004006.Name = "央企视野";
                    web004006.URL = "http://www.spacechina.com/n25/n144/n208/n130547/index.html";
                    web004006.Is_Search = true;
                    web004006.List_URL_XPath = "//a[@class='hei_x_12_a']";
                    web004006.Next_URL_XPath = "customer1";
                    web004006.m_page_xpath = "//div[@id='mainart']/table/tr/td/table[2]";
                    web004006.Save();

                    Web web004007 = new Web();
                    web004007.Parent_ID = web004.Oid;
                    web004007.Name = "军工信息";
                    web004007.URL = "http://www.spacechina.com/n25/n144/n208/n230/index.html";
                    web004007.Is_Search = true;
                    web004007.List_URL_XPath = "//a[@class='hei_x_12_a']";
                    web004007.Next_URL_XPath = "customer1";
                    web004007.m_page_xpath = "//div[@id='mainart']/table/tr/td/table[2]";
                    web004007.Save();

                    Web web005 = new Web();
                    web005.Parent_ID = 0;
                    web005.Name = "中国航天科工集团公司";
                    web005.Is_Search = false;
                    web005.Save();

                    Web web005001 = new Web();
                    web005001.Parent_ID = web005.Oid;
                    web005001.Name = "要闻导读";
                    web005001.URL = "http://www.casic.com.cn/n99188/n470321/index.html";
                    web005001.Is_Search = true;
                    web005001.List_URL_XPath = "//td[@class='black13_24']/a";
                    web005001.Next_URL_XPath = "customer1";
                    web005001.m_page_xpath = "/html/body/table[5]/tr/td";
                    web005001.Save();

                    Web web005002 = new Web();
                    web005002.Parent_ID = web005.Oid;
                    web005002.Name = "国资信息";
                    web005002.URL = "http://www.casic.com.cn/n103/n496141/index.html";
                    web005002.Is_Search = true;
                    web005002.List_URL_XPath = "//td[@class='black13_24']/a";
                    web005002.Next_URL_XPath = "customer1";
                    web005002.m_page_xpath = "/html/body/table[5]/tr/td";
                    web005002.Save();

                    Web web005003 = new Web();
                    web005003.Parent_ID = web005.Oid;
                    web005003.Name = "经营动态";
                    web005003.URL = "http://www.casic.com.cn/n103/n135/n1008321/index.html";
                    web005003.Is_Search = true;
                    web005003.List_URL_XPath = "//td[@class='black13_24']/a";
                    web005003.Next_URL_XPath = "customer1";
                    web005003.m_page_xpath = "/html/body/table[5]/tr/td";
                    web005003.Save();

                    Web web005004 = new Web();
                    web005004.Parent_ID = web005.Oid;
                    web005004.Name = "合作交流";
                    web005004.URL = "http://www.casic.com.cn/n103/n135/n1008323/index.html";
                    web005004.Is_Search = true;
                    web005004.List_URL_XPath = "//td[@class='black13_24']/a";
                    web005004.Next_URL_XPath = "customer1";
                    web005004.m_page_xpath = "/html/body/table[5]/tr/td";
                    web005004.Save();

                    Web web005005 = new Web();
                    web005005.Parent_ID = web005.Oid;
                    web005005.Name = "改革发展";
                    web005005.URL = "http://www.casic.com.cn/n103/n135/n1008325/index.html";
                    web005005.Is_Search = true;
                    web005005.List_URL_XPath = "//td[@class='black13_24']/a";
                    web005005.Next_URL_XPath = "customer1";
                    web005005.m_page_xpath = "/html/body/table[5]/tr/td";
                    web005005.Save();

                    Web web005006 = new Web();
                    web005006.Parent_ID = web005.Oid;
                    web005006.Name = "媒体聚焦";
                    web005006.URL = "http://www.casic.com.cn/n103/n139/index.html";
                    web005006.Is_Search = true;
                    web005006.List_URL_XPath = "//td[@class='black13_24']/a";
                    web005006.Next_URL_XPath = "customer1";
                    web005006.m_page_xpath = "/html/body/table[5]/tr/td";
                    web005006.Save();

                    Web web005007 = new Web();
                    web005007.Parent_ID = web005.Oid;
                    web005007.Name = "军工信息";
                    web005007.URL = "http://www.casic.com.cn/n103/n137/index.html";
                    web005007.Is_Search = true;
                    web005007.List_URL_XPath = "//td[@class='black13_24']/a";
                    web005007.Next_URL_XPath = "customer1";
                    web005007.m_page_xpath = "/html/body/table[5]/tr/td";
                    web005007.Save();

                    Web web006 = new Web();
                    web006.Parent_ID = 0;
                    web006.Name = "中国航空工业集团公司";
                    web006.Is_Search = false;
                    web006.Save();

                    Web web006001 = new Web();
                    web006001.Parent_ID = web006.Oid;
                    web006001.Name = "集团新闻";
                    web006001.URL = "http://www.avic.com.cn/cn/xwzx/jtxw/index.shtml";
                    web006001.Is_Search = true;
                    web006001.List_URL_XPath = "//ul[@class='lnews']/li/a";
                    web006001.Next_URL_XPath = "";
                    web006001.Encoding = "gb2312";
                    web006001.m_page_xpath = "//div[@class='leftevery']";
                    web006001.Save();

                    Web web006002 = new Web();
                    web006002.Parent_ID = web006.Oid;
                    web006002.Name = "国资委新闻";
                    web006002.URL = "http://www.avic.com.cn/cn/xwzx/gzwxw/index.shtml";
                    web006002.Is_Search = true;
                    web006002.List_URL_XPath = "//ul[@class='lnews']/li/a";
                    web006002.Next_URL_XPath = "";
                    web006002.Encoding = "gb2312";
                    web006002.m_page_xpath = "//div[@class='leftevery']";
                    web006002.Save();

                    Web web006003 = new Web();
                    web006003.Parent_ID = web006.Oid;
                    web006003.Name = "成员动态";
                    web006003.URL = "http://www.avic.com.cn/cn/xwzx/cydt/index.shtml";
                    web006003.Is_Search = true;
                    web006003.List_URL_XPath = "//ul[@class='lnews']/li/a";
                    web006003.Next_URL_XPath = "";
                    web006003.Encoding = "gb2312";
                    web006003.m_page_xpath = "//div[@class='leftevery']";
                    web006003.Save();

                    Web web006004 = new Web();
                    web006004.Parent_ID = web006.Oid;
                    web006004.Name = "行业动态";
                    web006004.URL = "http://www.avic.com.cn/cn/xwzx/xydt/index.shtml";
                    web006004.Is_Search = true;
                    web006004.List_URL_XPath = "//ul[@class='lnews']/li/a";
                    web006004.Next_URL_XPath = "";
                    web006004.Encoding = "gb2312";
                    web006004.m_page_xpath = "//div[@class='leftevery']";
                    web006004.Save();

                    Web web006005 = new Web();
                    web006005.Parent_ID = web006.Oid;
                    web006005.Name = "图片新闻";
                    web006005.URL = "http://www.avic.com.cn/cn/xwzx/tpxw/index.shtml";
                    web006005.Is_Search = true;
                    web006005.List_URL_XPath = "//ul[@class='lnews']/li/a";
                    web006005.Next_URL_XPath = "";
                    web006005.Encoding = "gb2312";
                    web006005.m_page_xpath = "//div[@class='leftevery']";
                    web006005.Save();

                    Web web007 = new Web();
                    web007.Parent_ID = 0;
                    web007.Name = "中国船舶重工集团公司";
                    web007.Is_Search = false;
                    web007.Save();

                    Web web007001 = new Web();
                    web007001.Parent_ID = web007.Oid;
                    web007001.Name = "集团新闻";
                    web007001.URL = "http://www.csic.com.cn/zgxwzx/csic_jtxw/";
                    web007001.Is_Search = true;
                    web007001.List_URL_XPath = "/html/body/table[9]/tr/td[2]//a[@target='_blank']";
                    web007001.Next_URL_XPath = "/html/body/table[9]/tr/td[2]/table[20]/tr/td[4]/a";
                    web007001.Encoding = "gb2312";
                    web007001.m_page_xpath = "/html/body/table[9]/tr/td[2]";
                    web007001.Save();

                    Web web007002 = new Web();
                    web007002.Parent_ID = web007.Oid;
                    web007002.Name = "图片新闻";
                    web007002.URL = "http://www.csic.com.cn/zgxwzx/csic_tpxw/";
                    web007002.Is_Search = true;
                    web007002.List_URL_XPath = "/html/body/table[9]/tr/td[2]//a[@target='_blank']";
                    web007002.Next_URL_XPath = "/html/body/table[9]/tr/td[2]/table[20]/tr/td[4]/a";
                    web007002.Encoding = "gb2312";
                    web007002.m_page_xpath = "/html/body/table[9]/tr/td[2]";
                    web007002.Save();

                    Web web007003 = new Web();
                    web007003.Parent_ID = web007.Oid;
                    web007003.Name = "国资要闻";
                    web007003.URL = "http://www.csic.com.cn/zgxwzx/gzdt/";
                    web007003.Is_Search = true;
                    web007003.List_URL_XPath = "/html/body/table[9]/tr/td[2]//a[@target='_blank']";
                    web007003.Next_URL_XPath = "/html/body/table[9]/tr/td[2]/table[20]/tr/td[4]/a";
                    web007003.Encoding = "gb2312";
                    web007003.m_page_xpath = "/html/body/table[9]/tr/td[2]";
                    web007003.Save();

                    Web web007004 = new Web();
                    web007004.Parent_ID = web007.Oid;
                    web007004.Name = "成员动态";
                    web007004.URL = "http://www.csic.com.cn/zgxwzx/zgcydt/";
                    web007004.Is_Search = true;
                    web007004.List_URL_XPath = "/html/body/table[9]/tr/td[2]//a[@target='_blank']";
                    web007004.Next_URL_XPath = "/html/body/table[9]/tr/td[2]/table[20]/tr/td[4]/a";
                    web007004.Encoding = "gb2312";
                    web007004.m_page_xpath = "/html/body/table[9]/tr/td[2]";
                    web007004.Save();

                    Web web007005 = new Web();
                    web007005.Parent_ID = web007.Oid;
                    web007005.Name = "船舶市场";
                    web007005.URL = "http://www.csic.com.cn/zgxwzx/csic_cshq/";
                    web007005.Is_Search = true;
                    web007005.List_URL_XPath = "/html/body/table[9]/tr/td[2]//a[@target='_blank']";
                    web007005.Next_URL_XPath = "/html/body/table[9]/tr/td[2]/table[20]/tr/td[4]/a";
                    web007005.Encoding = "gb2312";
                    web007005.m_page_xpath = "/html/body/table[9]/tr/td[2]";
                    web007005.Save();

                    Web web007006 = new Web();
                    web007006.Parent_ID = web007.Oid;
                    web007006.Name = "物资配套";
                    web007006.URL = "http://www.csic.com.cn/zgxwzx/csic_gcsc/";
                    web007006.Is_Search = true;
                    web007006.List_URL_XPath = "/html/body/table[9]/tr/td[2]//a[@target='_blank']";
                    web007006.Next_URL_XPath = "/html/body/table[9]/tr/td[2]/table[20]/tr/td[4]/a";
                    web007006.Encoding = "gb2312";
                    web007006.m_page_xpath = "/html/body/table[9]/tr/td[2]";
                    web007006.Save();

                    Web web007007 = new Web();
                    web007007.Parent_ID = web007.Oid;
                    web007007.Name = "通知公告";
                    web007007.URL = "http://www.csic.com.cn/zgxwzx/jttzgg/";
                    web007007.Is_Search = true;
                    web007007.List_URL_XPath = "/html/body/table[9]/tr/td[2]//a[@target='_blank']";
                    web007007.Next_URL_XPath = "/html/body/table[9]/tr/td[2]/table[20]/tr/td[4]/a";
                    web007007.Encoding = "gb2312";
                    web007007.m_page_xpath = "/html/body/table[9]/tr/td[2]";
                    web007007.Save();


                    Web web109 = new Web();
                    web109.Parent_ID = 0;
                    web109.Name = "华侨城集团公司";
                    web109.Is_Search = false;
                    web109.Save();

                    Web web109001 = new Web();
                    web109001.Parent_ID = web109.Oid;
                    web109001.Name = "新闻中心";
                    web109001.URL = "http://www.chinaoct.com/category.aspx?NodeID=29";
                    web109001.Is_Search = true;
                    web109001.List_URL_XPath = ".//*[@class='view_list']//a";
                    web109001.Next_URL_XPath = "";
                    web109001.Encoding = "";
                    web109001.m_page_xpath = ".//*[@class='view_show clearall']";
                    web109001.Save();

                    Web web109002 = new Web();
                    web109002.Parent_ID = web109.Oid;
                    web109002.Name = "电子公告";
                    web109002.URL = "http://www.chinaoct.com/category.aspx?NodeID=30";
                    web109002.Is_Search = true;
                    web109002.List_URL_XPath = ".//*[@class='view_list']//a";
                    web109002.Next_URL_XPath = "";
                    web109002.Encoding = "";
                    web109002.m_page_xpath = ".//*[@class='view_show clearall']";
                    web109002.Save();

                    Web web109003 = new Web();
                    web109003.Parent_ID = web109.Oid;
                    web109003.Name = "专题专栏";
                    web109003.URL = "http://www.chinaoct.com/category.aspx?NodeID=31";
                    web109003.Is_Search = true;
                    web109003.List_URL_XPath = ".//*[@class='view_list']//a";
                    web109003.Next_URL_XPath = "";
                    web109003.Encoding = "";
                    web109003.m_page_xpath = ".//*[@class='view_show clearall']";
                    web109003.Save();

                    Web web109004 = new Web();
                    web109004.Parent_ID = web109.Oid;
                    web109004.Name = "国资委要闻";
                    web109004.URL = "http://www.chinaoct.com/category.aspx?NodeID=254";
                    web109004.Is_Search = true;
                    web109004.List_URL_XPath = ".//*[@class='view_list']//a";
                    web109004.Next_URL_XPath = "";
                    web109004.Encoding = "";
                    web109004.m_page_xpath = ".//*[@class='view_show clearall']";
                    web109004.Save();

                    Web web109005 = new Web();
                    web109005.Parent_ID = web109.Oid;
                    web109005.Name = "媒体聚焦国企";
                    web109005.URL = "http://www.chinaoct.com/category.aspx?NodeID=256";
                    web109005.Is_Search = true;
                    web109005.List_URL_XPath = ".//*[@class='view_list']//a";
                    web109005.Next_URL_XPath = "";
                    web109005.Encoding = "";
                    web109005.m_page_xpath = ".//*[@class='view_show clearall']";
                    web109005.Save();

                    Web web109006 = new Web();
                    web109006.Parent_ID = web109.Oid;
                    web109006.Name = "华侨城改革发展";
                    web109006.URL = "http://www.chinaoct.com/category.aspx?NodeID=257";
                    web109006.Is_Search = true;
                    web109006.List_URL_XPath = ".//*[@class='view_list']//a";
                    web109006.Next_URL_XPath = "";
                    web109006.Encoding = "";
                    web109006.m_page_xpath = ".//*[@class='view_show clearall']";
                    web109006.Save();



                    Web web110 = new Web();
                    web110.Parent_ID = 0;
                    web110.Name = "南光（集团）有限公司";
                    web110.Is_Search = false;
                    web110.Save();

                    Web web110001 = new Web();
                    web110001.Parent_ID = web110.Oid;
                    web110001.Name = "领导活动";
                    web110001.URL = "http://www.namkwong.com.mo/enews/leader/";
                    web110001.Is_Search = true;
                    web110001.List_URL_XPath = ".//*[@class='list_news']//a";
                    web110001.Next_URL_XPath = ".//*[@title='(next)下一页']";
                    web110001.Encoding = "";
                    web110001.m_page_xpath = ".//*[@class='news_content w_news_content']";
                    web110001.Save();

                    Web web110002 = new Web();
                    web110002.Parent_ID = web110.Oid;
                    web110002.Name = "集团动态";
                    web110002.URL = "http://www.namkwong.com.mo/enews/group/";
                    web110002.Is_Search = true;
                    web110002.List_URL_XPath = ".//*[@class='list_news']//a";
                    web110002.Next_URL_XPath = ".//*[@title='(next)下一页']";
                    web110002.Encoding = "";
                    web110002.m_page_xpath = ".//*[@class='news_content w_news_content']";
                    web110002.Save();

                    Web web110003 = new Web();
                    web110003.Parent_ID = web110.Oid;
                    web110003.Name = "下属机构动态";
                    web110003.URL = "http://www.namkwong.com.mo/enews/subsidiaries/";
                    web110003.Is_Search = true;
                    web110003.List_URL_XPath = ".//*[@class='list_news']//a";
                    web110003.Next_URL_XPath = ".//*[@title='(next)下一页']";
                    web110003.Encoding = "";
                    web110003.m_page_xpath = ".//*[@class='news_content w_news_content']";
                    web110003.Save();

                    Web web110004 = new Web();
                    web110004.Parent_ID = web110.Oid;
                    web110004.Name = "安全生产";
                    web110004.URL = "http://www.namkwong.com.mo/enews/safty/";
                    web110004.Is_Search = true;
                    web110004.List_URL_XPath = ".//*[@class='list_news']//a";
                    web110004.Next_URL_XPath = ".//*[@title='(next)下一页']";
                    web110004.Encoding = "";
                    web110004.m_page_xpath = ".//*[@class='news_content w_news_content']";
                    web110004.Save();

                    Web web110005 = new Web();
                    web110005.Parent_ID = web110.Oid;
                    web110005.Name = "员工文娱活动";
                    web110005.URL = "http://www.namkwong.com.mo/enews/Culturalactivities/";
                    web110005.Is_Search = true;
                    web110005.List_URL_XPath = ".//*[@class='list_news']//a";
                    web110005.Next_URL_XPath = ".//*[@title='(next)下一页']";
                    web110005.Encoding = "";
                    web110005.m_page_xpath = ".//*[@class='news_content w_news_content']";
                    web110005.Save();

                    Web web110006 = new Web();
                    web110006.Parent_ID = web110.Oid;
                    web110006.Name = "履行社会责任";
                    web110006.URL = "http://www.namkwong.com.mo/csr/csr/";
                    web110006.Is_Search = true;
                    web110006.List_URL_XPath = ".//*[@class='list_news']//a";
                    web110006.Next_URL_XPath = ".//*[@title='(next)下一页']";
                    web110006.Encoding = "";
                    web110006.m_page_xpath = ".//*[@class='news_content w_news_content']";
                    web110006.Save();

                    Web web110007 = new Web();
                    web110007.Parent_ID = web110.Oid;
                    web110007.Name = "员工培训";
                    web110007.URL = "http://www.namkwong.com.mo/HumanResource/Staff_training/";
                    web110007.Is_Search = true;
                    web110007.List_URL_XPath = ".//*[@class='list_news']//a";
                    web110007.Next_URL_XPath = ".//*[@title='(next)下一页']";
                    web110007.Encoding = "";
                    web110007.m_page_xpath = ".//*[@class='news_content w_news_content']";
                    web110007.Save();

                    Web web111 = new Web();
                    web111.Parent_ID = 0;
                    web111.Name = "中国西电集团公司";
                    web111.Is_Search = false;
                    web111.Save();

                    Web web111001 = new Web();
                    web111001.Parent_ID = web111.Oid;
                    web111001.Name = "国资要闻";
                    web111001.URL = "http://www.xd.com.cn/structure/zxyd/gzyw.htm";
                    web111001.Is_Search = true;
                    web111001.List_URL_XPath = "//a[@class='CicroUM235B_0119_8943_xd_25_news_alistrow1']|//a[@class='CicroUM235B_0119_8943_xd_25_news_alistrow2']";
                    web111001.Next_URL_XPath = "customer:www.xd.com.cn";
                    web111001.Encoding = "";
                    web111001.m_page_xpath = "//*[@id='组件外围表格']/tr/td/table[@class='maintable']";
                    web111001.Save();

                    Web web111002 = new Web();
                    web111002.Parent_ID = web111.Oid;
                    web111002.Name = "关注与视野";
                    web111002.URL = "http://www.xd.com.cn/structure/zxyd/gzysy.htm";
                    web111002.Is_Search = true;
                    web111002.List_URL_XPath = "//a[@class='CicroUKOFRI_0836_8205_xd_25_news_listrow1']|//a[@class='CicroUKOFRI_0836_8205_xd_25_news_listrow2']";
                    web111002.Next_URL_XPath = "customer:www.xd.com.cn";
                    web111002.Encoding = "";
                    web111002.m_page_xpath = "//*[@id='组件外围表格']/tr/td/table[@class='maintable']";
                    web111002.Save();

                    Web web111003 = new Web();
                    web111003.Parent_ID = web111.Oid;
                    web111003.Name = "集团动态";
                    web111003.URL = "http://www.xd.com.cn/structure/zxyd/jtdt.htm";
                    web111003.Is_Search = true;
                    web111003.List_URL_XPath = "//a[@class='CicroO35T23_4871_7541_xd_25_news_listrow1']|//a[@class='CicroO35T23_4871_7541_xd_25_news_listrow2']";
                    web111003.Next_URL_XPath = "customer:www.xd.com.cn";
                    web111003.Encoding = "";
                    web111003.m_page_xpath = "//*[@id='组件外围表格']/tr/td/table[@class='maintable']";
                    web111003.Save();

                    Web web111004 = new Web();
                    web111003.Parent_ID = web111.Oid;
                    web111003.Name = "领导动态";
                    web111003.URL = "http://www.xd.com.cn/structure/zxyd/lddt.htm";
                    web111003.Is_Search = true;
                    web111003.List_URL_XPath = "//a[@class='CicroZ2N5P1_9729_0264_xd_25_news_alistrow1']|//a[@class='CicroZ2N5P1_9729_0264_xd_25_news_alistrow2']";
                    web111003.Next_URL_XPath = "customer:www.xd.com.cn";
                    web111003.Encoding = "";
                    web111003.m_page_xpath = "//*[@id='组件外围表格']/tr/td/table[@class='maintable']";
                    web111003.Save();

                    Web web111005 = new Web();
                    web111003.Parent_ID = web111.Oid;
                    web111003.Name = "子公司动态";
                    web111003.URL = "http://www.xd.com.cn/structure/zxyd/zgsdt.htm";
                    web111003.Is_Search = true;
                    web111003.List_URL_XPath = "//a[@class='CicroTX5RI0_4543_4477_xd_25_news_alistrow1']|//a[@class='CicroTX5RI0_4543_4477_xd_25_news_alistrow1']";
                    web111003.Next_URL_XPath = "customer:www.xd.com.cn";
                    web111003.Encoding = "";
                    web111003.m_page_xpath = "//*[@id='组件外围表格']/tr/td/table[@class='maintable']";
                    web111003.Save();


                    Web web112 = new Web();
                    web112.Parent_ID = 0;
                    web112.Name = "中国铁路物资总公司";
                    web112.Is_Search = false;
                    web112.Save();

                    Web web112001 = new Web();
                    web112001.Parent_ID = web112.Oid;
                    web112001.Name = "公司动态";
                    web112001.Is_Search = false;
                    web112001.Save();

                    Web web11200101 = new Web();
                    web11200101.Parent_ID = web112001.Oid;
                    web11200101.Name = "集团公司动态";
                    web11200101.URL = "http://www.crmg.com.cn/p398.aspx";
                    web11200101.Is_Search = true;
                    web11200101.List_URL_XPath = ".//*[@class='Crmg-second-news-item-title']/a";
                    web11200101.Next_URL_XPath = ".//*[@class='i-pager-next']";
                    web11200101.Encoding = "";
                    web11200101.m_page_xpath = ".//*[@id='zhd_ctr925_ModuleContent']/div/table";
                    web11200101.Save();

                    Web web11200102 = new Web();
                    web11200102.Parent_ID = web112001.Oid;
                    web11200102.Name = "子公司动态";
                    web11200102.URL = "http://www.crmg.com.cn/p397.aspx";
                    web11200102.Is_Search = true;
                    web11200102.List_URL_XPath = ".//*[@class='Crmg-second-news-item-title']/a";
                    web11200102.Next_URL_XPath = ".//*[@class='i-pager-next']";
                    web11200102.Encoding = "";
                    web11200102.m_page_xpath = ".//*[@id='zhd_ctr925_ModuleContent']/div/table";
                    web11200102.Save();

                    Web web112002 = new Web();
                    web112002.Parent_ID = web112.Oid;
                    web112002.Name = "公司告示";
                    web112002.URL = "http://www.crmg.com.cn/p369.aspx";
                    web112002.Is_Search = true;
                    web112002.List_URL_XPath = ".//*[@class='Crmg-second-news-item-title']/a";
                    web112002.Next_URL_XPath = ".//*[@class='i-pager-next']";
                    web112002.Encoding = "";
                    web112002.m_page_xpath = ".//*[@id='zhd_ctr925_ModuleContent']/div/table";
                    web112002.Save();

                    Web web112003 = new Web();
                    web112003.Parent_ID = web112.Oid;
                    web112003.Name = "市场资讯";
                    web112003.URL = "http://www.crmg.com.cn/p372.aspx";
                    web112003.Is_Search = true;
                    web112003.List_URL_XPath = ".//*[@class='Crmg-second-news-item-title']/a";
                    web112003.Next_URL_XPath = ".//*[@class='i-pager-next']";
                    web112003.Encoding = "";
                    web112003.m_page_xpath = ".//*[@id='zhd_ctr925_ModuleContent']/div/table";
                    web112003.Save();

                    Web web112004 = new Web();
                    web112004.Parent_ID = web112.Oid;
                    web112004.Name = "行业动态";
                    web112004.URL = "http://www.crmg.com.cn/p371.aspx";
                    web112004.Is_Search = true;
                    web112004.List_URL_XPath = ".//*[@class='Crmg-second-news-item-title']/a";
                    web112004.Next_URL_XPath = ".//*[@class='i-pager-next']";
                    web112004.Encoding = "";
                    web112004.m_page_xpath = ".//*[@id='zhd_ctr925_ModuleContent']/div/table";
                    web112004.Save();

                    Web web113 = new Web();
                    web113.Parent_ID = 0;
                    web113.Name = "中国国新控股有限责任公司";
                    web113.Is_Search = false;
                    web113.Save();

                    Web web113001 = new Web();
                    web113001.Parent_ID = web113.Oid;
                    web113001.Name = "公司动态";
                    web113001.URL = "http://www.crhc.cn/n12751492/n13812522/n13812591/index.html";
                    web113001.Is_Search = true;
                    web113001.List_URL_XPath = "html/body/table[3]/tr/td[2]/table[3]/tr/td/table/tr[1]/td/table/tr/td/a";
                    web113001.Next_URL_XPath = "";
                    web113001.Encoding = "";
                    web113001.m_page_xpath = "html/body/table[3]/tr/td/table[2]/tr/td";
                    web113001.Save();

                    Web web113002 = new Web();
                    web113002.Parent_ID = web113.Oid;
                    web113002.Name = "企业告示";
                    web113002.URL = "http://www.crhc.cn/n12751492/n13812522/n13812626/index.html";
                    web113002.Is_Search = true;
                    web113002.List_URL_XPath = "html/body/table[3]/tr/td[2]/table[3]/tr/td/table/tr[1]/td/table/tr/td/a";
                    web113002.Next_URL_XPath = "";
                    web113002.Encoding = "";
                    web113002.m_page_xpath = "html/body/table[3]/tr/td/table[2]/tr/td";
                    web113002.Save();

                    Web web11300301 = new Web();
                    web11300301.Parent_ID = web113.Oid;
                    web11300301.Name = "中国华星集团公司";
                    web11300301.URL = "http://www.crhc.cn/n12751492/n13812522/n13812645/n13814550/index.html";
                    web11300301.Is_Search = true;
                    web11300301.List_URL_XPath = "html/body/table[3]/tr/td[2]/table[3]/tr/td/table[2]/tr/td/a";
                    web11300301.Next_URL_XPath = "";
                    web11300301.Encoding = "";
                    web11300301.m_page_xpath = "html/body/table[3]/tr/td/table[2]/tr/td";
                    web11300301.Save();

                    Web web11300302 = new Web();
                    web11300302.Parent_ID = web113.Oid;
                    web11300302.Name = "国星有限责任公司";
                    web11300302.URL = "http://www.crhc.cn/n12751492/n13812522/n13812645/n14343081/index.html";
                    web11300302.Is_Search = true;
                    web11300302.List_URL_XPath = "html/body/table[3]/tr/td[2]/table[3]/tr/td/table[2]/tr/td/a";
                    web11300302.Next_URL_XPath = "";
                    web11300302.Encoding = "";
                    web11300302.m_page_xpath = "html/body/table[3]/tr/td/table[2]/tr/td";
                    web11300302.Save();

                    Web web11300303 = new Web();
                    web11300303.Parent_ID = web113.Oid;
                    web11300303.Name = "中国文化产业发展集团公司";
                    web11300303.URL = "http://www.crhc.cn/n12751492/n13812522/n13812645/n14754595/index.html";
                    web11300303.Is_Search = true;
                    web11300303.List_URL_XPath = "html/body/table[3]/tr/td[2]/table[3]/tr/td/table[2]/tr/td/a";
                    web11300303.Next_URL_XPath = "";
                    web11300303.Encoding = "";
                    web11300303.m_page_xpath = "html/body/table[3]/tr/td/table[2]/tr/td";
                    web11300303.Save();

                    uow.CommitChanges();        //  保存所有的数据
                }

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
