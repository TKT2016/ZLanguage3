using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using ZCompileCore.Reports;
using ZDev.Forms;
using ZDev.UI.Compilers;

namespace ZDev
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            initDock();     
        }

        public WeifenLuo.WinFormsUI.Docking.DockPanel MainDockPanel { get { return this.mainDockPanel; } }

        MsgDockForm msgDock;
        ProcDockForm procDock;
        ProjectDockForm projDock;

        void initDock()
        {
            //MainDockPanel.Theme = this.vS2013BlueTheme1;
            //EnableVSRenderer(VSToolStripExtender.VsVersion.Vs2013);
            msgDock = new MsgDockForm();
            msgDock.ParentDockPanel = MainDockPanel;
            msgDock.Show(MainDockPanel);//msgDock.Show(MainDockPanel, DockAlignment.Bottom);
            msgDock.DockTo(MainDockPanel,DockStyle.Bottom);
            msgDock.MainWindow = this;

            procDock = new ProcDockForm();
            procDock.ParentDockPanel = MainDockPanel;
            procDock.Show(MainDockPanel);
            procDock.DockTo(MainDockPanel, DockStyle.Right);
            procDock.MainWindow = this;

            projDock = new ProjectDockForm();
            projDock.ParentDockPanel = MainDockPanel;
            projDock.Show(MainDockPanel);
            projDock.DockTo(MainDockPanel, DockStyle.Left);
            projDock.MainWindow = this;

            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = fileExtFilter;
        }

        public void OpenFile(string fileName,bool isShowProjFile =true)
        {
            FileInfo fi = new FileInfo(fileName);
            EditorDockForm cvf = FindEditorForm(fi);
            if (cvf != null)
            {
                cvf.Show(this.MainDockPanel);
            }
            else
            {
                cvf = newEditorDockForm();
                cvf.OpenFile(fi);
                cvf.ParentDockPanel = this.MainDockPanel;
                cvf.Show(this.MainDockPanel);
            }
            if (fileName.EndsWith(ZDevCompiler.ZYYExt , StringComparison.InvariantCultureIgnoreCase))
            {
                this.procDock.ShowClass(fi);
            }
            else if (fileName.EndsWith(ZDevCompiler.ZXMExt, StringComparison.InvariantCultureIgnoreCase))
            {
                this.procDock.ClearTree();
                if (isShowProjFile)
                {
                    this.projDock.ShowClass(fi);
                }
            }
        }
        OpenFileDialog openFileDialog1;// = new OpenFileDialog();
        string fileExtFilter = "Z文件 (*.zyy)|*.zyy|Z项目文件 (*.zxm)|*.zxm|文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*"; 
        void dialogOpenFile()
        {         
            if (string.IsNullOrEmpty(openFileDialog1.InitialDirectory))
                openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                OpenFile(openFileDialog1.FileName);
            }
        }
        int newIndex = 1;

        void NewFile()
        {
            EditorDockForm cvf = newEditorDockForm();
            cvf.NewFile(newIndex);
            cvf.ParentDockPanel = this.MainDockPanel;
            cvf.Editor.NewFile();
            cvf.Show(this.MainDockPanel);
            newIndex++;
        }

        private EditorDockForm newEditorDockForm()
        {
            EditorDockForm cvf = new EditorDockForm();
            return cvf;
        }

        private void newStripButton_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            dialogOpenFile();
        }

        EditorDockForm GetCurrentEditor()
        {
            EditorDockForm cvf = MainDockPanel.ActiveDocument as EditorDockForm;
            if (cvf!=null && cvf.Editor!=null)
            {
                if(cvf.Editor.KeyDownAction == null)
                {
                    cvf.Editor.KeyDownAction = (sender, e) => { 
                        if( KeyDownCtrlS(sender, e))
                        {
                            e.SuppressKeyPress = true;//阻止 Ctrl+S [DC3]
                        }  
                    };
                }
            }
            return cvf;
        }

        EditorDockForm FindEditorForm(FileInfo fi)
        {
            var docs = MainDockPanel.Documents;
            if (docs == null || docs.Count() == 0) return null;
            foreach(var doc in docs)
            {
                EditorDockForm dockForm = doc as EditorDockForm;
                if(dockForm!=null)
                {
                    if(dockForm.Editor.CurrentFile.FullName==fi.FullName)
                    {
                        return dockForm;
                    }
                }
            }      
            return null;
        }

        public void EditorGotoPostion(int position)
        {
            EditorDockForm editor = GetCurrentEditor();
            if(editor!=null)
            {
                editor.Editor.GotoPostion(position);
            }
        }

        private void saveStripButton_Click(object sender, EventArgs e)
        {
            saveText();
        }

        void saveText()
        {
            EditorDockForm edf = GetCurrentEditor();
            if (edf != null)
            {
                if(edf.Editor.CurrentFile!=null)
                {
                    edf.Editor.Save();
                }
                else
                {
                    saveAsText();
                }
            }
        }

        void saveAsText()
        {
            EditorDockForm edf = GetCurrentEditor();
            if (edf != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = fileExtFilter;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    edf.Editor.CurrentFile = new FileInfo(saveFileDialog.FileName);
                    edf.Editor.Save();
                }
            }
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyDownCtrlS(sender, e)) return;
        }

        private bool KeyDownCtrlS(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                saveText();
                return true;
            }
            return false;
        }

        private void mainDockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            EditorDockForm df = GetCurrentEditor();
            if(df!=null)
            {
                FileInfo fi = df.Editor.CurrentFile;
                if (fi != null)
                {
                    var fileName = fi.FullName;
                    if (fileName.EndsWith(ZDevCompiler.ZYYExt, StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.procDock.ShowClass(fi);
                    }
                    else if (fileName.EndsWith(ZDevCompiler.ZXMExt, StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.procDock.ClearTree();
                        //if (isShowProjFile)
                        //{
                            this.projDock.ShowClass(fi);
                        //}
                    }
                }
            }
        }

        private void runStripButton4_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void Run()
        {
            var editorForm = GetCurrentEditor();
            if (editorForm == null) return;
            if(editorForm.Editor.CurrentFile==null) return;
            this.msgDock.ResetConsoles();
            saveText();
            ZDevCompiler compiler = new ZDevCompiler(editorForm.Editor.CurrentFile);
            ProjectCompileResult results = compiler.Compile();
            this.msgDock.ShowErrors(results);
            if (!results.MessageCollection.HasError())//results.Errors.Count == 0)
            {
                try
                {
                    compiler.Run();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("程序运行错误", ex.Message);
                }
            }
            else
            {

            }
        }   
    }
}
