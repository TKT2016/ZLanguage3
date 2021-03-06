﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Engines;
using ZCompileCore.Lex;
using ZCompileCore.Reports;

using ZLangRT;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;

using ZCompileDesc.Collections;
using ZCompileNLP;
using ZCompileCore.SourceModels;

namespace ZCompileCore.Contexts
{
    public class ContextFile
    {
        public ContextProject ProjectContext { get; set; }
        public ContextImportUse ImportUseContext { get; private set; }
        public SourceFileModel FileModel { get; private set; }
        public ZLClassInfo EmitedIZDescType { get; set; }

        string _KeyContext;

        public ContextFile(ContextProject projectContext, SourceFileModel fileModel)
        {
            ProjectContext = projectContext;
            FileModel = fileModel;
            ImportUseContext = new ContextImportUse(this);
            _KeyContext =  FileModel.GeneratedClassName;
        }

        #region error 
        
        public bool HasError()
        {
            var MessageCollection = this.ProjectContext.MessageCollection;
            if (MessageCollection.ContainsErrorSrcKey(this.FileModel.ShowKeyPath))
            {
                return true;
            }
            return false;
        }

        private void Error(int line ,int col,string message)
        {
            var file = this.FileModel.ShowKeyPath;//.ZFileInfo;
            CompileMessage cmsg = new CompileMessage(new CompileMessageSrcKey(file), line, col, message);
            this.ProjectContext.MessageCollection.AddError( cmsg);
            CompileConsole.Error("文件 '" + file + "' 第" + line + "行,第" + col + "列错误:" + message);
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
