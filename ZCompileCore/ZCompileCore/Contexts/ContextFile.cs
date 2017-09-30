using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Engines;
using ZCompileCore.Lex;
using ZCompileCore.Reports;
using ZCompileCore.Symbols;
using ZLangRT;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;
using ZCompileDesc.ZTypes;
using ZCompileDesc.ZMembers;
using ZCompileDesc.Collections;

namespace ZCompileCore.Contexts
{
    public class ContextFile : IWordDictionary
    {
        public ContextProject ProjectContext { get; set; }

        public ContextImport ImportContext;
        public ContextUse UseContext { get; private set; }
        public ContextClass ClassContext { get; set; }
      
        public ZFileModel FileModel;
       
        public ZDimType EmitedZDim;
        public ZType EmitedZClass;
        //public string ContextKey { get; protected set; }
        public string ContextKey { get { return FileModel.GetFileNameNoEx() + ""; } }
        public ContextFile(ContextProject projectContext,ZFileModel fileModel)
        {
            ProjectContext = projectContext;
            FileModel = fileModel;
            UseContext = new ContextUse();
            this.UseContext.SetFileContext(this);
            ImportContext = new ContextImport();
            ClassContext = new ContextClass(this);
            //ContextKey = fileModel.GetFileNameNoEx();
            //SymbolTable = UseContext.SymbolTable;
        }


        #region IWordDictionary实现
        public bool ContainsWord(string text)
        {
            return ImportContext.ContainsWord(text)
                || ClassContext.ContainsWord(text)
                || (EmitedZDim == null ? false : EmitedZDim.ContainsWord(text))
            ;
        }

        public WordInfo SearchWord(string text)
        {
            WordInfo info1 = ImportContext.SearchWord(text);
            WordInfo info2 = ClassContext.SearchWord(text);
            WordInfo info3 = null;
            if (EmitedZDim!=null)
            {
                info3 = EmitedZDim.SearchWord(text);
            }
            WordInfo newWord = WordInfo.Merge(info1, info2, info3);
            return newWord;
        }
        #endregion

        public ZMethodInfo[] SearchUseProc(ZCallDesc procDesc)
        {
            return UseContext.SearchProc(procDesc);
        }

        public IZDescType[] SearchZDescType(string zname)
        {
            return this.ImportContext.ImportPackageDescList.SearchZDescType(zname);
        }

        #region error 

        private void Error(int line ,int col,string message)
        {
            var file = this.FileModel.ZFileInfo;
            CompileMessage cmsg = new CompileMessage(file, line, col, message);
            this.ProjectContext.CompileResult.Errors.Add(file, cmsg);
            CompileConsole.Error("文件" + file.ZFileName + " 第" + line + "行,第" + col + "列错误:" + message);
        }

        public void Errorf(int line, int col, string messagef, params object[] args)
        {
            string msg = string.Format(messagef, args);
            Error(line,col,msg);
        }

        public void Errorf(CodePosition position, string messagef, params object[] args)
        {
            string msg = string.Format(messagef, args);
            Error(position.Line, position.Col, msg);
        }

        #endregion
        //public ISymbolTable SymbolTable { get; set; }
        
    }
}
