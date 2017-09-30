using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileDesc.Words
{
    public class WordInfo
    {
        public string Text { get; set; }
        public List<WordData> WDataList { get; set; }

        public WordInfo(string text)
        {
            Text = text;
            WDataList = new List<WordData>();
        }

        public WordInfo(string text, WordKind kind):
            this(text,kind,null)
        {
          
        }

        public WordInfo(string text, WordKind kind,object data)
        {
            this.Text = text;
            WDataList = new List<WordData>();
            WordData wdata = new WordData(this,kind,data);
            WDataList.Add(wdata);
        }

        public WordInfo(string text, WordKind kind, Func<string, WordKind, object> getDataFunc)
        {
            this.Text = text;
            WDataList = new List<WordData>();
            WordData wdata = new WordData(this, kind, null);
            wdata.GetDataFunc = getDataFunc;
            WDataList.Add(wdata);
        }

        public WordInfo(string text, List<WordData> datas)
        {
            this.Text = text;
            WDataList = datas;
        }

        public bool HasKind(WordKind kind)
        {
            foreach(WordData wd in this.WDataList)
            {
                if(wd.WKind==kind)
                {
                    return true;
                }
            }
            return false;
        }

        public WordKind FirstWordKind
        {
            get
            {
                return WDataList[0].WKind;
            }
        }

        public static WordInfo Merge(params WordInfo[] words)
        {
            if (words.Length==0) return null;
            string text = null;
            List<WordData> datas3 = new List<WordData>();
            foreach(var item in words)
            {
                if (item != null)
                {
                    datas3.AddRange(item.WDataList);
                    if(text==null)
                    {
                        text = item.Text;
                    }
                }
            }
            List<WordData> newData = datas3.Distinct().ToList();
            if (datas3.Count == 0) return null;
            WordInfo word3 = new WordInfo(text, datas3);
            return word3;
        }

        public override string ToString()
        {
            string dataText = string.Join("," , WDataList.Select(p=>p.ToString()));
            return string.Format("{0}-{1}-[{2}]", Text, dataText);
        }
    }
}
