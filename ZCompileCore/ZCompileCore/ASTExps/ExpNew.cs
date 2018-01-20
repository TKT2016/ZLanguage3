using System;
using System.Collections.Generic;
using System.Reflection;
using ZCompileCore.Contexts;
using ZCompileDesc.Descriptions;
using System.Linq;
using ZCompileKit.Tools;
using ZLangRT.Utils;
using ZCompileCore.ASTExps;
using ZCompileDesc.Utils;

namespace ZCompileCore.AST
{
    public class ExpNew:Exp
    {
        public ExpTypeBase TypeExp { get; set; }
        public ExpBracket BracketExp { get; set; }

        public ExpNew()
        {

        }

        public ExpNew(ExpTypeBase typeExp, ExpBracket bracketExp)
        {
            TypeExp = typeExp;
            BracketExp = bracketExp;
        }

        NewExpAnalyInfo NewAnalyInfo;
        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            TypeExp = (ExpTypeBase)(AnalySubExp(TypeExp)); 
            BracketExp = AnalySubExp(BracketExp) as ExpBracket; 
            if (!AnalyCorrect) return this;

            if (ZTypeUtil.IsListClass(TypeExp.RetType))//(IsListClass())
            {
                ExpNewList newListExp = new ExpNewList(this.ExpContext,TypeExp, BracketExp);
                IsAnalyed = true;
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

                if (ZTypeUtil.IsExtends(argExp.RetType, TypeExp.RetType))//(ReflectionUtil.IsExtends(argExp.RetType.SharpType,TypeExp.RetType.SharpType))
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
            ZLConstructorInfo ZConstructor = SearchZConstructor();
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
            castExp.SetContext(this.ExpContext);
            return castExp.Analy();
        }

        private ZLConstructorInfo SearchZConstructor()
        {
            int argsCount = BracketExp.Count;
            NewAnalyInfo = new NewExpAnalyInfo();

            var args = BracketExp.GetCallDesc();
            NewAnalyInfo.NewDesc = new ZNewCall(this.TypeExp.ToString(), args);
            //ZLConstructorInfo ZConstructor = (TypeExp.RetType as ZLClassInfo).SearchDeclaredZConstructor(NewAnalyInfo.NewDesc)[0];
            //return ZConstructor;
            var ZConstructors = (TypeExp.RetType as ZLClassInfo).SearchDeclaredZConstructor(NewAnalyInfo.NewDesc);
            if (ZConstructors.Length == 0) return null;
            else return ZConstructors[0];
        }

        private Exp AnalyNewExp()
        {
            ZLConstructorInfo ZConstructor = SearchZConstructor();
            if (ZConstructor == null)
            {
                ErrorF(BracketExp.Position, "没有正确的创建过程");
            }
            else
            {
               AnlaySearchedConstructor(ZConstructor);
            }
            return this;
        }

        private void AnlaySearchedConstructor(ZLConstructorInfo ZConstructor)
        {
            RetType = TypeExp.RetType;
            NewAnalyInfo.SearchedZConstructor = ZConstructor;
            NewAnalyInfo.ArgExps = BracketExp.InneExps;
            NewAnalyInfo.AdjustArgExps();
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
