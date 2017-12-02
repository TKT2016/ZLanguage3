﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileCore.AST;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZMembers;

namespace ZCompileCore.AST
{
    internal class NewExpAnalyInfo// : ZNewDesc
    {
        public List<Exp> ArgExps { get; set; }
        public List<Exp> AdjustedArgExps { get; set; }
        public ZNewDesc NewDesc { get; set; }
        public ZConstructorInfo SearchedZConstructor { get; set; }

        public void AdjustArgExps()
        {
            ConstructorInfo Constructor = SearchedZConstructor.Constructor;
            AdjustedArgExps = CallAjuster.AdjustExps(Constructor.GetParameters(), ArgExps);
        }
    }
}
