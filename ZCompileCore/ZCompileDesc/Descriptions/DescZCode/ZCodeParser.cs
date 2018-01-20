using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZLangRT;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileNLP;

namespace ZCompileDesc.Descriptions
{
    public class ZCodeParser
    {
        ZLMethodInfo zlmethod;
        int paramIndex = 0;
        //UserWordsSegementer Segementer = new UserWordsSegementer();
        public ZCodeParser(Type type, ZLMethodInfo method)
        {
            zlmethod = method;
            //genericTypeDict = GenericUtil.GetTypeNameGenericArgTypes(type); //new Dictionary<string, Type>();
            Init( type,  method.SharpMethod);
        }

        #region 初始化类型参数
        //private Dictionary<string, Type> genericTypeDict;
        //private Dictionary<string, Type> argTypeDict;
        //private Dictionary<string, ParameterInfo> paramsDict;

        private void Init(Type type, MethodInfo method)
        {
            //argTypeDict = new Dictionary<string, Type>();
            //paramsDict = new Dictionary<string, ParameterInfo>();
            //foreach(var key in genericTypeDict.Keys)
            //{
            //    AddType(key, genericTypeDict[key]);
            //}
            //if (type.IsGenericType)
            //{
            //    Type parentType = type.GetGenericTypeDefinition();
            //    Type[] subTypes = GenericUtil.GetInstanceGenriceType(type, parentType);
            //    Type[] gengeParams = parentType.GetGenericArguments();
            //    for (int i = 0; i < gengeParams.Length; i++)
            //    {
            //        AddType(gengeParams[i].Name, subTypes[i]);
            //    }
            //}
            //ParameterInfo[] paramArray = method.GetParameters();

            //foreach (var param in paramArray)
            //{
            //    //paramsDict.Add(param.Name, param);
            //    if (param.ParameterType.IsGenericType)
            //    {
            //        var ptype = param.ParameterType;
            //        Type parentType = ptype.GetGenericTypeDefinition();
            //        Type[] subTypes = GenericUtil.GetInstanceGenriceType(ptype, parentType);
            //        Type[] gengeParams = parentType.GetGenericArguments();
            //        string[] subNames = new string[gengeParams.Length];
            //        for (int i = 0; i < gengeParams.Length; i++)
            //        {
            //            string gengeParamName = gengeParams[i].Name;
            //            subNames[i] = gengeParamName;
            //        }
            //        Type newType = ptype;
            //        string newTypeName = GenericUtil.GetGenericTypeShortName(ptype) + "<" + string.Join(",", subNames) + ">";
            //        AddType(newTypeName, newType);
            //    }
            //}
        }

        //private void AddType(string name, Type type)
        //{
        //    if (argTypeDict.ContainsKey(name) == false)
        //        argTypeDict.Add(name, type);
        //}

        #endregion

        int i = 0;
        ZLMethodDesc zmethodDesc = null;
        string Code = null;
        char ch
        {
            get
            {
                if (i > Code.Length - 1) return '\0';
                return Code[i];
            }
        }

        public ZLMethodDesc Parser(string code)
        {     
            i = 0;
            paramIndex = 0;
            zmethodDesc = new ZLMethodDesc(this.zlmethod, code);
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
            ZLBracketDesc zbracket = new ZLBracketDesc();
            for (; i < Code.Length; i++ )
            {
                ZLParamInfo arg = ParseZParam();
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

        private ParameterInfo GetMethodParameterInfo(string parameterName)
        {
            ZLParamInfo zp =  this.zlmethod.SearchParameter(parameterName);
            if (zp == null) return null;
            return zp.ParameterInfo;
            //var parameters = this.zlmethod.SharpMethod.GetParameters();
            //foreach(var item in parameters)
            //{
            //    if(item.Name==parameterName )
            //    {
            //        return item;
            //    }
            //}
            //return null;
        }

        private ZLParamInfo ParseZParam()
        {
            string argTypeName = parseIdent(); 
            movenext();
            string argname = parseIdent();
            if (string.IsNullOrEmpty(argname)) return null;
            ParameterInfo paramInfo = GetMethodParameterInfo(argname);// paramsDict[argname];
            if (paramInfo == null) throw new ZyyRTException("ZCode'" + Code + "'的参数'" + argname+"'找不到对应的ParameterInfo");
            if (this.zlmethod.HasGenericParameter(argTypeName))//.genericTypeDict.ContainsKey(argTypeName))
            {
                //var genericRealTypeName = genericTypeDict[argTypeName].Name;
                ZLParamInfo zp = new ZLParamInfo(paramInfo, this.zlmethod, paramIndex);//, true);
                paramIndex++;
                return zp;
            }
            else
            {
                var ztype = GetZTypeByTypeName(argTypeName);
                if (ztype != null)
                {
                    ZLParamInfo zp = new ZLParamInfo(paramInfo, this.zlmethod, paramIndex);//, false);
                    paramIndex++;
                    return zp;
                }
                
            }
            throw new ZyyRTException("没有导入'" + argTypeName + "'类型");
        }

        private bool IsGenericTypName(string typeName)
        {
            int id1 = typeName.IndexOf('<');
            int id2 = typeName.IndexOf('>');
            if (id1 != -1 && id2 != -1)
            {
                return true;
            }
            return false;
        }

        private string GetGenericTypNameShortName(string typeName)
        {
            int id1 = typeName.IndexOf('<');
            int id2 = typeName.IndexOf('>');
            return typeName.Substring(0,id1); ;
            //return typeName.Substring(id1 + 1, id2 - id1); ;
        }

        private ZType GetZTypeByTypeName(string typeName)
        {
            if(!IsGenericTypName(typeName))
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
            else
            {
                string shortClassName = GetGenericTypNameShortName(typeName);
                return GetZTypeByTypeName(shortClassName);
            }
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
