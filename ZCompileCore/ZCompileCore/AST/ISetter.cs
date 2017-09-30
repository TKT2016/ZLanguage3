
namespace ZCompileCore.AST
{
    public interface ISetter
    {
        void EmitSet( Exp valueExp);
        bool CanWrite { get; }
    }
}
