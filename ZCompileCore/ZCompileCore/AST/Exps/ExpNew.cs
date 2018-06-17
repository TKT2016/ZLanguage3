using System;
using System.Collections.Generic;
using System.Reflection;
using ZCompileCore.Contexts;
using ZCompileDesc.Descriptions;
using System.Linq;
using ZCompileCore.Tools;
using ZLangRT.Utils;
using ZCompileCore.AST.Exps;
using ZCompileDesc.Utils;

namespace ZCompileCore.AST
{
    public class ExpNew:Exp
    {
        private ExpTypeBase _TypeExp;
        public ExpTypeBase TypeExp { get { return _TypeExp; } set { _TypeExp = value; _TypeExp.ParentExp = this; } }
        public ExpBracket _BracketExp;
        public ExpBracket BracketExp { get { return _BracketExp; } set { _BracketExp = value; _BracketExp.ParentExp = this; } }

        public ExpNew(ContextExp expContext, ExpTypeBase typeExp, ExpBracket bracketExp)
            : base(expContext)
        {
            InitExpNew(typeExp, bracketExp);
        }

        private void InitExpNew(ExpTypeBase typeExp, ExpBracket bracketExp)
        {
            TypeExp = typeExp;
            BracketExp = bracketExp;
        }

        private NewExpAnalyInfo NewAnalyInfo;
        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            TypeExp = (ExpTypeBase)(AnalySubExp(TypeExp)); 
            BracketExp = AnalySubExp(BracketExp) as ExpBracket; 
            if (!AnalyCorrect) return this;

            if (ZTypeUtil.IsListClass(TypeExp.RetType))
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
                Exp argExp = BracketExp.GetSubExps()[0];

                if (ZTypeUtil.IsExtends(argExp.RetType, TypeExp.RetType))
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
            ExpCast castExp = new ExpCast(this.ExpContext,this.TypeExp, this.BracketExp.GetSubExps()[0]);
            //castExp.SetContextExp(this.ExpContext);
            return castExp.Analy();
        }

        private ZLConstructorInfo SearchZConstructor()
        {
            int argsCount = BracketExp.Count;
            NewAnalyInfo = new NewExpAnalyInfo();

            var args = BracketExp.GetCallDesc();
            NewAnalyInfo.NewDesc = new ZNewCall(this.TypeExp.ToString(), args);

            var ZConstructors = (TypeExp.RetType as ZLClassInfo).SearchDeclaredZConstructor(NewAnalyInfo.NewDesc);
            if (ZConstructors.Length == 0) return null;
            else return ZConstructors[0];
        }

        private Exp AnalyNewExp()
        {
            ZLConstructorInfo ZConstructor = SearchZConstructor();
            if (ZConstructor == null)
            {
                Errorf(BracketExp.Position, "没有正确的创建过程");
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
            NewAnalyInfo.ArgExps = new List<Exp>( BracketExp.GetSubExps());
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

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
            TypeExp.SetParent(this);
            BracketExp.SetParent(this);
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { TypeExp, BracketExp };
        }

    }
}
