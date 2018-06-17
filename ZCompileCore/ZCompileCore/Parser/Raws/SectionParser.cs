using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.AST;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.CommonCollections;

namespace ZCompileCore.Parsers.Raws
{
    public class SectionParser
    {
        private ArrayTape<LineTokenCollection> tape;

        private ContextFile fileContext;
        private List<SectionRaw> Sections = new List<SectionRaw>();

        public List<SectionRaw> Parse(IEnumerable<LineTokenCollection> lineTokens,ContextFile fileContext)
        {
            this.fileContext = fileContext;
            Sections.Clear();
            List<LineTokenCollection> tokens = new List<LineTokenCollection>(lineTokens);

            tape = new ArrayTape<LineTokenCollection>(tokens.ToArray());

            while(tape.HasCurrent)
            {
                if(IsSectionHead(tape.Current))
                {
                    SectionRaw section = ParseSection();
                    if (section != null)
                    {
                        Sections.Add(section);
                    }
                }
                else
                {
                    if (tape.Current.Count > 0)
                    {
                        error(tape.Current.FirstToken, "错误的段头");
                    }
                    tape.MoveNext();
                }
            }
            return Sections;
        }

        private SectionRaw ParseSection()
        {
            string headText = tape.Current.FirstToken.Text;
            bool secondIsColon = tape.Current.SecondToken.IsKind(TokenKindSymbol.Colon);
            if(false)
            {
                //<<重新编写分析，同行换行都可以>>
            }
            else if (secondIsColon && headText=="导入包" )
            {
                SectionImportRaw section = ParseImport();
                return section;
            }
            else if (secondIsColon && headText == "导入类")
            {
                SectionUseRaw section = ParseUse();
                return section;
            }
            //else if (secondIsColon && headText == "约定")
            //{
            //    SectionEnum section = ParseEnum();
            //    return section;
            //}
            else if (secondIsColon && headText == "名称")
            {
                SectionNameRaw section = ParseClassName();
                return section;
            }
            else if (secondIsColon && headText == "属于")
            {
                SectionExtendsRaw section = ParseExtends();
                return section;
            }
            //else if (secondIsColon && headText == "声明" )
            //{
            //    SectionDim section = ParseDim();
            //    return section;
            //}
            else if (secondIsColon && headText == "属性" )
            {
                SectionPropertiesRaw section = ParseProperties();
                return section;
            }
            else
            {
                SectionProcRaw section = ParseProc();
                return section;    
            }
        }

        #region parse import

        private SectionImportRaw ParseImport()
        {
            SectionImportRaw ast = new SectionImportRaw();
            ast.KeyToken = (LexTokenText)tape.Current.FirstToken;
            List<SectionImportRaw.PackageRaw> packages = new List<SectionImportRaw.PackageRaw>();

            /*分析'导入包:'后面的包名称 */
            List<LexToken> packagesTokens;// = new List<LexToken>();
            packagesTokens = tape.Current.GetSubs(2);// new List<LexToken>();
            ParseImportPackages(packages,packagesTokens);
            tape.MoveNext();

            while(tape.HasCurrent)
            {
                if(IsSectionHead(tape.Current))
                {
                    break;
                }
                packagesTokens = tape.Current.ToList();
                ParseImportPackages(packages,packagesTokens);
                tape.MoveNext();
            }
            ast.Packages = packages;
            return ast;
        }

        private void ParseImportPackages(List<SectionImportRaw.PackageRaw> packages, List<LexToken> packagesTokens)
        {
            ImportPackageRawParser packageParser = new ImportPackageRawParser();
            List<SectionImportRaw.PackageRaw> package1 = packageParser.Parse(packagesTokens, this.fileContext);
            packages.AddRange(package1);
        }

        #endregion

        #region parse use

        private SectionUseRaw ParseUse()
        {
            SectionUseRaw ast = new SectionUseRaw();
            ast.KeyToken = (LexTokenText)tape.Current.FirstToken;
            List<LexToken> tokens = tape.Current.GetSubs(2); 
            ParseUseClass(ast,tokens);
            tape.MoveNext();
            while(tape.HasCurrent)
            {
                if(IsSectionHead(tape.Current))
                {
                    break;
                }
                List<LexToken> tokens2 = tape.Current.ToList();
                ParseUseClass(ast,tokens2);
                tape.MoveNext();
            }
            return ast;
        }

        private void ParseUseClass(SectionUseRaw ast, List<LexToken> sourceTokens)
        {
            ImportPackageRawParser packageParser = new ImportPackageRawParser();
            for (int i = 0; i < sourceTokens.Count;i++ )
            {
                LexToken token = sourceTokens[i];
                if (token.IsKind(TokenKindKeyword.Ident))
                {
                    ast.Parts.Add((LexTokenText)token);
                }
                else if (token.IsKind(TokenKindSymbol.Comma))
                {
                    
                }
                else
                {
                    error(token,string.Format("使用的'{0}'不是正确的类型名称", token.Text));
                }
            }
        }
        #endregion

        public void error(LexToken tok, string str)
        {
            this.fileContext.Errorf(tok.Position, str);
        }

