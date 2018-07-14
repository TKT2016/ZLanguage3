using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.AST.Exps;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;

namespace ZCompileCore.Parsers.Exps
{
    public class ChainItemFeaturer
    {
        ContextExp Context;
        ContextProc ProcContext;

        public ChainItemFeaturer(ContextExp context)
        {
            Context = context;
            ProcContext = context.ProcContext;
        }

        public string GetText(object Data)
        {
            if (!IsToken(Data)) return Data.ToString();
            else
            {
                LexToken tok = (LexToken)Data;
                string text = tok.Text;
                return text;
            }
        }

        private bool IsToken(object Data)
        {
             return Data is LexToken;
        }

        /// <summary>
        /// 是'的'
        /// </summary>
        public bool IsDe(object Data)
        {
            return IsKeyword(Data, "的");
        }

        private bool IsKeyword(object Data,string keytext)
        {
            //if (!IsToken(Data)) return false;
            if (!(Data is LexTokenText)) return false;
            LexTokenText token = Data as LexTokenText;
            if (token.Text != keytext) return false;
            return true;
        }

        /// <summary>
        /// 是'的'
        /// </summary>
        public bool IsNewfault(object Data)
        {
            return IsKeyword(Data, "新的");
        }

        /// <summary>
        /// 是'第'
        /// </summary>
        public bool IsDi(object Data)
        {
            return IsKeyword(Data, "第");
            //get
            //{
            //    //return IsToken && (Data is LexTokenText) && ((LexTokenText)Data).Kind == TokenKindKeyword.DI;
            //    if (!IsToken) return false;
            //    if (!(Data is LexTokenText)) return false;
            //    LexTokenText token = Data as LexTokenText;
            //    if (token.Text != "第") return false;
            //    return true;
            //}
        }

        /// <summary>
        /// 字面值
        /// </summary>
        public bool IsLiteral(object Data)
        {
            return (Data is LexTokenLiteral);
            //get
            //{
            //    return IsToken && (Data is LexTokenLiteral);
            //}
        }

        /// <summary>
        /// 局部变量
        /// </summary>
        public bool IsLocalVar(object Data)
        {
            if (!IsToken(Data)) return false;
            var text = this.GetText(Data);
            return this.ProcContext.LocalManager.IsDefLocal(text);
            //get
            //{
            //    return IsToken && (this.ProcContext.IsDefLocal(Text));
            //}
        }

        /// <summary>
        /// 参数
        /// </summary>
        public bool IsParameter(object Data)
        {
            if (!IsToken(Data)) return false;
            var text = this.GetText(Data);
            return this.ProcContext.HasParameter(text);
            //get
            //{
            //    return IsToken && (this.ProcContext.HasParameter(Text));
            //}
        }

        /// <summary>
        /// 是定义的属性
        /// </summary>
        public bool IsThisProperty(object Data)
        {
            if (!IsToken(Data)) return false;
            var text = this.GetText(Data);
            bool b= this.ProcContext.IsThisProperty(text);
            return b;
        }

        /// <summary>
        /// 是定义的字段
        /// </summary>
        public bool IsThisField(object Data)
        {
            if (!IsToken(Data)) return false;
            var text = this.GetText(Data);
            return this.ProcContext.IsThisField(text);
            //get
            //{
            //    return IsToken && (this.ProcContext.IsThisField(Text));
            //}
        }

        /// <summary>
        /// 是父类的属性
        /// </summary>
        public bool IsSuperProperty(object Data)
        {
            if (!IsToken(Data)) return false;
            var text = this.GetText(Data);
            return this.ProcContext.IsSuperProperty(text);
            //get
            //{
            //    return IsToken && (this.ProcContext.IsSuperProperty(Text));
            //}
        }

        /// <summary>
        /// 是父类的字段
        /// </summary>
        public bool IsSuperField(object Data)
        {
            if (!IsToken(Data)) return false;
            var text = this.GetText(Data);
            return this.ProcContext.IsSuperField(text);
            //get
            //{
            //    return IsToken && (this.ProcContext.IsSuperField(Text));
            //}
        }

