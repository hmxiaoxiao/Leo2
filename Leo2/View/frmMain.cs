using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using Leo2.Controller;
using DevExpress.Xpo;
using Leo2.Model;
using DevExpress.XtraTreeList.Nodes;
using Leo2.Helper;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraEditors.Controls;
using System.Diagnostics;
using DevExpress.Data.Filtering;
using System.Threading;
using Leo2.Rule;

namespace Leo2.View
{
    public partial class frmMain : DevExpress.XtraEditors.XtraForm
    {
        private LeoController m_controller;     // 当前的控制器
        private int m_current_web_oid = -1;         // 当前列表显示的ID值
        private XPCollection<Web> m_webs;
        private XPCollection<Page> m_pages;
        private Task m_task = null;            // Web扫描的线程

        // 构造函数，初始化必要的数据
        public frmMain(LeoController controller)
        {
            InitializeComponent();
            m_controller = controller;
            m_webs = m_controller.GetAllWebs();
        }

        // 载入时，初始化界面
        private void frmMain_Load(object sender, EventArgs e)
        {
            InitTree();
            beiStatus.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            beiProcess.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }

        // 对树进行初始化，树现在只分二级，一级网站名称，第二级为该网站下的栏目
        private void InitTree()
        {
            // 初始化树
            XPCollection<Web> webs = m_controller.GetAllWebs();
            treeList1.KeyFieldName = "Oid";
            treeList1.ParentFieldName = "Parent_ID";
            treeList1.DataSource = webs;
            //treeList1.ExpandAll();
        }


        // 树点击时，刷新该结点的内容
        private void treeList1_Click(object sender, EventArgs e)
        {
            ShowPageList();
        }

        /// <summary>
        /// 列表点击时，设置为已读
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_Click(object sender, EventArgs e)
        {
            ShowPageContent();
        }




