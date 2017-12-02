using System.Collections.Generic;
using ZCompileCore.Lex;
using ZCompileCore.Contexts;
using ZCompileCore.Parsers;
using ZCompileCore.AST;
using System.IO;
using ZCompileCore.Reports;

namespace ZCompileCore.Engines
{
    public class ZFileEngine
    {
        ContextProject projectContext;

        public ZFileEngine( ContextProject projectContext)
        {
            this.projectContext = projectContext;
        }

        public FileSource Parse(ZFileModel fileModel)
        {
            ContextFile fileContext = new ContextFile(this.projectContext, fileModel);
            List<LexToken> Tokens = Scan(fileContext, fileModel);
            FileSectionParser parser = new FileSectionParser();
            FileMutilType fileMutilType = parser.Parse(Tokens, fileContext);
            FileSource fileType = ParseSingleMutil(fileMutilType);
            if (fileType != null)
            {
                fileType.FileModel = fileModel;
                fileType.ProjectContext = this.projectContext;
            }
            return fileType;
        }

        #region Scan
        public List<LexToken> Scan(ContextFile fileContext, ZFileModel fileModel)
        {
            List<LexToken>  Tokens = new List<LexToken>();
            List<LexToken> preTokens = ScanPreCode(fileModel.ZFileInfo.FilePreText,fileContext);
            Tokens.AddRange(preTokens);

            List<LexToken> fileTokens = ScanFileCode(fileContext, fileModel);
            Tokens.AddRange(fileTokens);
            return Tokens;
        }

        private List<LexToken> ScanFileCode(ContextFile fileContext, ZFileModel fileModel)
        {
            
            if (fileModel.ZFileInfo.IsVirtual) return new List<LexToken> ();
            string srcFile = fileModel.ZFileInfo.RealFilePath;// zCompileClassModel.GetSrcFullPath();
            if (!File.Exists(srcFile))
            {
                projectContext.MessageCollection.AddError(
                    new CompileMessage(new CompileMessageSrcKey( fileModel.ZFileInfo.ZFileName), 0, 0, "源文件'" + srcFile + "'不存在"));
                return new List<LexToken>();
            }
            FileSourceReader reader = new FileSourceReader(srcFile);
            List<LexToken> tokens2 = ScanReaderTokens(reader, fileContext);
            return tokens2;
        }

        private List<LexToken> ScanPreCode(string preCode, ContextFile fileContext)
        {
            if (string.IsNullOrEmpty(preCode)) return new List<LexToken>();
            StringSourceReader reader = new StringSourceReader(preCode);
            List<LexToken> tokens2 = ScanReaderTokens(reader, fileContext);
            foreach (var token in tokens2)
            {
                token.Line = -token.Line - 1;
                token.Col = token.Col - 1000;//方法体以行列区分，所以减去一些。
            }
            return tokens2;
        }

        Tokenizer tokenizer = new Tokenizer();
        private List<LexToken> ScanReaderTokens(SourceReader reader, ContextFile fileContext)
        {
            List<LexToken> tokens = tokenizer.Scan(reader, fileContext);
            return tokens;
        }

        #endregion

        private FileSource ParseSingleMutil(FileMutilType fmt)
        {
            //int importCount = fmt.Importes.Count;
            //int useCount = fmt.Uses.Count;
            int propertyCount = fmt.Propertieses.Count;
            int procCount = fmt.Proces.Count;

            int enumCount = fmt.Enumes.Count;
            int dimCount = fmt.Dimes.Count;
            int classCount = fmt.Classes.Count;

            if (enumCount == 1 && dimCount == 0 && classCount == 0)
            {
                return new FileEnum(fmt.FileContext,fmt.Enumes);
            }
            else if (enumCount == 0 && dimCount == 1 && classCount == 0)
            {
                return new FileDim(fmt.FileContext, fmt.Dimes[0], fmt.ImporteSection);
            }
            else if (enumCount == 0  && classCount == 1)
            {
                FileClass fsc = new FileClass(fmt.FileContext, fmt);
                return fsc;
            }
            else if (enumCount == 0 && dimCount == 0 && classCount == 0)
            {
                return null;
            }
            else
            {
                return null;
            }
        }

        
    }
}
