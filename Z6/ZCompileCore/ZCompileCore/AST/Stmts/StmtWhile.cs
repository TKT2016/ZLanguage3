using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST.Exps;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;

namespace ZCompileCore.AST
{
    public class StmtWhile : Stmt
    {
        private StmtWhileRaw Raw;
        private StmtBlock StmtBody;
        private Exp ConditionExp;

        public StmtWhile(StmtWhileRaw raw, Stmt parentStmt)
        {
            Raw = raw;
            ParentStmt = parentStmt;
            StmtBody = new StmtBlock(this, raw.WhileBody);
        }

        public override Stmt Analy()
        {
            //ConditionExp.IsTopExp = true;
            //ConditionExp.IsTopExp = true;
            ConditionExp = AnalyWhileExpRaw(Raw.ConditionExp);// ParseAnalyRawExp(Raw.ConditionExp);
            ConditionExp = ConditionExp.Analy();
            //WhileBody.ProcContext = this.ProcContext;
            StmtBody.Analy();
            return this;
        }

        public override void AnalyExpDim()
        {
            ConditionExp.AnalyDim();
            StmtBody.AnalyExpDim();
        }

        private Exp AnalyWhileExpRaw(ExpRaw rawExp)
        {
            //ExpRaw rawExp = (ExpRaw)ConditionExp;
            //ContextExp context = new ContextExp(this.ProcContext, this);
            //rawExp.SetContext(context);
            List<LexToken> tokens = rawExp.RawTokens; //rawExp.Seg();
            if (tokens.Count > 0)
            {
                var lastIndex = tokens.Count - 1;
                
                var RepeatToken = tokens[lastIndex];
                if (RepeatToken is LexTokenText && RepeatToken.Text == "重复")//|| TimesToken.IsKeyIdent("次"))
                {
                    tokens.RemoveAt(lastIndex);
                }
                else
                {
                    Errorf(RepeatToken.Position, "循环语句的条件末尾缺少‘重复’");
                }
                //if (RepeatToken.Kind == TokenKindKeyword.Repeat)// == "重复" )//|| RepeatToken.IsKeyIdent("重复"))
                //{
                //    tokens.RemoveAt(lastIndex);
                //}
            }
            Exp exp = ParseAnalyRawExp(rawExp);
            return exp;
            //ExpParser parser = new ExpParser();
            //Exp exp = parser.Parse(tokens, this.FileContext);
            ////exp.SetContextExp(rawExp.ExpContext);
            //return exp;
        }

        //public override CodePosition Position
        //{
        //    get { return DangToken.Position; }
        //}

        public override void Emit()
        {
            var True_Label = IL.DefineLabel();
            var False_Label = IL.DefineLabel();

            IL.MarkLabel(True_Label);
            ConditionExp.Emit();
            EmitHelper.LoadInt(IL, 1);
            IL.Emit(OpCodes.Ceq);
            IL.Emit(OpCodes.Brfalse, False_Label);
            StmtBody.Emit();
            IL.Emit(OpCodes.Br, True_Label);
            IL.MarkLabel(False_Label);
        }

        public override string ToString()
        {
            return Raw.ToString();
        }
    }
}