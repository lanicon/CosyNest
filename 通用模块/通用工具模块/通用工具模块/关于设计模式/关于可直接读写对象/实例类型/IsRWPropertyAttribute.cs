using System;
using System.Collections.Generic;
using System.Text;

namespace System.Design.Direct
{
    /// <summary>
    /// 这个特性被用于修饰实现<see cref="IDirect"/>的对象的属性，
    /// 指示它可以通过<see cref="IDirect"/>的索引器直接读写
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IsRWPropertyAttribute : Attribute
    {
        #region 属性的别名
        /// <summary>
        /// 获取被修饰的属性的别名，
        /// 它可以允许在通过索引器访问它的时候，
        /// 使用一个不一样的名称
        /// </summary>
        public string? Alias { get; }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的别名初始化特性
        /// </summary>
        /// <param name="Alias">指定的别名，
        /// 如果为<see langword="null"/>，代表它没有别名</param>
        public IsRWPropertyAttribute(string? Alias = null)
        {
            this.Alias = Alias;
        }
        #endregion
    }
}
