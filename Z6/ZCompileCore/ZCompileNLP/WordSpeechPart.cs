using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileNLP
{
    /// <summary>
    /// 词性
    /// </summary>
    public enum WordSpeechPart
    {
        /// <summary>
        /// 不确定
        /// </summary>
        none,
        /// <summary>
        /// 名词
        /// </summary>
        noun,

        /// <summary>
        /// 动词
        /// </summary>
        verb,

        /// <summary>
        /// 形容词
        /// </summary>
        adjective,

        /// <summary>
        /// 副词
        /// </summary>
        adverb,

        /// <summary>
        /// 量词
        /// </summary>
        classifier,

        /// <summary>
        ///  介词
        /// </summary>
        preposition
    }
}
