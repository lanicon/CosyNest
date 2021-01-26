using System.Collections.Generic;
using System.Design;

namespace System.IO
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以直接读取二进制数据
    /// </summary>
    public interface IBitRead : IDisposablePro
    {
        #region 数据的总长度
        /// <summary>
        /// 返回二进制数据的总长度
        /// </summary>
        long Length { get; }
        #endregion
        #region 读取二进制流
        /// <summary>
        /// 以异步流的形式读取二进制数据
        /// </summary>
        /// <returns></returns>
        IAsyncEnumerable<ReadOnlyMemory<byte>> Read();
        #endregion
    }
}
