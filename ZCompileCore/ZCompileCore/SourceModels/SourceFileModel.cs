using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileCore.SourceModels
{
    public class SourceFileModel
    {
        public string ShowKeyPath { get; private set; }
        public string SourceFileFullPath { get;private set; }
        public string SourceFileName{ get;private set; }

        public string GeneratedPackageName { get;private set; }
        public string GeneratedClassName { get;private set; }

        public string PreSourceCode{ get; set; }
        public int PreSourceStartLine{ get; set; }

        public string RealSourceCode{ get;private set; }
        public int RealSourceStartLine{ get;private set; }

        public string BackSourceCode{ get; set; }
        public int BackSourceStartLine{ get; set; }


        public SourceFileModel(string showPath, string sourceFileFullPath, string sourceFileName,
            string generatedPackageName ,string generatedClassName ,
            string realSourceCode ,int realSourceStartLine)
        {
            ShowKeyPath = showPath;
            SourceFileFullPath=sourceFileFullPath;
            SourceFileName=sourceFileName;
            GeneratedPackageName=generatedPackageName;
            GeneratedClassName=generatedClassName;  
            RealSourceCode=realSourceCode;
            RealSourceStartLine = realSourceStartLine;
        }
    }
}
