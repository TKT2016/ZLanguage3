using System;
using System.Collections.Generic;
using System.Reflection;
using ZCompileCore.Contexts;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZMembers;
using ZCompileDesc.ZTypes;
using System.Linq;
using ZCompileKit.Tools;
using ZLangRT.Utils;

namespace ZCompileCore.AST
{
    public class ExpNew:Exp
    {
        public ExpType TypeExp { get; set; }
        public ExpBracket BracketExp { get; set; }

        public ExpNew()
        {

        }

        public ExpNew(ExpType typeExp, ExpBracket bracketExp)
        {
            TypeExp = typeExp;
            BracketExp = bracketExp;
        }

        NewExpAnalyInfo NewAnalyInfo;
        public override Exp Analy( )
        {
            TypeExp = AnalySubExp(TypeExp) as ExpType; 
            BracketExp = AnalySubExp(BracketExp) as ExpBracket; 
            if (!AnalyCorrect) return this;

            if (IsListClass())
            {
                ExpNewList newListExp = new ExpNewList(this.ExpContext,TypeExp, BracketExp);
                return newListExp.Analy();
            }
            else
            {
                return AnalyNewExpOrQiangDiao();
            }
        }

        private Exp AnalyNewExpOrQiangDiao()
        {
            int argsCount = BracketExp.Count;
            if(argsCount!=1)
            {
                return AnalyNewExp();
            }
            else
            {
                Exp argExp = BracketExp.InneExps[0];

                if(ReflectionUtil.IsExtends(argExp.RetType.SharpType,TypeExp.RetType.SharpType))
                {
                    return argExp;
                }
                else
                {
                    return AnalyNewExpOneArg();
                }
            }
        }

        private Exp AnalyNewExpOneArg()
        {
            ZConstructorInfo ZConstructor = SearchZConstructor();
            if (ZConstructor == null)
            {
                //强制转换类型
                return AnalyCast();
            }
            else
            {
                AnlaySearchedConstructor(ZConstructor);
            }
            return this;
        }

        private Exp AnalyCast()
        {
            ExpCast castExp = new ExpCast(this.TypeExp, this.BracketExp.GetSubExps()[0]);
            return castExp.Analy();
        }

        private ZConstructorInfo SearchZConstructor()
        {
            int argsCount = BracketExp.Count;
            NewAnalyInfo = new NewExpAnalyInfo();

            var args = BracketExp.GetCallDesc();
            NewAnalyInfo.NewDesc = new ZNewDesc(args);
            ZConstructorInfo ZConstructor = (TypeExp.RetType as ZClassType).FindDeclaredZConstructor(NewAnalyInfo.NewDesc);
            return ZConstructor;
        }


        private Exp AnalyNewExp()
        {
            //int argsCount = BracketExp.Count;
            //NewAnalyInfo = new NewExpAnalyInfo();

            //var args = BracketExp.GetCallDesc();
            //NewAnalyInfo.NewDesc = new ZNewDesc(args);
            // (TypeExp.RetType as ZClassType).FindDeclaredZConstructor(NewAnalyInfo.NewDesc);
            ZConstructorInfo ZConstructor = SearchZConstructor();
            if (ZConstructor == null)
            {
                ErrorE(BracketExp.Postion, "没有正确的创建过程");
            }
            else
            {
               AnlaySearchedConstructor(ZConstructor);
            }
            return this;
        }

        private void AnlaySearchedConstructor(ZConstructorInfo ZConstructor)
        {
            RetType = TypeExp.RetType;
            NewAnalyInfo.SearchedZConstructor = ZConstructor;
            NewAnalyInfo.ArgExps = BracketExp.InneExps;
            NewAnalyInfo.AdjustArgExps();
        }

        private bool IsListClass()
        {
             Type subjectType = TypeExp.RetType.SharpType;
            if (!subjectType.Name.StartsWith(CompileConstant.ZListClassZName)) return false;
            if (subjectType.Namespace!= CompileConstant.LangPackageName) return false;
            return true;
        }

        public override void Emit()
        {
            EmitConstructor();
            var constructor = NewAnalyInfo.SearchedZConstructor.Constructor;
            EmitHelper.NewObj(IL, constructor);
            base.EmitConv();
        }

        private void EmitConstructor( )
        {
            var parameterInfos = NewAnalyInfo.SearchedZConstructor.Constructor.GetParameters();
            var argExps = NewAnalyInfo.AdjustedArgExps;
            base.EmitArgsExp(argExps, parameterInfos);
        }
        
        public override string ToString()
        {
            return TypeExp.ToString() + BracketExp.ToString();
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { TypeExp, BracketExp };
        }

    }
}
