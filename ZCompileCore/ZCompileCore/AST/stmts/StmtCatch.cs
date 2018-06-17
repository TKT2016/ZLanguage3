using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;
using ZCompileCore.Lex;
using ZCompileCore.Parsers.Exps;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST
{
    public class StmtCatch:Stmt
    {
        //public StmtCatchRaw Raw;
        //public LexTokenText CatchToken { get; set; }
        //public LexToken ExceptionTypeVarToken { get; set; }
        //public StmtBlock CatchBody { get; set; }

        private StmtCatchRaw Raw;
        private StmtBlock StmtBody;

        public StmtCatch(StmtCatchRaw raw, Stmt parentStmt)
        {
            Raw = raw;
            ParentStmt = parentStmt;
            StmtBody = new StmtBlock(this, raw.CatchBody);
        }

        //public override CodePosition Position
        //{
        //    get { return CatchToken.Position; }
        //}
        string exTypeName;
        string exName;
        ZLType exType;
        ZCLocalVar exSymbol;

        public override Stmt Analy()
        {
            TypeArgParser parser = new TypeArgParser(this.ProcContext.ClassContext);
            TypeArgParser.ParseResult result = parser.Parse( Raw.ExceptionTypeVarToken);
            if (result.ResultCount == 1)
            {
                exTypeName = result.ArgZTypes[0].ZTypeName;
                exType = (ZLType)result.ArgZTypes[0];
                exName = result.ArgName;
            }
            if (this.ProcContext.ContainsVarName(exName) == false)
            {
                exSymbol = new ZCLocalVar(exName, exType, true);
                //exSymbol.LoacalVarIndex =this.ProcContext.CreateLocalVarIndex(exName);
                this.ProcContext.AddLocalVar(exSymbol);
            }
            else
            {
                if (this.ProcContext.LocalManager.IsDefLocal(exName))
                {
                    exSymbol = this.ProcContext.LocalManager.GetDefLocal(exName);
                    if (exSymbol.GetZType() != exType)
                    {
                        Errorf(Raw.ExceptionTypeVarToken.Position, "变量'{0}'的类型与异常的类型不一致", exName);
                    }
                }
                else
                {
                    Errorf(Raw.ExceptionTypeVarToken.Position, "变量名称'{0}'已经使用过", exName);
                }
            }
            //CatchBody.ProcContext = this.ProcContext;
            StmtBody.Analy();
            return this;
        }

        public override void AnalyExpDim()
        {
            StmtBody.AnalyExpDim();
        }

        public override void Emit()
        {
            //MarkSequencePoint(context);
            IL.BeginCatchBlock(exType.SharpType);
            EmitHelper.StormVar(IL, exSymbol.VarBuilder);
            StmtBody.Emit();
            IL.EndExceptionBlock();
        }

        public override string ToString()
        {
            return Raw.ToString();
        }

        //#region 覆盖
        

        //public CodePosition Postion
        //{
        //    get
        //    {
        //        return CatchToken.Position;
        //    }
        //}
        //#endregion

    }
}
