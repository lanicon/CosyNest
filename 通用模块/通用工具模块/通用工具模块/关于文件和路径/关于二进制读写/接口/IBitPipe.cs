using System;

namespace System.IO
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个全双工的二进制管道，
    /// 它可以同时读取和写入二进制数据
    /// </summary>
    public interface IBitPipe : IBitWrite, IBitRead
    {
        #region 数据的当前长度
        /// <summary>
        /// 返回二进制数据的当前长度
        /// </summary>
        new long Length { get; }
        #endregion
    }
}
