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
    public class FileRawParser
    {
        private List<SectionRaw> Sections ;
        private FileRaw fileRaw;

        public FileRaw Parse(IEnumerable<LineTokenCollection> lineTokens, ContextFile fileContext)
        {
            SectionParser sectionParser = new SectionParser ();
            List<LineTokenCollection> tempLineTokens = new List<LineTokenCollection>();
            foreach (var item in lineTokens)
            {
                if(item.Count>0)
                {
                    tempLineTokens.Add(item);
                }
            }
            Sections = sectionParser.Parse(tempLineTokens, fileContext);
            fileRaw = new FileRaw();
            fileRaw.Sections = Sections;
            return fileRaw;
        }
         
    }
}
