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
using ZCompileDesc.ZTypes;
using ZCompileDesc.ZMembers;
using ZCompileDesc.Collections;
using ZNLP;

namespace ZCompileCore.Contexts
{
    public class ContextFile
    {
        public ContextProject ProjectContext { get; set; }
        public ContextImportUse ImportUseContext { get; private set; }
        public ContextClass ClassContext { get; set; }
        public ZFileModel FileModel { get; private set; }  
        public IZDescType EmitedIZDescType { get; set; }

        string _KeyContext;

        public ContextFile(ContextProject projectContext,ZFileModel fileModel)
        {
            ProjectContext = projectContext;
            FileModel = fileModel;
            //ClassContext = new ContextClass(this);
            ImportUseContext = new ContextImportUse(this);
            _KeyContext = FileModel.GetFileNameNoEx();
        }

        #region error 

        private void Error(int line ,int col,string message)
        {
            var file = this.FileModel.ZFileInfo;
            CompileMessage cmsg = new CompileMessage( new CompileMessageSrcKey( file.ZFileName) , line, col, message);
            this.ProjectContext.MessageCollection.AddError( cmsg);
            CompileConsole.Error("文件 '" + file.ZFileName + "' 第" + line + "行,第" + col + "列错误:" + message);
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
    }
}