        #region parse class

        private SectionExtendsRaw ParseExtends()
        {
            SectionExtendsRaw ast = new SectionExtendsRaw();
            LexToken headToken = tape.Current.FirstToken;
            LineTokenCollection lineTokens = tape.Current;
            if (lineTokens.Count == 3)
            {
                ast.BaseTypeToken = (LexTokenText)lineTokens.Get(2);
            }
            else if (lineTokens.Count > 3)
            {
                error(lineTokens.Get(3), string.Format("不支持多继承'{0}'", lineTokens.Get(3)));
            }
            tape.MoveNext();
            return ast;
        }

        #endregion

        #region parse class

        private SectionNameRaw ParseClassName()
        {
            SectionNameRaw ast = new SectionNameRaw();
            LexToken headToken = tape.Current.FirstToken;
            LineTokenCollection lineTokens = tape.Current;
            if(lineTokens.Count==3)
            {
                ast.NameToken = (LexTokenText)lineTokens.Get(2);
            }
            else if (lineTokens.Count >3 )
            {
                error(lineTokens.Get(3), string.Format("类型名称后的'{0}'是多余的", lineTokens.Get(3)));
            }
            //string headFirstText = headToken.Text;
            //if (headFirstText.Length > 2)
            //{
            //    string baseTypeName = headFirstText.Substring(0, headFirstText.Length - 2);
            //    ast.BaseTypeToken = new LexTokenText( headToken.Line, headToken.Col,baseTypeName);
            //}
            //else
            //{
            //    ast.BaseTypeToken = null;
            //}

            //if(tape.Current.Count>=3)
            //{
            //    LexToken nameToken = tape.Current.Get(2);
            //    if (nameToken.IsKind(TokenKindKeyword.Ident))
            //    {
            //        ast.NameToken = nameToken;
            //        tape.MoveNext();
            //    }
            //}
            //if (tape.Current.Count > 3)
            //{
            //    for(int i=3;i<tape.Current.Count;i++)
            //    {
            //        LexToken tok = tape.Current.Get(i);
            //        error(tok, string.Format("类型名称后的'{0}'是多余的", tok.Text));
            //    }
            //}
            tape.MoveNext();
            return ast;
        }

        #endregion

        #region parse Properties

        private SectionPropertiesRaw ParseProperties()
        {
            SectionPropertiesRaw ast = new SectionPropertiesRaw();
            List<PropertyASTRaw> list = new List<PropertyASTRaw>();
            LexToken headFistToken = tape.Current.FirstToken;
            ast.KeyToken = (LexTokenText)headFistToken;

            List<LexToken> sourceTokens = tape.Current.GetSubs(2);
            ParsePropertyBody( list,sourceTokens);
            tape.MoveNext();
            while(tape.HasCurrent)
            {
                //SkipSpaceLine();
                if(IsSectionHead(tape.Current))
                {
                    break;
                }
                sourceTokens = tape.Current.ToList();
                ParsePropertyBody( list,sourceTokens);
                tape.MoveNext();
            }

            ast.Properties.AddRange(list);
            return ast;
        }

        private void ParsePropertyBody(List<PropertyASTRaw> list, List<LexToken> sourceTokens)
        {
             ASTPropertiesParser pparser = new ASTPropertiesParser();
             List<PropertyASTRaw> ppts = pparser.Parse(sourceTokens, this.fileContext);
            list.AddRange(ppts);
        }
        #endregion

        #region Parse 过程

        private SectionProcRaw ParseProc()
        {
            SectionProcRaw ast = new SectionProcRaw();
            ASTProcNameParser procnameparser = new ASTProcNameParser();
            ast = procnameparser.Parse(tape.Current.ToList() , this.fileContext, ast);
            if (ast.NamePart == null)
            {
                tape.MoveNext();
                return null;
            }
            tape.MoveNext();
            ast.Body = ParseProcBody();
            return ast;
        }

        private StmtBlockRaw ParseProcBody()
        {
            StmtBlockRaw raw = new StmtBlockRaw();
            while (tape.HasCurrent && !IsSectionHead(tape.Current))
            {
                raw.LineTokens.Add(tape.Current);
                tape.MoveNext();
            }
            return raw;
        }
        #endregion

        #region 辅助方法
        //private void SkipSpaceLine()
        //{
        //    while (tape.HasCurrent)
        //    {
        //        if (tape.Current.Count == 0)
        //        {
        //            tape.MoveNext();
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //}

        private bool IsSectionHead(LineTokenCollection ltc)
        {
            if (ltc.Count < 2) return false;
            int bracketCounter = 0;
            for (int i = 0; i < ltc.Count; i++)
            {
                LexToken token = ltc.Get(i);
                if (token.IsKind(TokenKindSymbol.LBS))
                {
                    bracketCounter++;
                }
                else if (token.IsKind(TokenKindSymbol.RBS))
                {
                    bracketCounter--;
                }
                else if (token.IsKind(TokenKindSymbol.Colon))
                {
                    if (bracketCounter == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
    }
}
