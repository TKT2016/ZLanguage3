using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileDesc.Words
{
    public class WordData
    {
        public WordKind WKind { get; set; }
        public WordInfo WInfo { get; set; }
        public Object _Data;
        public Object Data
        {
            get
            {
                if (GetDataFunc != null)
                {
                    var data = GetDataFunc(this.WInfo.Text,this.WKind);
                    return data;
                }
                else
                {
                    return _Data;
                }
            }
            set { _Data = value; }
        }
        public Func<string, WordKind, object> GetDataFunc { get; set; }

        public WordData(WordInfo wordInfo,WordKind kind, Object data)
        {
            WInfo = wordInfo;
            WKind = kind;
            Data = data;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}",WKind,Data);
        }
    }
}
