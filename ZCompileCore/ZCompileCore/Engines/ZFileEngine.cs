using System.Collections.Generic;
using ZCompileCore.Lex;
using ZCompileCore.Contexts;
using ZCompileCore.Parsers;
using ZCompileCore.AST;

namespace ZCompileCore.Engines
{
    public class ZFileEngine
    {
        //ZyyFileModel fileModel;
        ContextProject projectContext;
        //List<Token> Tokens;

        public ZFileEngine( ContextProject projectContext)
        {
            //this.fileModel = fileModel;
            this.projectContext = projectContext;
        }

        public FileType Parse(ZFileModel fileModel)
        {
            ContextFile fileContext = new ContextFile(this.projectContext, fileModel);
            List<Token> Tokens = Scan(fileContext, fileModel);
            FileSectionParser parser = new FileSectionParser();
            FileMutilType fileMutilType = parser.Parse(Tokens, fileContext);
            FileType fileType = ParseSingleMutil(fileMutilType);
            fileType.FileModel = fileModel;
            fileType.ProjectContext = this.projectContext;
            return fileType;
        }

        #region Scan
        public List<Token> Scan(ContextFile fileContext, ZFileModel fileModel)
        {
            List<Token>  Tokens = new List<Token>();
            List<Token> preTokens = ScanPreCode(fileModel.ZFileInfo.FilePreText,fileContext);
            Tokens.AddRange(preTokens);

            List<Token> fileTokens = ScanFileCode(fileContext, fileModel);
            Tokens.AddRange(fileTokens);
            return Tokens;
        }

        private List<Token> ScanFileCode(ContextFile fileContext, ZFileModel fileModel)
        {
            if (fileModel.ZFileInfo.IsVirtual) return new List<Token> ();
            string srcFile = fileModel.ZFileInfo.RealFilePath;// zCompileClassModel.GetSrcFullPath();
            FileSourceReader reader = new FileSourceReader(srcFile);
            List<Token> tokens2 = ScanReaderTokens(reader, fileContext);
            return tokens2;
        }

        private List<Token> ScanPreCode(string preCode, ContextFile fileContext)
        {
            if (string.IsNullOrEmpty(preCode)) return new List<Token>();
            StringSourceReader reader = new StringSourceReader(preCode);
            List<Token> tokens2 = ScanReaderTokens(reader, fileContext);
            foreach (var token in tokens2)
            {
                token.Line = -token.Line - 1;
                token.Col = token.Col - 1000;//方法体以行列区分，所以减去一些。
            }
            return tokens2;
        }

        Tokenizer tokenizer = new Tokenizer();
        private List<Token> ScanReaderTokens(SourceReader reader, ContextFile fileContext)
        {
            List<Token> tokens = tokenizer.Scan(reader, fileContext);
            return tokens;
        }

        #endregion

        private FileType ParseSingleMutil(FileMutilType fmt)
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
            else if (enumCount == 0  && classCount == 1)// (enumCount == 0 && dimCount == 0 && classCount == 1)
            {
                FileClass fsc = new FileClass(fmt.FileContext, fmt);
                //FileClass fsc = new FileClass()
                //{
                //    ClassSection = fmt.Classes[0],
                //    ImporteSection = fmt.ImporteSection,
                //    Proces = fmt.Proces,
                //    UseSection = fmt.UseSection

                //};
                //if (fmt.Dimes.Count > 0)
                //{
                //    fsc.DimSection = fmt.Dimes[0];
                //}
                //if (fmt.Propertieses.Count > 0)
                //{
                //    fsc.PropertiesesSection = fmt.Propertieses[0];
                //}
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

        //public void AnalyTypeName()
        //{
        //    fileType.AnalyTypeName();
        //}

        //public void EmitTypeName()
        //{
        //    fileType.EmitTypeName();
        //}

        //public void AnalyEnum()
        //{
        //    if(fileType is FileEnum )
        //    {
        //        (fileType as FileEnum).AnalyEnum();
        //    }
        //    else if (fileType is FileMutilType)
        //    {
        //        (fileType as FileMutilType).AnalyEnum();
        //    }
        //}

        //public void EmitEnum()
        //{
        //    if (fileType is FileEnum)
        //    {
        //        (fileType as FileEnum).EmitEnum();
        //    }
        //    else if (fileType is FileMutilType)
        //    {
        //        (fileType as FileMutilType).EmitEnum();
        //    }
        //}

        //public void AnalyDim()
        //{
        //    if (fileType is FileDim)
        //    {
        //        (fileType as FileDim).AnalyDim();
        //    }
        //    else if (fileType is FileClass)
        //    {
        //        (fileType as FileClass).AnalyDim();
        //    }
        //    else if (fileType is FileMutilType)
        //    {
        //        (fileType as FileMutilType).AnalyDim();
        //    }
        //}

        //public void EmitDim()
        //{
        //    if (fileType is FileDim)
        //    {
        //        (fileType as FileDim).EmitDim();
        //    }
        //    else if (fileType is FileClass)
        //    {
        //        (fileType as FileClass).EmitDim();
        //    }
        //    else if (fileType is FileMutilType)
        //    {
        //        (fileType as FileMutilType).EmitDim();
        //    }
        //}

        //public void EmitClassMember()
        //{
        //    if (fileType is FileClass)
        //    {
        //        (fileType as FileClass).EmitClassMember();
        //    }
        //    else if (fileType is FileMutilType)
        //    {
        //        (fileType as FileMutilType).EmitClassMember();
        //    }
        //}

        //public void AnalyClassMember()
        //{
        //    if (fileType is FileClass)
        //    {
        //        (fileType as FileClass).AnalyClassMember();
        //    }
        //    else if (fileType is FileMutilType)
        //    {
        //        (fileType as FileMutilType).AnalyClassMember();
        //    }
        //}

        //public void EmitProperties()
        //{
        //    if (fileType is FileClass)
        //    {
        //        (fileType as FileClass).EmitProperties();
        //    }
        //    else if (fileType is FileMutilType)
        //    {
        //        (fileType as FileMutilType).EmitProperties();
        //    }
        //}

        //public void AnalyProperties()
        //{
        //    if (fileType is FileClass)
        //    {
        //        (fileType as FileClass).AnalyProperties();
        //    }
        //    else if (fileType is FileMutilType)
        //    {
        //        (fileType as FileMutilType).AnalyProperties();
        //    }
        //}

        //public void EmitProc()
        //{
        //    if (fileType is FileClass)
        //    {
        //        (fileType as FileClass).EmitProc();
        //    }
        //    else if (fileType is FileMutilType)
        //    {
        //        (fileType as FileMutilType).EmitProc();
        //    }
        //}

        //public void AnalyProc()
        //{
        //    if (fileType is FileClass)
        //    {
        //        (fileType as FileClass).AnalyProc();
        //    }
        //    else if (fileType is FileMutilType)
        //    {
        //        (fileType as FileMutilType).AnalyProc();
        //    }
        //}

        //public IZDescType[] CreateZType()
        //{
        //    return fileType.CreateZType();
        //}
    }
}
