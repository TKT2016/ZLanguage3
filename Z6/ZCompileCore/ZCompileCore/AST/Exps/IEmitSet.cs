
using ZCompileCore.AST.Exps;
namespace ZCompileCore.AST.Exps
{
    public interface IEmitSet
    {
        void EmitSet( Exp valueExp);
        bool CanWrite { get; }
    }
}
