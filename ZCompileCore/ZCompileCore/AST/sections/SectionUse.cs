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
        public Token KeyToken;
        public List<Token> TypeNameTokens = new List<Token>();

        public void AddTypeToken(Token token)
        {
            TypeNameTokens.Add(token);
        }

        public void Analy()
        {
            foreach (var nameToken in TypeNameTokens)
            {
                AnalyNameItem(nameToken);
            }
        }

        public void AnalyNameItem(Token nameToken)
        {
            ContextUse useContext = this.FileContext.UseContext;
            string typeName = nameToken.GetText();
            if (useContext.ContainsType(typeName))
            {
                this.FileContext.Errorf(nameToken.Position, "'{0}'已经被使用", typeName);
            }
            else
            {
                IZDescType[] ztypes = this.FileContext.ImportContext.ImportPackageDescList.SearchZDescType(typeName);
                IZDescType descType = ztypes[0];
                if (descType is ZClassType)
                {
                    ZClassType zclass = descType as ZClassType;
                    if (zclass.IsStatic)
                    {
                        useContext.UseZClassList.Add(zclass);
                    }
                    else
                    {
                        this.FileContext.Errorf(nameToken.Position, "'{0}'不是唯一类型，不能被简略使用", typeName);
                    }
                }
                else if (descType is ZEnumType)
                {
                    ZEnumType zenum = descType as ZEnumType;
                    useContext.UseZEnumList.Add(zenum);
                }
                else if (descType is ZDimType)
                {
                    ZDimType zdim = descType as ZDimType;
                    useContext.UseZDimList.Add(zdim);
                }
                else
                {
                    throw new CompileCoreException();
                }
            }
        }

        public override string ToString()
        {
            return KeyToken.GetText() + ":" + string.Join(".", TypeNameTokens.Select(p => p.GetText()));
        }
    }
}
