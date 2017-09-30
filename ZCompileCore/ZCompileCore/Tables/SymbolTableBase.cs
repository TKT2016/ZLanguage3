using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Symbols;
using ZLangRT;

namespace ZCompileCore.Symbols
{
    public abstract class SymbolTableBase : ISymbolTable
    {
        public virtual ISymbolTable ParentTable { get;set;}
        public virtual string TableName { get;protected set;}
        public abstract bool CurrentContains(string symbolName);
        public abstract SymbolBase CurrentGet(string symbolName);

        public virtual ISymbolTable Push(ISymbolTable iSymbolTable)
        {
            iSymbolTable.ParentTable = this;
            return iSymbolTable;
        }

        public virtual bool Contains(string symbolName)
        {
            if (this.CurrentContains(symbolName))
            {
                return true;
            }
            else if (ParentTable != null)
            {
                return this.ParentTable.Contains(symbolName);
            }
            return false;
        }
        
        public virtual SymbolBase Get(string symbolName)
        {
            if (this.CurrentContains(symbolName))
            {
                return CurrentGet(symbolName);
            }
            else if (this.ParentTable != null)
            {
                return this.ParentTable.Get(symbolName);
            }
            return null;
        }

        public virtual ISymbolTable Pop()
        {
            return  this.ParentTable;
        }

        public override string ToString()
        {
            return this.GetType().Name + "-" + TableName;
        }
     
    }
}
