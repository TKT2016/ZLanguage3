using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileCore.Lex
{
    public enum TokenKind
    {
        Assign,//=
        AssignTo,//=>
        LBS,//(
        RBS,//)
        //LBM,//[
        //RBM,//]
        //LBB,//{
        //RBB,//}
        //GoesTo,// ->
        //Dot,//.

        /// <summary>
        /// 逗号 , 
        /// </summary>
        Comma,//,

        /// <summary>
        /// 分号 ;
        /// </summary>
        Semi,//;
   
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
       
        //Function,
        //Var,
        DE,//的
        DI,//第
        IF,//如果
        ELSE,//否则
        ELSEIF,//否则如果
        //Switch,
        //Case,
        //For,
        //Foreach,
        While,//循环当
        //Foreach,//循环每一个
        Catch,//捕捉
        Each,//每一个
        //Break,
        //Continue,
        //Return,
        True,
        False,
        //New,
        //Catch,
        AND,//并且
        OR,//或者
        NOT,//!
        //Load,
        //Using,

        NULL,
        LiteralInt,
        LiteralFloat,
        //LiteralBool,
        //LiteralChar,
        LiteralString ,

        //Function,
        //ToolClass,
        //Main,//启动
        //RightAssign,

        //INT,//整数
        //FLOAT,//浮点数
        //Bool,
        //Char,
        //String,//文本
        //Object,//事物
        Ident,
        Caption,//说明
        //IdentStmt,
       
        //Start,

        /// <summary>
        /// 冒号
        /// </summary>
        Colon,//:
        //Colond,//::双冒号
        //LibType,
        //LibMember,
        //LibMethod,
        Unknow,
        Error,

        EOF,
        NewLine,
    }
}
