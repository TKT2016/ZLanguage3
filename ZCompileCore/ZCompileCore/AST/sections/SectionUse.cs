using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;
using ZCompileKit;

namespace ZCompileCore.AST
{
    public class SectionUse: SectionBase
    {
        public LexToken KeyToken;
        public List<LexToken> TypeNameTokens = new List<LexToken>();

        public void AddTypeToken(LexToken token)
        {
            TypeNameTokens.Add(token);
        }

        List<LexToken> _TextStructTokens;
        public override void AnalyText()
        {
            _TextStructTokens = new List<LexToken>();
            foreach (LexToken nameToken in this.TypeNameTokens)
            {
                AnalyNameItemText(nameToken);
            }
        }

        public override void AnalyType()
        {
            foreach (LexToken nameToken in this._TextStructTokens)
            {
                AnalyNameItemType(nameToken);
            }
        }

        public override void AnalyBody()
        {
            return;
        }

        public override void EmitName()
        {
            return;
        }

        public override void EmitBody()
        {
            return;
        }

        public void AnalyNameItemType(LexToken nameToken)
        {
            ContextImportUse importUseContext = this.FileContext.ImportUseContext;
            string typeName = nameToken.GetText();
            //if (typeName == "随机数器")
            //{
            //    Console.WriteLine("随机数器");
            //}
            IZDescType[] ztypes = importUseContext.SearchImportIZType_WithUse(typeName);
            if (ztypes.Length == 0)
            {
                this.FileContext.Errorf(nameToken.Position, "没有搜索到'{0}'", typeName);
                return;
            }
            IZDescType descType = ztypes[0];
            if (descType is ZClassType)
            {
                ZClassType zclass = descType as ZClassType;
                if (zclass.IsStatic)
                {
                    importUseContext.AddUseType(zclass);
                }
                else
                {
                    this.FileContext.Errorf(nameToken.Position, "'{0}'不是唯一类型，不能被简略使用", typeName);
                }
            }
            else if (descType is ZEnumType)
            {
                ZEnumType zenum = descType as ZEnumType;
                importUseContext.AddUseType(zenum);
            }
            else if (descType is ZDimType)
            {
                ZDimType zdim = descType as ZDimType;
                importUseContext.AddUseType(zdim);
            }
            else
            {
                throw new CCException();
            }
        }

        private void AnalyNameItemText(LexToken nameToken)
        {
            ContextImportUse contextiu = this.FileContext.ImportUseContext;
            string typeName = nameToken.GetText();
            if (contextiu.ContainsUserZTypeName(typeName))
            {
                this.FileContext.Errorf(nameToken.Position, "'{0}'重复使用", typeName);
            }
            else
            {
                contextiu.AddUseZTypeName(typeName);
                _TextStructTokens.Add(nameToken);
            }
        }

        public void SetContext(ContextFile fileContext)
        {
            this.FileContext = fileContext;
        }

        public override string ToString()
        {
            return KeyToken.GetText() + ":" + string.Join(".", TypeNameTokens.Select(p => p.GetText()));
        }
    }
}
