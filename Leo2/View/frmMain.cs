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

namespace Leo2.View
{
    public partial class frmMain : DevExpress.XtraEditors.XtraForm
    {
        private LeoController m_controller;     // 当前的控制器
        private int m_current_oid = -1;         // 当前列表显示的ID值
        private XPCollection<Web> m_webs;
        private XPCollection<Page> m_pages;


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
        }

        private void InitTree()
        {
            // 初始化树
            XPCollection<Web> webs = m_controller.GetAllWebs();
            treeList1.KeyFieldName = "Oid";
            treeList1.ParentFieldName = "Parent_ID";
            treeList1.DataSource = webs;
            treeList1.ExpandAll();
        }

        private void btnUpdateAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            TreeListNode node = treeList1.Nodes[0]; // 先取得根结点
            UpdateAll(node);
            MessageBox.Show("分析完成");
        }


        private void UpdateAll(TreeListNode node)
        {
            foreach (TreeListNode child in node.Nodes)
            {
                if ((bool)child[treeIsSearch])
                {
                    m_controller.DownloadPageFromURL(int.Parse(child[treeOid].ToString()));
                }

                // 如果有子结点，就继续遍历
                if (child.Nodes.Count > 0)
                    UpdateAll(child);
            }

        }

        private void treeList1_Click(object sender, EventArgs e)
        {
            if (treeList1.FocusedNode != null && (bool)treeList1.FocusedNode[treeIsSearch] == true)
            {
                int oid = int.Parse(treeList1.FocusedNode[treeOid].ToString());
                if (oid != m_current_oid)
                {
                    m_pages = m_controller.GetSubPages(oid);
                    gridControl1.DataSource = m_pages;
                    m_current_oid = oid;
                }
            }
        }

        /// <summary>
        /// 列表点击时，设置为已读
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
                return;

            int oid = int.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridOid).ToString());
            //MessageBox.Show(oid.ToString());
            if (!(bool)gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridIsRead))
            {
                m_controller.SetPageHasRead(oid);       // 设置网页已读
                ChangeWebUnReadCount();                 // 更新数量
            }

            foreach (Page page in m_pages)
            {
                if (page.Oid == oid)
                {
                    page.Is_Read = true;
                    webBrowser1.DocumentText = page.DisplayContent();
                }
            }
            gridView1.RefreshData();


            //gridView1.SetFocusedRowCellValue(gridIsRead, true);

            //// 显示网页
            //string content = gridView1.GetFocusedRowCellValue(gridContent).ToString();
            //webBrowser1.DocumentText = content;
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
    }
}