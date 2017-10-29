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
using ZCompileCore.Reports;
using ZLogoCompiler;

namespace ZLogoIDE
{
    public partial class IDEForm : Form
    {
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private String FileFullPath;

        Boolean textChangedFlag = false;
        private const string EditorName = "ZLOGO 4.0";


        private CompileMsgForm compileMsgForm;
        public IDEForm()
        {
            InitializeComponent();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.Text = "无标题 - " + EditorName;
            compileMsgForm = new CompileMsgForm();
        }
        LogoCompiler compiler = new LogoCompiler();
        private void runStripButton_Click(object sender, EventArgs e)
        {
            if (FileFullPath == null) return;
            Console.WriteLine("runStripButton_Click FileFullPath=" + FileFullPath);
            if (Save())
            {
                compileMsgForm.Hide();
                LogoCompiler compiler = new LogoCompiler();// (new FileInfo(FileFullPath));
                var result= compiler.Compile(FileFullPath);
                if (result.HasError())
                {
                    StringBuilder buffBuilder = new StringBuilder();
                    buffBuilder.AppendFormat("文件'{0}{1}'有以下错误:\n", FileName, LogoCompiler.ZLogoExt);
                    foreach (CompileMessage compileMessage in result.Errors.ValuesToList())
                    {
                        if (compileMessage.Line > 0 || compileMessage.Col > 0)
                        {
                            buffBuilder.AppendFormat("第{0}行,第{1}列", compileMessage.Line, compileMessage.Col);
                        }
                        buffBuilder.AppendFormat("错误:{0}\n", compileMessage.Text);
                    }
                    compileMsgForm.ShowMessage(buffBuilder.ToString());
                    compileMsgForm.Show();
                }
                else if (!compiler.CheckRunZLogo(result))
                {
                    compileMsgForm.ShowMessage("程序没有‘开始画图’过程");
                }
                else
                {
                    compileMsgForm.Hide();
                    compiler.Run(result);
                }
                
            }
        }

        private void newStripButton_Click(object sender, EventArgs e)
        {
            Save();
            this.textBoxEditor.Text = "";
            this.FileFullPath = null;
            textChangedFlag = false;
        }

        private String FileName
        {
            get
            {
                 if (string.IsNullOrEmpty(FileFullPath)) return null;
                if (!File.Exists(FileFullPath)) return null;
                return Path.GetFileNameWithoutExtension(FileFullPath);
            }
        }
        string fileExtFilter = "ZLOGO文件 (*.zlogo)|*.zlogo|文本文件 (*.txt)|*.txt"; 
        private void openStripButton_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "";
            openFileDialog.Filter = fileExtFilter;
            if (FileFullPath == null)
            {
                openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                openFileDialog.InitialDirectory = (new FileInfo(FileFullPath)).DirectoryName;
            }
            DialogResult dialogResult = openFileDialog.ShowDialog();
            saveFileDialog.FileName = openFileDialog.FileName;
            if (dialogResult == DialogResult.OK)
            {
                FileFullPath = openFileDialog.FileName;
                StreamReader streamReader = new StreamReader(FileFullPath, System.Text.Encoding.Default);
                SetFormTitle();
                textBoxEditor.Text = streamReader.ReadToEnd();
                streamReader.Dispose();
            }              
        }

        private void SetFormTitle()
        {
            if (FileName == null)
            {
                this.Text = "无标题 - " + EditorName;
            }
            else
            {
                this.Text = FileName +" - " + EditorName;
            }
        }

        private void saveStripButton_Click(object sender, EventArgs e)
        {      
            Save(); 
        }

        private bool Save()
        {
            if (textChangedFlag)
            {
                if (FileFullPath == null)
                {
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        FileFullPath = saveFileDialog.FileName;
                        if (!FileFullPath.EndsWith(LogoCompiler.ZLogoExt, StringComparison.CurrentCultureIgnoreCase))
                        {
                            FileFullPath += LogoCompiler.ZLogoExt;
                        }

                        SaveText(FileFullPath); //textBoxEditor.SaveFile(FileFullPath, RichTextBoxStreamType.PlainText);
                        SetFormTitle();
                        textChangedFlag = false;
                        return true;
                    }
                }
                else
                {
                    FileInfo fileInfo = new FileInfo(FileFullPath);
                    fileInfo.Delete();
                    //textBoxEditor.SaveFile(FileFullPath, RichTextBoxStreamType.PlainText);
                    SaveText(FileFullPath);
                    textChangedFlag = false;
                    return true;
                }
                return false;
            }
            else
            {
                return true;
            } 
        }

        private void SaveText(string path)
        {
            String text = textBoxEditor.Text;
            FileUtil.WriteText(path,text);
        }

        private void saveAsStripButton_Click(object sender, EventArgs e)
        {
            if (FileName != null)
            {
                saveFileDialog.FileName = FileName + LogoCompiler.ZLogoExt;
            }
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveText(FileFullPath);// textBoxEditor.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
                textChangedFlag = false;
            }
        }

        private void aboutStripButton_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void IDEForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Save();
        }

        private void textBoxEditor_TextChanged(object sender, EventArgs e)
        {
            textChangedFlag = true;
        }
    }
}
