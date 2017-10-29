using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileKit.Collections;
using ZCompileCore.Symbols;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;
using ZCompileDesc.ZTypes;

namespace ZCompileCore.Contexts
{
    public class ContextProc : IWordDictionary
    {
        public ContextClass ClassContext { get; private set; }
        public ProcContextCollection ProcManagerContext { get; set; }
        public bool IsConstructor { get; set; }
        public ZMethodDesc ProcDesc { get; set; }
        public ZType RetZType { get; set; }
        public ProcSymbolTable Symbols { get; set; }
        public string ProcName { get; set; }

        public WordDictionary ProcNameWordDictionary { get;private set; }
        public WordDictionary ProcVarWordDictionary { get; private set; }
        public WordDictionary LoacalVarWordDictionary { get; private set; }

        private static int ProcIndex = 0;
        public string ContextKey { get { return ClassContext.ContextKey + "." + (ProcName ?? ProcIndex.ToString()); } }

        public ContextProc(ContextClass classContext)
        {
            this.ClassContext = classContext;
            ProcIndex++;
            EmitContext = new ProcEmitContext(ContextKey + ".EmitContext");
            Symbols = new ProcSymbolTable(ContextKey + ".符号表", classContext.Symbols);
            ProcNameWordDictionary = new WordDictionary(ContextKey + ".代码过程字典");
            ProcVarWordDictionary = new WordDictionary(ContextKey + ".代码变量字典");
            LoacalVarWordDictionary = new WordDictionary(ContextKey + ".局部变量字典");
        }

        #region IWordDictionary实现
        public bool ContainsWord(string text)
        {
            return ProcNameWordDictionary.ContainsWord(text)
                //|| ProcVarWordDictionary.ContainsWord(text)
            ;
        }

        public WordInfo SearchWord(string text)
        {
            WordInfo word1 = ProcNameWordDictionary.SearchWord(text);
           // WordInfo word2 = ProcVarWordDictionary.SearchWord(text);
            //WordInfo newWord = WordInfo.Merge(word1, word2);
            return word1;
        }

        #endregion

        public bool ContainsVar(string text)
        {
            return ProcVarWordDictionary.ContainsWord(text)
                || LoacalVarWordDictionary.ContainsWord(text);
            //|| ProcVarWordDictionary.ContainsWord(text)
            ;
        }

        public WordInfo SearchVar(string text)
        {
            WordInfo word1 = ProcVarWordDictionary.SearchWord(text);
            WordInfo word2 = LoacalVarWordDictionary.SearchWord(text);
            WordInfo newWord = WordInfo.Merge(word1, word2);
            return word1;
        }

        #region create index :localvar ,arg, each
        int LoacalVarIndex = -1;
        public List<string> LoacalVarList = new List<string>();
        public int CreateLocalVarIndex(string name)
        {
            LoacalVarIndex++;
            LoacalVarList.Add(name);
            return LoacalVarIndex;
        }

        int ArgIndex = -1;
        public List<string> ArgList = new List<string>();
        public int CreateArgIndex(string name)
        {
            if (ArgIndex == -1)
            {
                if(IsStatic)
                {
                    ArgIndex = 0;
                }
                else
                {
                    ArgIndex = 1;
                }
                ArgList.Add(name);
            }
            else
            {
                ArgIndex++;
                ArgList.Add(name);
            }
            return ArgIndex;
        }

        int EachIndex = -1;
        //public List<string> ArgList = new List<string>();
        public int CreateEachIndex( )
        {
            EachIndex ++;
            //ArgList.Add(name);
            return EachIndex;
        }

        int RepeatIndex = -1;
        public int CreateRepeatIndex()
        {
            RepeatIndex++;
            return RepeatIndex;
        }

        #endregion

        public ProcEmitContext EmitContext { get; set; }
        public bool IsStatic
        {
            get
            {
                return this.ClassContext.IsStaticClass;
            }
        }

        private int NestedIndex = 0;
        public string CreateNestedClassName()
        {
            NestedIndex++;
            return (ProcName ?? "") + "Nested" + NestedIndex;
        }

        public class ProcEmitContext
        {
            public MethodBuilder CurrentMethodBuilder { get; private set; }
            public ConstructorBuilder CurrentConstructorBuilder { get; private set; }
            public string ContextKey { get; private set; }

            public ProcEmitContext(string key)
            {
                ContextKey = key;
            }

            public void SetBuilder(MethodBuilder methodBuilder)
            {
                CurrentMethodBuilder = methodBuilder;
            }

            public void SetBuilder(ConstructorBuilder constructorBuilder)
            {
                CurrentConstructorBuilder = constructorBuilder;
            }

            public ILGenerator ILout { get; set; }
        }
    }
}
