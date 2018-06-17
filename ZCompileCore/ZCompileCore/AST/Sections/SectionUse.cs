using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST
{
    public class SectionUse
    {
        private FileAST ASTFile;
        private SectionUseRaw Raw;

        public SectionUse(FileAST fileAST, SectionUseRaw raw)
        {
            ASTFile = fileAST;
            Raw = raw;
        }

        public SectionUse( SectionUseRaw raw)
        {
            Raw = raw;
        }
        private List<LexTokenText> _TextStructTokens;

        public void Analy()//FileAST fileAST)
        {
            //ASTFile = fileAST;

            _TextStructTokens = new List<LexTokenText>();
            foreach (LexTokenText nameToken in this.Raw.Parts)
            {
                AnalyNameItemText(nameToken);
            }

            foreach (LexTokenText nameToken in this._TextStructTokens)
            {
                AnalyNameItemType(nameToken);
            }
        }

        private void AnalyNameItemText(LexTokenText nameToken)
        {
            ContextImportUse contextiu = this.ASTFile.FileContext.ImportUseContext;
            string typeName = nameToken.Text;
            if (contextiu.ContainsUserZTypeName(typeName))
            {
                this.ASTFile.FileContext.Errorf(nameToken.Position, "'{0}'重复使用", typeName);
            }
            else
            {
                contextiu.AddUseZTypeName(typeName);
                _TextStructTokens.Add(nameToken);
            }
        }

        public void AnalyNameItemType(LexTokenText nameToken)
        {
            ContextImportUse importUseContext = this.ASTFile.FileContext.ImportUseContext;
            string typeName = nameToken.Text;
            var ztypes = importUseContext.SearchByTypeName(typeName);
            if (ztypes.Length == 0)
            {
                this.ASTFile.FileContext.Errorf(nameToken.Position, "没有搜索到'{0}'", typeName);
                return;
            }
            var descType = ztypes[0];
            if (descType is ZLClassInfo)
            {
                ZLClassInfo zclass = descType as ZLClassInfo;
                if (zclass.IsStatic)
                {
                    importUseContext.AddUseType(zclass);
                }
                else
                {
                    this.ASTFile.FileContext.Errorf(nameToken.Position, "'{0}'不是唯一类型，不能被导入类", typeName);
                }
            }
            else if (descType is ZLEnumInfo)
            {
                ZLEnumInfo zenum = descType as ZLEnumInfo;
                importUseContext.AddUseType(zenum);
            }
            else if (descType is ZLDimInfo)
            {
                ZLDimInfo zdim = descType as ZLDimInfo;
                importUseContext.AddDimType(zdim);
            }
            else
            {
                throw new CCException();
            }
        }
    }
}
