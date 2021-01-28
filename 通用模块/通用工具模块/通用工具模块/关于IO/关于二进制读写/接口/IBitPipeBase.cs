using System.Design;
using System.IO;

namespace System.IOFrancis.Bit
{
    /// <summary>
    /// 这个接口是所有二进制管道的基接口
    /// </summary>
    public interface IBitPipeBase : IDisposablePro
    {
        #region 转换为流
        /// <summary>
        /// 将这个二进制管道转换为等效的<see cref="Stream"/>
        /// </summary>
        /// <returns></returns>
        Stream ToStream();
        #endregion
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
