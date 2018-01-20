
namespace ZCompileCore.ASTExps
{
    public interface ISetter
    {
        void EmitSet( Exp valueExp);
        bool CanWrite { get; }
    }
}
