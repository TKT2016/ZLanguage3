using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.ASTExps;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;

namespace ZCompileCore.Parsers
{
    public class ChainItemFeaturer
    {
        ContextExp Context;
        ContextProc ProcContext;
        public object Data { get; private set; }

        public ChainItemFeaturer(ContextExp context,object item)
        {
            Context = context;
            ProcContext = context.ProcContext;
            this.Data = item;
            if(Data==null)
            {
                IsNone = true;
            }
        }

        public ChainItemFeaturer()
        {
            IsNone = true;
        }

        public bool IsNone { get; private set; }

        public string Text
        {
            get
            {
                if (!IsToken) return Data.ToString();
                else
                {
                    LexToken tok = (LexToken)Data;
                    string text = tok.GetText();
                    return text;
                }
            }
        }

        private bool IsToken
        {
            get
            {
                if (IsNone) return false;
                if (Data==null) return false;
                return Data is LexToken;
            }
        }

        /// <summary>
        /// 是'的'
        /// </summary>
        public bool IsDe
        {
            get
            {
                return IsToken && ((LexToken)Data).Kind == TokenKind.DE;
            }
        }

        /// <summary>
        /// 是'第'
        /// </summary>
        public bool IsDi
        {
            get
            {
                return IsToken && ((LexToken)Data).Kind == TokenKind.DI;
            }
        }

        /// <summary>
        /// 字面值
        /// </summary>
        public bool IsLiteral
        {
            get
            {
                return IsToken && ((LexToken)Data).IsLiteral;
            }
        }

        /// <summary>
        /// 局部变量
        /// </summary>
        public bool IsLocalVar
        {
            get
            {
                return IsToken && (this.ProcContext.IsDefLocal(Text));
            }
        }

        /// <summary>
        /// 参数
        /// </summary>
        public bool IsParameter
        {
            get
            {
                return IsToken && (this.ProcContext.HasParameter(Text));
            }
        }

        /// <summary>
        /// 是定义的属性
        /// </summary>
        public bool IsThisProperty
        {
            get
            {
                return IsToken && (this.ProcContext.IsThisProperty(Text));
            }
        }

        /// <summary>
        /// 是定义的字段
        /// </summary>
        public bool IsThisField
        {
            get
            {
                return IsToken && (this.ProcContext.IsThisField(Text));
            }
        }

        /// <summary>
        /// 是父类的属性
        /// </summary>
        public bool IsSuperProperty
        {
            get
            {
                return IsToken && (this.ProcContext.IsSuperProperty(Text));
            }
        }

        /// <summary>
        /// 是父类的字段
        /// </summary>
        public bool IsSuperField
        {
            get
            {
                return IsToken && (this.ProcContext.IsSuperField(Text));
            }
        }

        /// <summary>
        /// 是使用的枚举值
        /// </summary>
        public bool IsUsedEnumItem
        {
            get
            {
                return IsToken && (this.ProcContext.IsUsedEnumItem(Text));
            }
        }

        /// <summary>
        /// 是使用的字段
        /// </summary>
        public bool IsUsedField
        {
            get
            {
                return IsToken && (this.ProcContext.IsUsedField(Text));
            }
        }

        /// <summary>
        /// 是使用的属性
        /// </summary>
        public bool IsUsedProperty
        {
            get
            {
                return IsToken && (this.ProcContext.IsUsedProperty(Text));
            }
        }

        /// <summary>
        /// 是当前类的类名
        /// </summary>
        public bool IsThisClassName
        {
            get
            {
                return IsToken && (this.ProcContext.IsCompilingClassName(Text));
            }
        }

        /// <summary>
        /// 是导入的类型名称
        /// </summary>
        public bool IsImportTypeName
        {
            get
            {
                return IsToken && (this.ProcContext.IsImportClassName(Text));
            }
        }

        /// <summary>
        /// 是字符串
        /// </summary>
        public bool IsText
        {
            get
            {
                return IsToken && !(IsDi || IsDe||IsLiteral);
            }
        }

        /// <summary>
        /// 是表达式
        /// </summary>
        public bool IsExp
        {
            get
            {
                if (IsNone) return false;
                if (Data == null) return false;
                return Data is Exp;
            }
        }

        public override string ToString()
        {
            if (IsNone) return "(none)";
            else return Data.ToString();
        }
    }
}
