using System;
using System.Collections.Generic;
using System.Text;

namespace System.IO
{
    /// <summary>
    /// 这个类型是<see cref="IStrongTypeStream"/>的实现，
    /// 可以作为一个强类型的流
    /// </summary>
    class StrongTypeStream : IStrongTypeStream
    {
        #region 返回流
        public Stream Stream { get; }
        #endregion
        #region 数据的扩展名
        public string NameExtension { get; }
        #endregion
        #region 对数据的描述
        public string? Describe { get; }
        #endregion
        #region 释放流
        public void Dispose()
            => Stream.Dispose();
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的流和扩展名初始化对象
        /// </summary>
        /// <param name="Stream">用来枚举数据的流</param>
        /// <param name="NameExtension">数据的扩展名</param>
        /// <param name="Describe">对数据的描述</param>
        public StrongTypeStream(Stream Stream, string NameExtension, string? Describe = null)
        {
            this.Stream = Stream;
            this.NameExtension = NameExtension;
            this.Describe = Describe;
        }
        #endregion
    }
}
