﻿using System.Design;
using System.Threading.Tasks;

namespace System.IO
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以直接写入二进制数据
    /// </summary>
    public interface IBitWrite : IDisposablePro
    {
        #region 数据的当前长度
        /// <summary>
        /// 返回二进制数据的当前长度
        /// </summary>
        long Length { get; }
        #endregion
        #region 写入数据
        /// <summary>
        /// 写入二进制数据
        /// </summary>
        /// <param name="data">待写入的二进制数据</param>
        /// <returns></returns>
        Task Write(ReadOnlyMemory<byte> data);
        #endregion
    }
}
