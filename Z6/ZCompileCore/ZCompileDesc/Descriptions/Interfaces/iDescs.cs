using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Descriptions.Utils;

namespace ZCompileDesc.Descriptions
{
    public interface IParts { 
        int GetPartCount();
        object GetPart(int i);
        object[] GetParts();
    }

    public interface IBracket
    {
        int GetParametersCount();
        IParameter[] GetParameters();
    }

    public interface IParameter
    {
        string GetZParamName();
        bool HasCallParameterName();
        bool IsCallArg();
        ZType GetZParamType();
        bool IsRuntimeType { get; }
    }

    public interface IParameterCollection
    {
       
    }

    public interface IIdent
    {
        string ZName{get;}
        ZType GetZType();
        bool GetCanRead();
        bool GetCanWrite();
    }
}
