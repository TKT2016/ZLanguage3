using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.AST;
using ZCompileCore.Symbols;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using Z语言系统;

namespace ZCompileCore.Contexts
{
    public class ContextExp //:IWordDictionary
    {
        public Stmt Stmt { get;private set; }
        public ContextProc ProcContext { get; private set; }
        //public SymbolTable Table { get; set; }

        public ContextExp(ContextProc procContext)
        {
            ProcContext = procContext;
        }

        public ContextExp(ContextProc procContext,Stmt stmt)
        {
            ProcContext = procContext;
            Stmt = stmt;
        }

        public ContextClass ClassContext
        {
            get
            {
                return this.ProcContext.ClassContext;
            }
        }

        public ContextFile FileContext
        {
            get
            {
                return this.ClassContext.FileContext;
            }
        }

        //#region IWordDictionary实现
        //public bool ContainsWord(string text)
        //{
        //    return ProcContext.ContainsWord(text)
        //    ;
        //}

        //public WordInfo SearchWord(string text)
        //{
        //    return ProcContext.SearchWord(text);
        //}
        //#endregion

        //public IWordDictionary ExpWordDictionary
        //{
        //    get{
        //        return this.ProcContext.ClassWordDictionary;
        //    }
        //}
        //{
        //    return this.ProcContext.GetWordCollection();
            //if(ExpWordCollection==null)
            //{
            //    //ExpWordCollection = new WordCollection();
            //    //ExpWordCollection.Add(this.ProcContext.ClassContext.FileContext.GetWordCollection().ToArray());
            //    //ExpWordCollection.Add(this.ProcContext.ClassContext.FileContext.ImportContext.DictMember);

            //    //ExpWordCollection.Add(this.ProcContext.ProcWordDictionary);
            //    ////ExpWordManager.Add(this.ProcContext.ClassContext.ClassWordDictionary);
            //    //var WordManager2 = this.ProcContext.ClassContext.FileContext.GetWordCollection();
            //    //ExpWordCollection.Add(WordManager2.GetDictionaryArray());
            //}
            //return ExpWordCollection;
       // }
    }
}
