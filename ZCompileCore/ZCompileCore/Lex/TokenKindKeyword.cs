using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileCore.Lex
{
    public enum TokenKindKeyword
    {
        DE,//的
        DI,//第
        IF,//如果
        ELSE,//否则
        ELSEIF,//否则如果
        //Switch,
        //Case,
        //For,
        //While,//循环当
        Repeat,//重复
        Dang,//当
        Times,//次
        Loop,//循环
        Catch,//捕捉
        Each,//每一个
        //Break,
        //Continue,
        //Return,
        True,//是
        False,//否
        //New,
        //Catch,
        //AND,//并且
        //OR,//或者
        ////NOT,//!
        //Load,
        //Using,
        Ident,
        Caption,//说明
        Enum,//约定
        NewDefault , //新的
        Ge//个
    }
}
