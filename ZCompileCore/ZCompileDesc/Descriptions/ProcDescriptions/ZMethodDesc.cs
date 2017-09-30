using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;
using ZCompileDesc.ZMembers;
using ZCompileDesc.ZTypes;
using ZLangRT;

namespace ZCompileDesc.Descriptions
{
    public class ZMethodDesc : ZProcDescBase,IDefDesc
    {
        public ZMethodInfo ZMethod { get; set; }
        List<string> StringParts { get; set; }
        List<ZBracketDefDesc> BracketParts { get; set; }
        public List<ZParam> DefArgs { get;private set; }

        public ZMethodDesc()
        {
            Parts = new List<object>();
            StringParts = new List<string>();
            BracketParts = new List<ZBracketDefDesc>();
            DefArgs = new List<ZParam>();
        }

        public void Add(string str)
        {
            Parts.Add(str);
            StringParts.Add(str);
        }

        public void Add(ZBracketDefDesc zbracket)
        {
            Parts.Add(zbracket);
            BracketParts.Add(zbracket);
            DefArgs.AddRange(zbracket.Params);
        }

        public bool ContainsWord(string text)
        {
            int index = StringParts.IndexOf(text);
            return index != -1;
        }

        public WordInfo SearchWord(string text)
        {
            if (!ContainsWord(text)) return null;
            WordInfo info = new WordInfo(text, WordKind.ProcNamePart, this);
            return info;
        }

        public bool ZEquals(ZMethodDesc zdef)
        {
            //#region DEBUG
            //string zdefcode = zdef.ToZCode();
            //string zcallcode = zdef.ToZCode();
            //if (zdefcode.StartsWith("打印") && zdefcode.StartsWith("打印"))
            //{
            //    Console.WriteLine("ZCallDesc.Compare " + zdefcode);
            //}
            //#endregion
            if (this.PartsCount != zdef.PartsCount) return false;
            int size = this.PartsCount;
            for (int i = 0; i < size; i++)
            {
                var callitem = this.Parts[i];
                var defitem = zdef.Parts[i];
                if (callitem is string)
                {
                    if ((defitem is string) == false) return false;
                    var callstr = callitem as string;
                    var defstr = defitem as string;
                    if (callstr != defstr) return false;
                }
                else if (callitem is ZBracketDefDesc)
                {
                    if ((defitem is ZBracketDefDesc) == false) return false;
                    var callbracket = callitem as ZBracketDefDesc;
                    var defbracket = defitem as ZBracketDefDesc;
                    if (!defbracket.ZEquals(callbracket)) return false;
                }
                else
                {
                    throw new ZyyRTException();
                }
            }
            return true;
        }

        public bool ZEquals(ZCallDesc zcall)
        {
            if (this.PartsCount != zcall.PartsCount) return false;
            int size = this.PartsCount;
            for (int i = 0; i < size; i++)
            {
                var defitem = this.Parts[i];
                var callitem  = zcall.Parts[i];
                if (defitem is string)
                {
                    if (!(callitem is string)) return false;
                    var callstr = callitem as string;
                    var defstr = defitem as string;
                    if (callstr != defstr) return false;
                }
                else if (defitem is ZBracketDefDesc )
                {
                    if ((callitem is ZBracketCallDesc) == false) return false;
                    var callbracket = callitem as ZBracketCallDesc;
                    var defbracket = defitem as ZBracketDefDesc;
                    if (!defbracket.ZEquals(callbracket)) return false;
                }
                else
                {
                    throw new ZyyRTException();
                }
            }
            return true;
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
                else if (this.Parts[i] is ZBracketDefDesc)
                {
                    list.Add( (this.Parts[i] as ZBracketDefDesc).ToZCode() );
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