        /// <summary>
        /// 是使用的枚举值
        /// </summary>
        public bool IsUsedEnumItem(object Data)
        {
           if (!IsToken(Data)) return false;
            var text = this.GetText(Data);
            return this.ProcContext.IsUsedEnumItem(text);

            //get
            //{
            //    return IsToken && (this.ProcContext.IsUsedEnumItem(Text));
            //}
        }

        /// <summary>
        /// 是使用的字段
        /// </summary>
        public bool IsUsedField(object Data)
        {
            if (!IsToken(Data)) return false;
            var text = this.GetText(Data);
            return this.ProcContext.IsUsedField(text);

            //get
            //{
            //    return IsToken && (this.ProcContext.IsUsedField(Text));
            //}
        }

        /// <summary>
        /// 是使用的属性
        /// </summary>
        public bool IsUsedProperty(object Data)
        {
            if (!IsToken(Data)) return false;
            var text = this.GetText(Data);
            return this.ProcContext.IsUsedProperty(text);
            //get
            //{
            //    return IsToken && (this.ProcContext.IsUsedProperty(Text));
            //}
        }

        /// <summary>
        /// 是当前类的类名
        /// </summary>
        public bool IsThisClassName(object Data)
        {
            if (!IsToken(Data)) return false;
            var text = this.GetText(Data);
            return this.ProcContext.IsCompilingClassName(text);
            //get
            //{
            //    return IsToken && (this.ProcContext.IsCompilingClassName(Text));
            //}
        }

        /// <summary>
        /// 是导入的类型名称
        /// </summary>
        public bool IsImportTypeName(object Data)
        {
            if (!IsToken(Data)) return false;
            var text = this.GetText(Data);
            return this.ProcContext.IsImportClassName(text);
            //get
            //{
            //    return IsToken && (this.ProcContext.IsImportClassName(Text));
            //}
        }


        public bool IsIdent(object Data)
        {
            if (!(Data is LexTokenText)) return false;
            //LexTokenText token = Data as LexTokenText;
            //if (token.Kind != TokenKindLiteral.LiteralString) return false;
            return true;
            //if (!IsToken(Data)) return false;
            //if (IsDe(Data)) return false;
            //if (IsDi(Data)) return false;
            //if (IsLiteral(Data)) return false;
            //if (IsNewfault(Data)) return false;

            //var text = this.GetText(Data);
            //return this.ProcContext.IsImportClassName(text);
            //get
            //{
            //    return IsToken && !(IsDi || IsDe || IsLiteral || IsNewfault);
            //}
        }

        /// <summary>
        /// 是字符串
        /// </summary>
        public bool IsString(object Data)
        {
            if(!(Data is LexTokenLiteral)) return false;
            LexTokenLiteral token = Data as LexTokenLiteral;
            if (token.Kind != TokenKindLiteral.LiteralString ) return false;
            return true;
            //if (!IsToken(Data)) return false;
            //if (IsDe(Data)) return false;
            //if (IsDi(Data)) return false;
            //if (IsLiteral(Data)) return false;
            //if (IsNewfault(Data)) return false;
            
            //var text = this.GetText(Data);
            //return this.ProcContext.IsImportClassName(text);
            //get
            //{
            //    return IsToken && !(IsDi || IsDe || IsLiteral || IsNewfault);
            //}
        }

        /// <summary>
        /// 是表达式
        /// </summary>
        public bool IsExp(object Data)
        {
            return (Data is Exp);
            //get
            //{
            //    if (IsNone) return false;
            //    if (Data == null) return false;
            //    return Data is Exp;
            //}
        }

        public bool IsProcNamePart(string text)
        {
            bool b = false;

            b = this.ProcContext.ClassContext.FileContext.ImportUseContext.ContextFileManager.IsCompilingMehtodNamePart(text);
            if (b) { return true; }

            b = this.ProcContext.ClassContext.FileContext.ProjectContext.CompiledTypes.IsProcNamePart(text);
            if (b) { return true; }

            b= this.ProcContext.ClassContext.FileContext.ImportUseContext.ContextFileManager.IsProcNamePart(text);
            if (b) { return true; }

            return false;
        }
    }
}
