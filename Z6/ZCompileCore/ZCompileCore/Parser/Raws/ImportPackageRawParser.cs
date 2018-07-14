using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.ASTRaws;
using ZCompileCore.CommonCollections;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;

namespace ZCompileCore.Parsers.Raws
{
    class ImportPackageRawParser
    {
        TokenTape tape;
        ContextFile fileContext;

        public List<SectionImportRaw.PackageRaw> Parse(IEnumerable<LexToken> tokens, ContextFile fileContext)
        {
            this.fileContext = fileContext;
            List<LexToken> tokens2 = new List<LexToken>(tokens);

            tape = new TokenTape(tokens2.ToArray(), fileContext); 
            List<SectionImportRaw.PackageRaw> asts = ParsePackageList();
            return asts;
        }

        private List<SectionImportRaw.PackageRaw> ParsePackageList()
        {
            List<SectionImportRaw.PackageRaw> list = new List<SectionImportRaw.PackageRaw>();

            while (tape.HasCurrent && !tape.Current.IsKind( TokenKindSymbol.EOF))
            {
                if (tape.Current.IsKind(TokenKindKeyword.Ident))
                {
                    SectionImportRaw.PackageRaw pname = ParsePackageName(tape);
                    if (pname != null)
                    {
                        list.Add(pname);
                    }
                }
                else if (tape.Current.IsKind(TokenKindSymbol.Comma))
                {
                    tape.MoveNext();
                }
                else
                {
                    tape.error(string.Format("'{0}'不是正确的导入名称", tape.Current.Text));
                    tape.MoveNext();
                }
                //else if (tape.CurrentKind == TokenKind.NewLine)
                //{
                //    SkipNewLine();
                //}
                //else
                //{
                //    tape.error(string.Format("'{0}'不是正确的导入名称", tape.Current.GetText()));
                //    tape.MoveNext();
                //}
            }
            //if (tape.CurrentKind == TokenKindSymbol.Semi)
            //{
            //    tape.MoveNext();
            //}
            //SkipNewLine();
            //tape.Match(TokenKind.Semi);
            return list;
        }

        //private bool IsImportPackageNamePart(LexToken token)
        //{
        //    return token.IsKind(TokenKindKeyword.Ident) 
        //        || token.IsKind(TokenKindSymbol.DIV) 
        //        || token.IsKind(TokenKindSymbol.Comma);
        //}

        private SectionImportRaw.PackageRaw ParsePackageName(TokenTape tape)
        {
            SectionImportRaw.PackageRaw pname = new SectionImportRaw.PackageRaw();
            if (tape.HasCurrent )
            {
                if(tape.Current.IsKind(TokenKindKeyword.Ident))
                {
                    pname.Parts.Add((LexTokenText)tape.Current);
                    tape.MoveNext();
                    if(tape.HasCurrent)
                    {
                        if (tape.Current.IsKind(TokenKindSymbol.DIV))
                        {
                            ParsePackageNameBackPart(tape, pname);
                        }
                    }
                }
                else
                {
                    tape.error(string.Format("'{0}'不是正确的开发包名称", tape.Current.Text));
                    return null;
                }
            }
            else
            {
                tape.error(string.Format("'{0}'没有开发包名称", tape.Current.Text));
                return null;
            }
            return pname;
        }

        private void ParsePackageNameBackPart(TokenTape tape, SectionImportRaw.PackageRaw pname)
        {
            while (tape.HasCurrent && tape.Current.IsKind(TokenKindSymbol.DIV))
            {
                tape.MoveNext();
                if (tape.Current.IsKind(TokenKindKeyword.Ident))
                {
                    pname.Parts.Add((LexTokenText)tape.Current);
                    tape.MoveNext();
                }
                else
                {
                    tape.error(string.Format("'{0}'没有开发包名称", tape.Current.Text));
                }
            }
        }
    }
}
