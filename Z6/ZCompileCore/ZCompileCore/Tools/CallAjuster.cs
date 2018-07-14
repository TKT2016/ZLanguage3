using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileCore.AST;
using ZCompileDesc.Descriptions;
using ZCompileCore;
using ZCompileCore.AST.Exps;

namespace ZCompileCore.Tools
{
    public static class CallAjuster
    {
        public static bool IsNeedAdjust(List<Exp> exps )
        {
            int size = exps.Count;
            if (size == 0) return false;
            if (size == 1) return false;
            if (exps[0] is ExpNameValue)
            {
                return true;
            }
            return false;
        }

        public static List<Exp> AdjustExps(ParameterInfo[] paramArr, List<Exp> exps)
        {
            if (!IsNeedAdjust(exps))
            {
                return exps;
            }
            var AdjustedArgExps = new List<Exp>();
            Dictionary<string, ExpNameValue> argsDict = new Dictionary<string, ExpNameValue>();
            foreach (var arg in exps)
            {
                if(arg is ExpNameValue)
                {
                    ExpNameValue env = arg as ExpNameValue;
                    argsDict.Add(env.ArgName, env);
                }
                else
                {
                    throw new CCException();
                }
            }
            foreach (var pi in paramArr)
            {
                string paramName = pi.Name;
                ExpNameValue exp = argsDict[paramName];
                AdjustedArgExps.Add(exp);
            }
            return AdjustedArgExps;
        }

        public static List<Exp> AdjustExps(ZLParamInfo[] paramArr, List<Exp> exps)
        {
            if (!IsNeedAdjust(exps))
            {
                return exps;
            }
            var AdjustedArgExps = new List<Exp>();
            Dictionary<string, ExpNameValue> argsDict = new Dictionary<string, ExpNameValue>();
            foreach (var arg in exps)
            {
                if (arg is ExpNameValue)
                {
                    ExpNameValue env = arg as ExpNameValue;
                    argsDict.Add(env.ArgName, env);
                }
                else
                {
                    throw new CCException();
                }
            }
            foreach (var pi in paramArr)
            {
                string paramName = pi.ZParamName;
                ExpNameValue exp = argsDict[paramName];
                AdjustedArgExps.Add(exp);
            }
            return AdjustedArgExps;
        }

        public static List<Exp> AdjustExps(ZCParamInfo[] paramArr, List<Exp> exps)
        {
            if (!IsNeedAdjust(exps))
            {
                return exps;
            }
            var AdjustedArgExps = new List<Exp>();
            Dictionary<string, ExpNameValue> argsDict = new Dictionary<string, ExpNameValue>();
            foreach (var arg in exps)
            {
                if (arg is ExpNameValue)
                {
                    ExpNameValue env = arg as ExpNameValue;
                    argsDict.Add(env.ArgName, env);
                }
                else
                {
                    throw new CCException();
                }
            }
            foreach (var pi in paramArr)
            {
                string paramName = pi.ZParamName;
                ExpNameValue exp = argsDict[paramName];
                AdjustedArgExps.Add(exp);
            }
            return AdjustedArgExps;
        }
    }
}
