using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Symbols;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;

namespace ZCompileCore.Contexts
{
    public class ProcContextCollection : IWordDictionary
    {
        static int ProcContextCollection_Index = 0;

        public ContextClass ClassContext { get; set; }
        private List<ContextProc> ProcContextList;// { get; private set; }
        private string Key;

        public ProcContextCollection()
        {
            ProcContextList = new List<ContextProc>();
            Key = "ProcContextCollection_" + ProcContextCollection_Index;
            ProcContextCollection_Index++;
        }

        #region IWordDictionary实现
        public bool ContainsWord(string text)
        {
            foreach(var item in ProcContextList)
            {
                if (item.ContainsWord(text))
                    return true;
            }
            return false;
        }

        public WordInfo SearchWord(string text)
        {
            List<WordInfo> wordList = new List<WordInfo>();
            foreach (var item in ProcContextList)
            {
                WordInfo word = item.SearchWord(text);
                if(word!=null)
                {
                    wordList.Add(word);
                }
            }
            WordInfo newWord = WordInfo.Merge(wordList.ToArray());
            return newWord;
        }
        #endregion

        //List<IWordDictionary> _WordCollection;
        //public List<IWordDictionary> GetWordCollection()
        //{
        //    if (_WordCollection == null)
        //    {
        //        _WordCollection = new IWordDictionaryList();
        //        foreach (var item in ProcContextList)
        //        {
        //            _WordCollection.Add(item.ProcNameWordDictionary);
        //        }
        //    }
        //    return _WordCollection;
        //}

        public void AddContext(ContextProc procContext)
        {
            ProcContextList.Add(procContext);
        }

        public ZMethodDesc[] SearchProc(ZCallDesc procDesc)
        {
            List<ZMethodDesc> data = new List<ZMethodDesc>();
            foreach (var context in ProcContextList)
            {
                if (!context.IsConstructor && context.ProcDesc.ZEquals(procDesc))
                {
                    data.Add(context.ProcDesc);
                }
            }
            return data.ToArray();
        }
    }
}
