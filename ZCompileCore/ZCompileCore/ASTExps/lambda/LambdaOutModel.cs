using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.ASTExps
{
    public class LambdaOutModel
    {
        /// <summary>
        /// 方法内或局部变量
        /// </summary>
        public List<ZCParamInfo> BodyZParams { get; set; }

        /// <summary>
        /// 方法内或局部变量
        /// </summary>
        public List<ZCLocalVar> BodyZVars { get; set; }

        /// <summary>
        /// 实际运行的exp
        /// </summary>
        public Exp ActionExp { get; set; }

        /// <summary>
        /// lambda返回类型
        /// </summary>
        public ZType FnRetType { get; set; }

        /// <summary>
        /// lambda调用的属性exp
        /// </summary>
        public List<ExpFieldPropertyBase> BodyFieldExps { get; set; }

        /// <summary>
        /// 方法内局部变量exp
        /// </summary>
        public List<ExpLocal> BodyLocalExps { get; set; }

        /// <summary>
        /// 方法内参数exp
        /// </summary>
        public List<ExpArg> BodyArgExps { get; set; }

        public LambdaOutModel(Exp actionExp, ZType fnRetType)
        {
            ActionExp = actionExp;
            FnRetType = fnRetType;

            BodyZParams = new List<ZCParamInfo>();
            BodyZVars = new List<ZCLocalVar>();
            BodyFieldExps = new List<ExpFieldPropertyBase>();
            BodyLocalExps = new List<ExpLocal>();
            BodyArgExps = new List<ExpArg>();

            InitSub();
        }

        private void InitSub()
        {
            List<Exp> exps = GetAllSubExps(ActionExp);
            foreach (var item in exps)
            {
                if(item is ExpLocalVar)
                {
                    ExpLocalVar varExp = (ExpLocalVar)item;
                    BodyLocalExps.Add(varExp);
                    BodyZVars.Add(varExp.LocalVarSymbol);
                }
                else if (item is ExpArg)
                {
                    ExpArg argExp = (ExpArg)item;
                    BodyLocalExps.Add(argExp);
                    BodyZParams.Add(argExp.ArgSymbol);
                }
                else if (item is ExpFieldPropertyBase)
                {
                    ExpFieldPropertyBase fieldExp = (ExpFieldPropertyBase)item;
                    BodyFieldExps.Add(fieldExp);
                }
            }
            BodyLocalExps = BodyLocalExps.Distinct().ToList();
            BodyZVars = BodyZVars.Distinct().ToList();
            BodyLocalExps = BodyLocalExps.Distinct().ToList();
            BodyZParams = BodyZParams.Distinct().ToList();
            BodyFieldExps = BodyFieldExps.Distinct().ToList();
        }

        private List<Exp> GetAllSubExps(Exp exp)
        {
            List<Exp> list = new List<Exp>();
            AddSubExp(exp, list);
            return list;
        }

        private void AddSubExp(Exp exp, List<Exp> list)
        {
            list.Add(exp);
            Exp[] subexps = exp.GetSubExps();
            foreach (var expsub in subexps)
            {
                AddSubExp(expsub, list);
            }
        }
    }
}
