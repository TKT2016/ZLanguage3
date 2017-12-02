using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZNLP
{
    /// <summary>
    /// 词编译时角色
    /// </summary>
    public enum WordCompilePart
    {
        /// <summary>
        /// (不确定)
        /// </summary>
        none,

        /// <summary>
        /// 的
        /// </summary>
        de,

        /// <summary>
        /// 第
        /// </summary>
        di,
        /// <summary>
        /// 表达式
        /// </summary>
        exp,

        /// <summary>
        /// 值
        /// </summary>
        literal,

        /// <summary>
        /// 变量
        /// </summary>
        localvar,

        /// <summary>
        /// 参数
        /// </summary>
        arg,

        /// <summary>
        /// 属性(this)
        /// </summary>
        property_this,

        /// <summary>
        /// 属性(base)
        /// </summary>
        property_base,

        /// <summary>
        /// 枚举值
        /// </summary>
        enumitem_use,

        /// <summary>
        /// 属性(use)
        /// </summary>
        property_use,

        /// <summary>
        ///  当前类名称
        /// </summary>
        tname_this,

        /// <summary>
        ///  类名称(import)
        /// </summary>
        tname_import,

        /// <summary>
        ///  字符串
        /// </summary>
        str

    }
}
