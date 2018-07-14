using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.CommonCollections;
using ZCompileDesc.Descriptions;
//using ZCompileKit.Infoes;

using ZCompileDesc.Collections;
using ZCompileCore.SourceModels;

namespace ZCompileCore.Reports
{
    public class ProjectCompileResult
    {
        public CompileMessageCollection MessageCollection { get; set; }
        public SourceProjectModel ProjectModel { get; set; }
        public string BinaryFilePath { get; set; }
        public ZLCollection CompiledTypes { get; set; }
        public ZLType EntrtyZType { get; set; }

        public ProjectCompileResult() 
        {
            CompiledTypes = new ZLCollection();
        }

        
    }
}
