using System.Collections.Generic;
using ZCompileCore.Lex;
using ZCompileCore.Contexts;
using ZCompileCore.Parsers;
using ZCompileCore.AST;
using System.IO;
using ZCompileCore.Reports;
using ZCompileCore.SourceModels;
using ZCompileCore.ASTRaws;
using System;
using ZCompileCore.Parsers.Raws;
using ZCompileCore.Parsers.Asts;

namespace ZCompileCore.Engines
{
    public class ZFileEngine
    {
        ContextProject projectContext;

        public ZFileEngine( ContextProject projectContext)
        {
            this.projectContext = projectContext;
        }

        public FileAST Parse(SourceFileModel fileModel)//FileSource
        {
            ContextFile fileContext = new ContextFile(this.projectContext, fileModel);
            List<LineTokenCollection> Tokens = Scan(fileContext, fileModel);
            //foreach (LineTokenCollection ltc in Tokens)
            //{
            //    Console.WriteLine(ltc);
            //    foreach(LexToken tok in ltc.ToList())
            //    {
            //        Console.Write(tok.Text+" ");
            //    }
            //    Console.WriteLine();
            //}
            FileRawParser parser = new FileRawParser();
            FileRaw fileRaw = parser.Parse(Tokens, fileContext); //FileMutilTypeRaw
            FileASTParser fileASTParser = new FileASTParser();
            FileAST fileAST = fileASTParser.Parse(fileRaw, fileContext);
            //FileSource fileType = ParseSingleMutil(fileMutilType);
            //if (fileType != null)
            //{
            //    fileType.FileModel = fileModel;
            //    fileType.ProjectContext = this.projectContext;
            //}
            //return fileType;
            return fileAST;
        }

        #region Scan
        public List<LineTokenCollection> Scan(ContextFile fileContext, SourceFileModel fileModel)
        {
            List<LineTokenCollection> Tokens = new List<LineTokenCollection>();
            if (!string.IsNullOrWhiteSpace(fileModel.PreSourceCode))
            {
                List<LineTokenCollection> preTokens = ScanTextCode(fileModel.PreSourceCode, fileContext, fileModel.PreSourceStartLine);
                Tokens.AddRange(preTokens);
            }

            if (!string.IsNullOrWhiteSpace(fileModel.RealSourceCode))
            {
                List<LineTokenCollection> realTokens = ScanTextCode(fileModel.RealSourceCode, fileContext, fileModel.RealSourceStartLine);
                Tokens.AddRange(realTokens);
            }

            if (!string.IsNullOrWhiteSpace(fileModel.BackSourceCode))
            {
                List<LineTokenCollection> backTokens = ScanTextCode(fileModel.BackSourceCode, fileContext, fileModel.BackSourceStartLine);
                Tokens.AddRange(backTokens);
            }
            return Tokens;
        }

        private List<LineTokenCollection> ScanTextCode(string code, ContextFile fileContext, int startLine)
        {
            if (string.IsNullOrEmpty(code)) return new List<LineTokenCollection>();
            StringSourceReader reader = new StringSourceReader(code);
            List<LineTokenCollection> tokens2 = ScanReaderTokens(reader, fileContext, startLine);
            //foreach (var token in tokens2)
            //{
            //    token.Line = -token.Line - 1;
            //    token.Col = token.Col - 1000;//方法体以行列区分，所以减去一些。
            //}
            return tokens2;
        }

        Tokenizer tokenizer = new Tokenizer();
        private List<LineTokenCollection> ScanReaderTokens(SourceReader reader, ContextFile fileContext, int startLine)
        {
            List<LineTokenCollection> tokens = tokenizer.Scan(reader, fileContext, startLine);
            return tokens;
        }

        #endregion

    }
}
