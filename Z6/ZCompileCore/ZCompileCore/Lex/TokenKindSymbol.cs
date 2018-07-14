using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileCore.Lex
{
    public enum TokenKindSymbol
    {
        Assign,//=
        AssignTo,//=>

        /// <summary>
        /// 左小括号
        /// </summary>
        LBS,//(

        /// <summary>
        /// 右小括号
        /// </summary>
        RBS,//)
        //LBM,//[
        //RBM,//]
        //LBB,//{
        //RBB,//}
        //GoesTo,// ->
        //Dot,//.
        ADD,//+
        SUB,//-
        MUL,//*
        DIV,///
        //Mod,//%
        EQ,//==
        NE,//!=
        GT,//>
        GE,//=
        LT,//<
        LE,//<=
        //Inc,//++
        //Dec,//--
        /// <summary>
        /// 冒号
        /// </summary>
        Colon,//:
        //Colond,//::双冒号
        /// <summary>
        /// 逗号 , 
        /// </summary>
        Comma,//,

        /// <summary>
        /// 分号 ;
        /// </summary>
        Semi,//;
        Unknow,
        Error,

        EOF,
        //NewLine,
        AND,//并且
        OR,//或者
        //NOT,//!
    }
}
