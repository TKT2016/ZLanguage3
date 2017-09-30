using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT;

namespace ZCompileDesc.Descriptions
{
    public class ZCallDesc : ZProcDescBase,ICallDesc
    {
        List<string> StringParts { get; set; }
        List<ZBracketCallDesc> BracketParts { get; set; }
        public List<ZArg> CallArgs { get;private set; }

        public ZCallDesc()
        {
            Parts = new List<object>();
            StringParts = new List<string>();
            BracketParts = new List<ZBracketCallDesc>();
            CallArgs = new List<ZArg>();
        }

        public void Add(string str)
        {
            Parts.Add(str);
            StringParts.Add(str);
        }

        public void Add(ZBracketCallDesc zbracket)
        {
            Parts.Add(zbracket);
            BracketParts.Add(zbracket);
            CallArgs.AddRange(zbracket.Args);
        }

        public bool HasSubject()
        {
            if (this.Parts.Count <= 0) return false;
            if (!(this.Parts[0] is ZBracketCallDesc)) return false;
            if ((this.Parts[0] as ZBracketCallDesc).ArgsCount != 1) return false;
            return true;
        }

        public ZCallDesc CreateTail()
        {
            ZCallDesc tailDesc = new ZCallDesc();
            List<object> list = this.Parts;
            for (int i = 1; i < list.Count; i++)
            {
                object item = list[i];
                if (item is string)
                {
                    tailDesc.Add(item as string);
                }
                else if (item is ZBracketCallDesc)
                {
                    tailDesc.Add(item as ZBracketCallDesc);
                }
                else
                {
                    throw new ZyyRTException();
                }
            }
            return tailDesc;
        }

        public override string ToZCode()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < this.Parts.Count; i++)
            {
                if (this.Parts[i] is string)
                {
                    list.Add(this.Parts[i] as string);
                }
                else if (this.Parts[i] is ZBracketCallDesc)
                {
                    list.Add((this.Parts[i] as ZBracketCallDesc).ToZCode());
                }
                else
                {
                    throw new ZyyRTException();
                }
            }
            return string.Join("", list);
        }
    }
}
