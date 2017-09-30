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
using ZCompileCore.Engines;
using ZCompiler;
using ZDev.Schema.Parses;

namespace ZDev.Forms
{
    public partial class ProjectDockForm : DockFormBase
    {
        public ProjectDockForm()
        {
            InitializeComponent();
        }

        //TKTXMParser parser = new TKTXMParser();
        ZProjFileParser projFileParser = new ZProjFileParser();
        public void ClearTree()
        {
            this.projTreeView.Nodes.Clear();
        }

        public ZProjectModel ShowClass(FileInfo fi)
        {
            string[] lines = File.ReadAllLines(fi.FullName);
            ZProjectModel projectModel = projFileParser.ParseProjectFile(lines, fi.Directory.FullName);
            projectModel.AddRefPackage("Z语言系统");
            projectModel.AddRefPackage("Z标准包");

            if (projectModel != null)
            {
                ShowClass(projectModel, fi);
                return projectModel;
            }
            
            return null;
        }

        public void ShowClass(ZProjectModel tktclass, FileInfo fi)
        {
            ClearTree();
            if (tktclass == null) return;

            TreeNode xmNode = new TreeNode();
            xmNode.Text = fi.Name;
            xmNode.Tag = fi.FullName;
            projTreeView.Nodes.Add(xmNode);

            foreach (ZFileModel pp in tktclass.SouceFileList)
            {
                TreeNode pnode = new TreeNode();
                pnode.Text = pp.GetFileNameNoEx();
                pnode.Tag = pp.ZFileInfo.RealFilePath;// Path.Combine(fi.Directory.FullName, pp);
                projTreeView.Nodes.Add(pnode);
            }
            projTreeView.ExpandAll();
        }

        private void projTreeView_DoubleClick(object sender, EventArgs e)
        {
            TreeNode node = this.projTreeView.SelectedNode;
            if (node.Tag != null)
            {
                string tmb = node.Tag as string;
                if (tmb != null)
                {
                    this.MainWindow.OpenFile(tmb,false);
                }
            }
        }
    }
}
