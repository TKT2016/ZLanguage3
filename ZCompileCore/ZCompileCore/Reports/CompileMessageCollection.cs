using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileKit.Collections;
using ZCompileKit.Infoes;

namespace ZCompileCore.Reports
{
    public class CompileMessageCollection
    {
        public List< CompileMessage> Errors { get; private set; }
        public List<CompileMessage> Warnings { get; private set; }

        public CompileMessageCollection()
        {
            Errors = new List< CompileMessage>();
            Warnings = new List<CompileMessage>();
        }

        public void Clear()
        {
            if (Errors == null) Errors = new List<CompileMessage>();
            else Errors.Clear();

            if (Warnings == null) Warnings = new List<CompileMessage>();
            else Warnings.Clear();
        }

        public bool HasError()
        {
            return this.Errors.Count > 0;
        }

        public bool HasWarning()
        {
            return this.Warnings.Count > 0;
        }

        public void AddError(CompileMessage error)
        {
            Errors.Add(error);
        }

        public void AddWarning(CompileMessage error)
        {
            Warnings.Add(error);
        }

        public bool ContainsErrorSrcKey(string fileName)
        {
            foreach(var item in this.Errors)
            {
                if(item.Key is CompileMessageSrcKey)
                {
                    CompileMessageSrcKey srcKey = (item.Key as CompileMessageSrcKey);
                    if(srcKey.SrcFileName==fileName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
