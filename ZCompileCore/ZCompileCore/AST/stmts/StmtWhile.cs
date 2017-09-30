using System.Reflection.Emit;
using ZCompileCore.Lex;
using ZCompileDesc.Descriptions;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
   public  class StmtWhile:Stmt
    {
       public Token WhileToken { get; set; }
       public Exp Condition { get; set; }
       public StmtBlock WhileBody { get; set; }

       public override void Analy( )
       {
           Condition = AnalyCondition(Condition, WhileToken.Position);
           WhileBody.ProcContext = this.ProcContext;
           WhileBody.Analy();
       }

       public override void Emit()
       {
           var True_Label = IL.DefineLabel();
           var False_Label = IL.DefineLabel();

           IL.MarkLabel(True_Label);
           Condition.Emit();
           EmitHelper.LoadInt(IL, 1);
           IL.Emit(OpCodes.Ceq);
           IL.Emit(OpCodes.Brfalse, False_Label);
           WhileBody.Emit();
           IL.Emit(OpCodes.Br, True_Label);
           IL.MarkLabel(False_Label);
       }

    }
}
