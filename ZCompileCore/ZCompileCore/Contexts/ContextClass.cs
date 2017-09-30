using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileKit.Collections;
using ZCompileCore.Symbols;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;
using ZCompileDesc.ZTypes;
using ZCompileDesc.ZMembers;

namespace ZCompileCore.Contexts
{
    public class ContextClass : IWordDictionary
    {
        public ContextFile FileContext { get; set; }

        public PropertyContextCollection PropertyContext { get; set; }
        public ProcContextCollection ProcManagerContext { get; set; }

        public string ClassName { get; set; }
        public string ExtendsName { get; set; }
       
        
        public ZClassType BaseZType { get; set; }
        public bool IsStaticClass { get; set; }
        public ClassEmitContext EmitContext { get; set; }
        
        public MethodBuilder InitPropertyMethod { get; set; }

        public ClassSymbolTable Symbols { get{return CurrentTable;} }


        public SuperSymbolTable SuperTable { get; private set; }
        public ClassSymbolTable CurrentTable { get; private set; }

        public SymbolDefField NestedOutFieldSymbol { get; set; }
        public string ContextKey { get { return FileContext.ContextKey + "." + (ClassName ?? ""); } }
        public ContextClass(ContextFile fileContext)
        {
            FileContext = fileContext;

            PropertyContext = new PropertyContextCollection();
            PropertyContext.ClassContext = this;

            ProcManagerContext = new ProcContextCollection();
            ProcManagerContext.ClassContext = this;
            EmitContext = new ClassEmitContext();

            //MemberDictionary = new NameDictionary<SymbolDefMember>();
            CurrentTable = new ClassSymbolTable("Class");
        }

        #region IWordDictionary实现
        public bool ContainsWord(string text)
        {
            return ClassName==text
                || PropertyContext.ContainsWord(text)
                || ProcManagerContext.ContainsWord(text)
            ;
        }

        public WordInfo SearchWord(string text)
        {
            WordInfo info1 = null;
            if (ClassName == text)
                info1 = new WordInfo(text, WordKind.TypeName);
            WordInfo info2 = PropertyContext.SearchWord(text);
            WordInfo info3 = ProcManagerContext.SearchWord(text);
            WordInfo newWord = WordInfo.Merge(info1, info1, info3);
            return newWord;
        }
        #endregion

        public void AddMember(SymbolDefProperty symbol)
        {
            Symbols.Add(symbol);
            //MemberDictionary.Add(symbol);
        }

        public void SetSuperTable(SuperSymbolTable superTable)
        {
            SuperTable = superTable;
            CurrentTable.ParentTable = SuperTable;
        }

        //public SymbolDefMember FindMember(string name)
        //{
        //    if(MemberDictionary.ContainsKey(name))
        //    {
        //        SymbolDefMember member = MemberDictionary.Get(name);
        //        return member;
        //    }
        //    if (IsStaticClass) return null;
        //    ZMemberInfo zmember = BaseZType.SearchZMember(name);
        //    if (zmember == null) return null;
        //    SymbolDefMember symbol = SymbolDefMember.Create(name,zmember);
        //    return symbol;
        //}

        public ZMethodDesc[] SearchThisProc(ZCallDesc procDesc)
        {
            return ProcManagerContext.SearchProc(procDesc);
        }

        public class ClassEmitContext
        {
            public TypeBuilder ClassBuilder { get; set; }
            public ConstructorBuilder ZeroConstructor { get;  set; }
            public MethodBuilder InitMemberValueMethod { get;  set; }
            public ISymbolDocumentWriter IDoc { get;  set; }
        }

        //IWordDictionaryList _WordCollection;
        //public IWordDictionary ClassWordDictionary
        //{
        //    get
        //    {
        //        if (_WordCollection == null)
        //        {
        //            _WordCollection = new IWordDictionaryList();
        //            _WordCollection.Add(this.FileContext.ImportContext.ImportPackageDescList);
        //            _WordCollection.Add(this.PropertyContext.Dict);
        //            //_WordCollection.AddRange(this.ProcManagerContext.GetWordCollection());
        //        }
        //        return _WordCollection;
        //    }
        //}
    }
}
