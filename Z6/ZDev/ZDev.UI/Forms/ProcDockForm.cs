using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZDev.Forms
{
    public partial class ProcDockForm : DockFormBase
    {
        public ProcDockForm()
        {
            InitializeComponent();
        }

        //SchemaScanner scanner = new SchemaScanner();
        //SchemaParser parser = new SchemaParser();

        public void ClearTree()
        {
            this.procTreeView.Nodes.Clear();
        }

        //public TKTClassModel ShowClass(FileInfo fi)
        //{
        //    string code =  File.ReadAllText(fi.FullName);
        //    var tokens = scanner.Scan(code);
        //    TKTClassModel tktclass = parser.Parse(tokens);
        //    if (tktclass.NameModel==null)
        //    {
        //        tktclass.NameModel = new TKTContentModel() { Content = Path.GetFileName(fi.FullName) };
        //    }
        //    ShowClass(tktclass);
        //    return tktclass;
        //}

        Font TipFont = new Font("宋体",10, FontStyle.Bold);
        Font TipFont2 = new Font("宋体", 10);
        Color TipColor = Color.AliceBlue;

        void InitTipNode(TreeNode node,string text)
        {
            node.Text = text+" ";
            node.NodeFont = TipFont;
            node.ForeColor = TipColor;
            node.BackColor = Color.FromArgb(0X29,0X39,0X55);
        }

        //public void ShowClass(TKTClassModel tktclass)
        //{
        //    this.procTreeView.Nodes.Clear();
        //    if (tktclass == null) return;

        //    TreeNode nameNode = new TreeNode();
        //    nameNode.Text = tktclass.NameModel.Content+"    ";
        //    nameNode.NodeFont = TipFont;
        //    nameNode.ForeColor = Color.BlueViolet;
        //    nameNode.Tag = tktclass.NameModel;
        //    procTreeView.Nodes.Add(nameNode);

        //    TreeNode baseNode = new TreeNode();
        //    InitTipNode(baseNode, "属于");
        //    nameNode.Nodes.Add(baseNode);

        //    TreeNode baseNameNode = new TreeNode();
        //    if (tktclass.BaseType!=null)
        //        baseNameNode.Text = tktclass.BaseType.Content;
        //    baseNode.Nodes.Add(baseNameNode);

        //    /*------------------------- 导入 ---------------------*/

        //    TreeNode importPackageNode = new TreeNode();
        //    InitTipNode(importPackageNode, "导入");
        //    importPackageNode.NodeFont = TipFont2;
        //    nameNode.Nodes.Add(importPackageNode);

        //    foreach (TKTContentModel pp in tktclass.UsingPackages)
        //    {
        //        TreeNode pnode = new TreeNode();
        //        pnode.Text = pp.Content;
        //        pnode.Tag = pp;
        //        importPackageNode.Nodes.Add(pnode);
        //    }

        //    TreeNode importRedirectNode = new TreeNode();
        //    InitTipNode(importRedirectNode, "使用");
        //    importRedirectNode.NodeFont = TipFont2;
        //    nameNode.Nodes.Add(importRedirectNode);

        //    foreach (TKTContentModel pp in tktclass.RedirectTypes)
        //    {
        //        TreeNode pnode = new TreeNode();
        //        pnode.Text = pp.Content;
        //        pnode.Tag = pp;
        //        importRedirectNode.Nodes.Add(pnode);
        //    }

        //    if (tktclass.BaseType.Content == "约定类型")
        //    {
        //        TreeNode enumNode = new TreeNode();
        //        InitTipNode(enumNode, "约定");
        //        nameNode.Nodes.Add(enumNode);              

        //        foreach (var ei in tktclass.EnumItems)
        //        {
        //            TreeNode pnode = new TreeNode();
        //            pnode.Text = ei.Content;
        //            pnode.Tag = ei;
        //            enumNode.Nodes.Add(pnode);
        //        }
        //    }
        //    else
        //    {
        //        /*--------------------- 属性 ------------------------*/
        //        TreeNode propertyNode = new TreeNode();
        //        InitTipNode(propertyNode, "属性");
        //        nameNode.Nodes.Add(propertyNode);

        //        foreach(TKTPropertyModel pp in tktclass.PropertyList)
        //        {
        //            TreeNode pnode = new TreeNode();
        //            pnode.Text = pp.ToString();
        //            pnode.Tag = pp;
        //            propertyNode.Nodes.Add(pnode);
        //        }

        //        /*------------------------ 构造函数 ---------------------------*/
        //        TreeNode constractNode = new TreeNode();
        //        InitTipNode(constractNode, "创建过程");
        //        nameNode.Nodes.Add(constractNode);

        //        foreach (TKTConstructionModel tc in tktclass.ContructList)
        //        {
        //            TreeNode pnode = new TreeNode();
        //            pnode.Text = tc.ToString();
        //            pnode.Tag = tc;
        //            constractNode.Nodes.Add(pnode);
        //        }

        //        /*------------------------ 过程 ---------------------------*/
        //        TreeNode procNode = new TreeNode();
        //        InitTipNode(procNode, "过程");
        //        nameNode.Nodes.Add(procNode);

        //        foreach (TKTProcModel tp in tktclass.ProcList)
        //        {
        //            TreeNode pnode = new TreeNode();
        //            pnode.Text = tp.ToString();
        //            pnode.Tag = tp;
        //            procNode.Nodes.Add(pnode);
        //        }
        //    }
        //    this.procTreeView.ExpandAll();
        //    //importNode.Collapse();
        //}

        private void procTreeView_DoubleClick(object sender, EventArgs e)
        {
        //    TreeNode node = this.procTreeView.SelectedNode;
        //    if (node.Tag != null)
        //    {
        //        TKTModelBase tmb = node.Tag as TKTModelBase;
        //        if (tmb != null)
        //        {
        //            if (tmb.Postion == null) return;
        //            if (tmb.Postion.Index <= 0) return;
        //            MainWindow.EditorGotoPostion(tmb.Postion.Index);
        //        }
        //    }
       }
    }
}
