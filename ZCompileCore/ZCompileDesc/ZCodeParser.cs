using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZLangRT;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;
using ZNLP;

namespace ZCompileDesc
{
    public class ZCodeParser
    {
        //UserWordsSegementer Segementer = new UserWordsSegementer();
        public ZCodeParser(Type type, MethodInfo method)
        {
            argTypeDict = new Dictionary<string, Type>();
             Init( type,  method);
        }

        #region 初始化类型参数
        private Dictionary<string, Type> argTypeDict;
        private void Init(Type type, MethodInfo method)
        {
            if (type.IsGenericType)
            {
                Type parentType = type.GetGenericTypeDefinition();
                Type[] subTypes = GenericUtil.GetInstanceGenriceType(type, parentType);
                Type[] gengeParams = parentType.GetGenericArguments();
                for (int i = 0; i < gengeParams.Length; i++)
                {
                    AddType(gengeParams[i].Name, subTypes[i]);
                }
            }
            ParameterInfo[] paramArray = method.GetParameters();

            foreach (var param in paramArray)
            {
                if (param.ParameterType.IsGenericType)
                {
                    var ptype = param.ParameterType;
                    Type parentType = ptype.GetGenericTypeDefinition();
                    Type[] subTypes = GenericUtil.GetInstanceGenriceType(ptype, parentType);
                    Type[] gengeParams = parentType.GetGenericArguments();
                    string[] subNames = new string[gengeParams.Length];
                    for (int i = 0; i < gengeParams.Length; i++)
                    {
                        string gengeParamName = gengeParams[i].Name;
                        subNames[i] = gengeParamName;
                    }
                    Type newType = ptype;
                    string newTypeName = GenericUtil.GetGenericTypeShortName(ptype) + "<" + string.Join(",", subNames) + ">";
                    AddType(newTypeName, newType);
                }
            }
        }

        private void AddType(Type type)
        {
            string name = type.Name;
            if (argTypeDict.ContainsKey(name) == false)
                argTypeDict.Add(name, type);
        }

        private void AddType(string name, Type type)
        {
            if (argTypeDict.ContainsKey(name) == false)
                argTypeDict.Add(name, type);
        }

        #endregion

        int i = 0;
        ZMethodDesc zmethodDesc = null;
        string Code = null;
        char ch
        {
            get
            {
                if (i > Code.Length - 1) return '\0';
                return Code[i];
            }
        }

        public ZMethodDesc Parser(string code)
        {     
            i = 0;
            zmethodDesc = new ZMethodDesc();
            Code = code;
            while (i < Code.Length)
            {
                if(ch=='(')
                {
                    parseBracket();
                }
                else
                {
                    parseText();
                }
            }
            return zmethodDesc;
        }

        void parseText()
        {
            StringBuilder buff = new StringBuilder();
            for (;  i < Code.Length&&ch != '(' ; i++)
            {
                buff.Append(ch);
            }
            string text = buff.ToString();
            zmethodDesc.Add(text);
            //string[] strarr = Segementer.Cut(text);
            //foreach(var  item in strarr)
            //{
            //    zmethodDesc.Add(item);
            //}
        }

        void parseBracket()
        {
            i++;
            ZBracketDefDesc zbracket = new ZBracketDefDesc();
            for (; i < Code.Length; i++ )
            {
                ZParam arg = ParseZParam();
                if (arg != null)
                {
                    zbracket.Add(arg);
                }
                if(ch=='\0')
                {
                    break;
                }
                if (ch == ')')
                {
                    i++;
                    break;
                } 
            }
            /* ZCode括号内无参数则去掉括号 */
            if (zbracket.ParamsCount > 0)
            {
                zmethodDesc.Add(zbracket);
            }
        }

        private ZParam ParseZParam()
        {
            string argTypeName = parseIdent();
            //if (argTypeName=="T")
            //{
            //    Console.WriteLine("T");
            //}
            movenext();
            string argname = parseIdent();
            if (string.IsNullOrEmpty(argname)) return null;
            if (argTypeDict.ContainsKey(argTypeName))
            {
                var genericRealTypeName = argTypeDict[argTypeName].Name;
                ZParam zp = new ZParam() { IsGenericArg = true , ZParamName = genericRealTypeName };
                return zp;
            }
            else
            {
                var ztype = GetZTypeByTypeName(argTypeName);
                if (ztype != null)
                {
                    ZParam zp = new ZParam() {  ZParamType=ztype , ZParamName = argname };
                    return zp;
                }
            }
            throw new ZyyRTException("没有导入'" + argTypeName + "'类型");
        }

        private ZType GetZTypeByTypeName(string typeName)
        {
            var ztypes = ZTypeManager.GetByMarkName(typeName);
            if (ztypes.Length == 0)
            {
                ztypes = ZTypeManager.GetBySharpName(typeName);
            }
            if (ztypes.Length > 0)
            {
                ZType ztype = ztypes[0] as ZType;
                //arg = new ZMethodArg(typeName, ztype);
                return ztype;
            }
            else
            {

            }
            return null;
        }

        string parseIdent()
        {
            StringBuilder buff = new StringBuilder();
            for (; i < Code.Length; i++)
            {
                if (ch == ':'||ch == ')'||ch == ',')
                {
                    break;
                }
                else if(ch!=' '&& ch!='\t')
                {
                    buff.Append(ch);
                }
            }
            return buff.ToString();
        }

        void movenext()
        {
            i++;
        }
    }
}
