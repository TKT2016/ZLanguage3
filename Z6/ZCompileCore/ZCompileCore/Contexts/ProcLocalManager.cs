using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;

namespace ZCompileCore.Contexts
{
    public class ProcLocalManager
    {
        private ContextProc Context;
        public List<ZCLocalVar> LocalVarList = new List<ZCLocalVar>();
        public ProcLocalManager(ContextProc proc)
        {
            Context = proc;
        }

        public int Add(ZCLocalVar localVar)
        {
            LocalVarList.Add(localVar);
            int index = LocalVarList.Count;
            localVar.LoacalVarIndex = index;
            return index;
        }

        public void BuildVar(ILGenerator IL)
        {
            foreach (ZCLocalVar localVar in LocalVarList)
            {
                if (!localVar.IsReplaceToNestedFiled)
                {
                    localVar.VarBuilder = IL.DeclareLocal(ZTypeUtil.GetTypeOrBuilder(localVar.GetZType()));
                    localVar.VarBuilder.SetLocalSymInfo(localVar.ZName);
                }
            }
        }

        /// <summary>
        /// 从指定名称开始局部变量序号都自减1 (Lambda表达式使用)
        /// </summary>
        public void DecLocalIndex(string startName)
        {
            bool isDec = false;
            foreach (var item in LocalVarList)
            {
                if (item.ZName == startName)
                {
                    isDec = true;
                }
                if(isDec)
                {
                    item.LoacalVarIndex--;
                }
            }
        }

       
        public bool IsDefLocal(string name)
        {
            //return localDefDict.ContainsKey(name);
            foreach(var item in LocalVarList)
            {
                if(item.ZName ==name)
                {
                    return true;
                }
            }
            return false;
        }

        public ZCLocalVar GetDefLocal(string name)
        {
            //return localDefDict[name];
            foreach (var item in LocalVarList)
            {
                if (item.ZName == name)
                {
                    return item;
                }
            }
            return null;
        }

        //public int Add(string varName)
        //{
        //    LoacalVarList.Add(varName);
        //    int index =  LoacalVarList.Count;
        //    return index;
        //}
    }
}