        // 右键菜单的处理
        private void treeList1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                popupMenu1.ShowPopup(e.Location);
            }
        }

        // 下载所有的内容 
        private void btnDownload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckTask()) return;

            DownloadPageContent();
        }


        // 更新树的已读数量
        private void ChangeWebUnReadCount()
        {
            // 简单起见，当前的结点直接减1
            int web_oid = int.Parse(treeList1.FocusedNode[treeOid].ToString());
            foreach (Web web in m_webs)
            {
                if (web.Oid == web_oid)
                {
                    web.Unread -= 1;
                }
            }
            treeList1.RefreshDataSource();
        }
        
        private static XPCollection<Web> m_batch_webs = null;
        private static int m_location = 0;

        // 全部更新
        private void btnUpdateAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // 如果正在扫描，就不扫描了。
            if (!CheckTask())
                return;

            this.Cursor = Cursors.WaitCursor;   // 设置忙光标

            m_batch_webs = new XPCollection<Web>(CriteriaOperator.Parse("Is_Search = ?", true));
            Task taskA = Task.Factory.StartNew(() => BatchStartSingleScan());

            this.Cursor = Cursors.Default;      // 光标恢复正常
        }

        private Web GetNextWeb()
        {
            if (m_location < m_batch_webs.Count())
                return m_batch_webs[m_location++];
            else
                return null;
        }

        private void BatchStartSingleScan()
        {
            Web web = GetNextWeb();
            if (web != null)
                m_task = Task.Factory.StartNew(() => StartBatchScan(web.Rule));
            else
            {
                MessageBox.Show("全部更新完成!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        // 开始扫描列表页中的所有的内容
        private void StartBatchScan(BaseRule rule)
        {
            rule.SiteScanBegin += ShowBeginScan;
            rule.PageScanComplete += ShowProcess;
            rule.SiteScanComplete += ShowBatchCompleteInfo;
            rule.SingleSiteScan();
        }


        // 扫描完成后的显示处理
        private void ShowBatchCompleteInfo(object sender, BaseRule.SiteScanCompleteEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new BaseRule.SiteScanCompleteEventHandle(ShowBatchCompleteInfo), new object[] { null, e });
            }
            else
            {
                beiProcess.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;    // 不显示进度条
                beiStatus.Caption = string.Format(@"[{0}]扫描完成！", e.web.Name);
                //ShowPageList(true);         // 刷新一下结点
                m_task = null;        // 清空线程 
                Task.Factory.StartNew(() => BatchStartSingleScan());    // 扫描下一个
            }
        }


        // 单Web的更新
        private void btnUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BeginWebDownload();
        }

        private bool CheckTask()
        {
            if (m_task != null)
            {
                MessageBox.Show("当前正在扫描，请等扫描完后，再进行类似的操作!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }



        // 开始扫描页面
        private void BeginWebDownload()
        {
            // 如果正在扫描，就不扫描了。
            if (!CheckTask()) return;

            this.Cursor = Cursors.WaitCursor;   // 设置忙光标

            // 取得对应的Web
            int web_id = int.Parse(treeList1.FocusedNode[treeOid].ToString());
            XPCollection<Web> webs = new XPCollection<Web>(XpoDefault.Session,
                CriteriaOperator.Parse("Oid = ?", web_id));
            Debug.Assert(webs.Count() == 1);
            Web current_web = webs[0];

            // 取得对应的规则
            BaseRule br = current_web.Rule;

            // 如果规则没有取到，就提示尚未完成，不再继续
            if (br == null)
            {
                MessageBox.Show("该扫描规则还没有完成，暂不能使用!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                //开始一个线程进行工作
                //m_web_thread = new Thread(new ParameterizedThreadStart(StartSingleScan)) { IsBackground = true };
                //m_web_thread.Start(m_rule);
                m_task = Task.Factory.StartNew(() => StartSingleScan(br));
            }

            this.Cursor = Cursors.Default;      // 光标恢复正常
        }

        // 开始扫描列表页中内容
        private void StartSingleScan(BaseRule rule)
        {
            rule.SiteScanBegin += ShowBeginScan;
            rule.PageScanComplete += ShowProcess;
            rule.SiteScanComplete += ShowCompleteInfo;
            rule.SingleSiteScan();
        }

        // 开始扫描时，界面的显示
        private void ShowBeginScan(object sender, BaseRule.SiteScanBeginEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new BaseRule.SiteScanBeginHandler(ShowBeginScan), new object[] { null, e });
            }
            else
            {
                // 将状态栏的文本提示和进度条显示出来
                beiStatus.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                beiProcess.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                // 设置进度条的最大值 
                this.repositoryItemProgressBar1.Maximum = e.web.MaxPage;

                // 初始化显示内容
                beiStatus.Caption = string.Format(@"[{0}]开始准备扫描...", e.web.Name);
                beiProcess.EditValue = 0;
            }
        }



        // 显示扫描的进度
        private void ShowProcess(object sender, BaseRule.PageScanCompleteEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new BaseRule.PageScanCompleteEventHandler(ShowProcess), new object[] { null, e });
            }
            else
            {
                int process = int.Parse(beiProcess.EditValue.ToString());
                beiProcess.EditValue = (++process);
                beiStatus.Caption = string.Format(@"[{2}]正在扫描...,一共{0}页, {1}页已扫描.", e.web.MaxPage, process, e.web.Name);
            }
        }

        // 扫描完成后的显示处理
        private void ShowCompleteInfo(object sender, BaseRule.SiteScanCompleteEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new BaseRule.SiteScanCompleteEventHandle(ShowCompleteInfo), new object[] { null, e });
            }
            else
            {
                beiProcess.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;    // 不显示进度条
                beiStatus.Caption = string.Format(@"[{0}]扫描完成！", e.web.Name);
                ShowPageList(true);         // 刷新一下结点
                m_task = null;        // 清空线程 
            }
        }

        //private void UpdateAll(TreeListNode node, bool update_all = false)
        //{
        //    foreach (TreeListNode child in node.Nodes)
        //    {
        //        if ((bool)child[treeIsSearch])
        //        {
        //            m_controller.DownloadPageFromURL(int.Parse(child[treeOid].ToString()), update_all);
        //        }

        //        // 如果有子结点，就继续遍历
        //        if (child.Nodes.Count > 0)
        //            UpdateAll(child);
        //    }
        //}

        // 下载page,只要没有下载的PAGE，会全部下载
        CancellationTokenSource cts = null;
        private void DownloadPageContent()
        {
            // 显示下载信息
            beiStatus.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            if(cts != null)
                cts.Cancel();        // 取消过去的任务

            cts = new CancellationTokenSource();
            m_controller.PageDownloadComplete += ShowPageDownloadComplete;
            m_controller.AllPageDownloadComplete += AllPageDownComplete;
            Task<bool> t = new Task<bool>(() => m_controller.DownloadPageContent(cts.Token), cts.Token);
            t.Start();
        }

        private void ShowPageDownloadComplete(object sender, LeoController.PageDownloadCompleteEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new LeoController.PageDownloadCompleteHandler(ShowPageDownloadComplete), new object[] { null, e });
            }
            else
            {
                beiStatus.Caption = string.Format(@"[{0}-{1}]下载完成！", e.page.ParentWeb.Name, e.page.Title);
            }
        }

        private void AllPageDownComplete(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new LeoController.AllPageDownloadCompleteHandler(AllPageDownComplete), new object[] { null, e });
            }
            else
            {
                beiStatus.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }


        /// <summary>
        /// 显示树结点对应的所有的网页列表
        /// </summary>
        private void ShowPageList(bool force_refresh = false)
        {
            // 判断当前结点是否为二级结点，是则刷新内容
            if (treeList1.FocusedNode != null && (bool)treeList1.FocusedNode[treeIsSearch] == true)
            {
                int oid = int.Parse(treeList1.FocusedNode[treeOid].ToString());     // 取得当前的ID
                if (oid != m_current_web_oid || gridView1.RowCount <= 0 || force_refresh)            // 如果ID已经更改了就刷新
                {
                    m_pages = m_controller.GetSubPages(oid);
                    gridControl1.DataSource = m_pages;
                    m_current_web_oid = oid;
                }
            }
            ShowPageContent();
        }


        /// <summary>
        /// 显示网页的内容
        /// </summary>
        private void ShowPageContent()
        {
            if (gridView1.FocusedRowHandle < 0)     // 如果表格中没有数据，就直接返回
                return;

            int oid = int.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridOid).ToString());
            if (!(bool)gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridIsRead))       // 如果当前没有读取过的话
            {
                m_controller.SetPageHasRead(oid);       // 设置网页已读
                ChangeWebUnReadCount();                 // 更新数量
            }

            Page page = m_pages.FirstOrDefault(p => p.Oid == oid);
            if (page != null)
            {
                page.Is_Read = true;
                webBrowser1.DocumentText = page.DisplayContent();
            }
            gridView1.RefreshData();
        }



    }
}