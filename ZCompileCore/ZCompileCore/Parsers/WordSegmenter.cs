using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Lex;
using ZCompileKit.Tools;
using ZCompileDesc.Collections;
using ZCompileDesc.Words;
using ZCompileKit;

namespace ZCompileCore.Parser
{
   public class WordSegmenter
    {
       IWordDictionary Tree;

       public WordSegmenter(IWordDictionary tree)
       {
           Tree = tree;
       }

       public Token[] Split(Token token)
       {
           string src = token.GetText();
           WordInfo[] words = Split(src);
           List<Token> list = new List<Token>();
           int col = token.Col;
           int line = token.Line;
           foreach(WordInfo word in words)
           {
               string text = word.Text;
               Token tok = null;
               if(StringHelper.IsInt(text))
               {
                   tok = new Token() { Line = line, Col = col, Kind = TokenKind. LiteralInt , WKind = word.FirstWordKind, Text = word.Text };
               }
               else if (StringHelper.IsFloat(text))
               {
                   tok = new Token() { Line = line, Col = col, Kind = TokenKind.LiteralFloat, WKind = word.FirstWordKind, Text = word.Text };
               }
               else
               {
                   tok = new Token() { Line = line, Col = col, Kind = token.Kind, WKind = word.FirstWordKind, Text = word.Text };
               }
               list.Add(tok);
               col += tok.GetText().Length;
           }
           return list.ToArray();
       }

       private WordInfo[] Split(string src)
       {
           if (string.IsNullOrEmpty(src)) throw new CompileCoreException("Split的参数没有内容");
           //if (src == "异常信息")
           //{
           //    Console.WriteLine("WordSegmenter "+src);
           //}
           string src2 = src;
           List<WordInfo> list = new List<WordInfo>();
           while(true)
           {
               SplitResult result = SplitOne(src2);
               //Console.WriteLine(result.ToString());
               list.Add(result.ResultWord);
               src2 = result.UnSplitText;
               if (src2 == "")
               {
                   break;
               }
           }
           list.Reverse();
           WordInfo[] mergedArray = MergeUnkown(list);
           return mergedArray;
       }

       private WordInfo[] MergeUnkown(List<WordInfo> list)
       {
           ContinuousMerger<WordInfo> meger = new ContinuousMerger<WordInfo>();
           meger.Source = list.ToArray();
           meger.InStateFunc = (WordInfo info) => info.FirstWordKind == WordKind.Unkown;
           meger.MergeObjsFunc = MergeObjs;
           return meger.Merge();
       }

       private WordInfo MergeObjs(WordInfo[] arr)
       {
           string newText = string.Join( "",arr.Select(p => p.Text));
           WordInfo wordInfo = new WordInfo(newText, WordKind.Unkown);
           return wordInfo;
       }

       private SplitResult SplitOne(string src)
       {
           //Console.WriteLine("WordSegmenter.SplitOne: "+src);
           if (src.Length == 1)
           {
               WordInfo result = GetFromTree(src);
               if (result == null)
               {
                   result = new WordInfo(src, WordKind.Unkown);
               }
               SplitResult splitResult = new SplitResult();
               splitResult.UnSplitText = "";
               splitResult.ResultWord = result;
               return splitResult;
           }
           else
           {
               SplitResult splitResult = null;
               WordInfo result = null;
               int i = 0;
               for (; i < src.Length; i++)
               {
                   string newsrc = src.Substring(i);
                   //Console.WriteLine("WordSegmenter.SplitOne: " + newsrc);
                   result = GetFromTree(newsrc);
                   if (result != null)
                   {
                       splitResult = new SplitResult();
                       splitResult.UnSplitText = src.Substring(0, i);
                       splitResult.ResultWord = result;
                       return splitResult;
                   }
               }
               if (splitResult == null)
               {
                   splitResult = new SplitResult();
                   splitResult.UnSplitText = src.Substring(0,src.Length - 1);
                   WordInfo info = new WordInfo(src.Substring(src.Length - 1), WordKind.Unkown);
                   splitResult.ResultWord = info;
               }
               return splitResult;
           }
       }

       private WordInfo GetFromTree(string src)
       {
           //if(src=="控制台")
           //{
           //    Console.WriteLine("控制台");
           //}
           WordInfo info = Tree.SearchWord(src);
           return info;
       }

       private WordInfo SplitOneChar(string src)
       {
           //string src = tok.GetText();
           if (src.Length != 1) throw new CompileCoreException("SplitOneChar的内容'" + src + "'长度不为1");

           WordInfo word = GetFromTree (src);
           if(word!=null)
           {
               return word;
           }
           else
           {
               WordInfo info2 = new WordInfo(src, WordKind.Unkown);
               return info2;
           }
       }

      class SplitResult
      {
          public string UnSplitText { get; set; }
          public WordInfo ResultWord { get; set; }

          public override string ToString()
          {
              return string.Format("{0}|{1}", UnSplitText, ResultWord.Text);
          }
      }

    }
}
