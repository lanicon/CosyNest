﻿using System;

namespace System.IOFrancis
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以作为一个二进制数据的基本信息
    /// </summary>
    public interface IBitInfo
    {
        #region 数据的描述
        /// <summary>
        /// 对数据的描述，如果没有描述，
        /// 则为<see langword="null"/>
        /// </summary>
        string? Describe { get; }
        #endregion
        #region 数据的格式
        /// <summary>
        /// 返回二进制数据的格式，
        /// 如果格式未知，则为<see cref="string.Empty"/>
        /// </summary>
        string Format { get; }
        #endregion
        #region 数据的总长度
        /// <summary>
        /// 返回二进制数据的总长度（以字节为单位）
        /// </summary>
        long Length { get; }
        #endregion
    }
}
