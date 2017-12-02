using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileKit.Collections;
using ZCompileDesc.Descriptions;
using ZCompileKit.Infoes;
using ZCompileDesc.ZTypes;

namespace ZCompileCore.Reports
{
    public class ProjectCompileResult
    {
        public CompileMessageCollection MessageCollection { get; set; }
        //public List<CompileMessage> Errors { get; set; }
        //public List<CompileMessage> Warnings { get;set; }

        public string BinaryFilePath { get; set; }
        public List<IZDescType> CompiledTypes { get; set; }
        public ZType EntrtyZType { get; set; }

        public ProjectCompileResult() 
        {
            CompiledTypes = new List<IZDescType>();
        }

        
    }
}
