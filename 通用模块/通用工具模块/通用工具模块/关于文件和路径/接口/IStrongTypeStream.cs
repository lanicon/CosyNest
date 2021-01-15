using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个强类型的流，
    /// 它可以告诉调用者如何解读流中的数据
    /// </summary>
    public interface IStrongTypeStream : IDisposable
    {
        #region 返回流
        /// <summary>
        /// 返回用来枚举数据的流
        /// </summary>
        Stream Stream { get; }
        #endregion
        #region 数据的扩展名
        /// <summary>
        /// 返回数据的扩展名，不带点号
        /// 它是用来解释数据的正确方式
        /// </summary>
        string NameExtension { get; }
        #endregion
        #region 对数据的描述
        /// <summary>
        /// 对数据的描述
        /// </summary>
        string? Describe { get; }
        #endregion
        #region 将流保存到文件中
        /// <summary>
        /// 将流保存到文件中
        /// </summary>
        /// <param name="path">要保存的路径，不需要加上扩展名</param>
        /// <returns>执行保存文件的异步任务</returns>
        Task Save(PathText path)
            => Stream.Save($"{path}.{NameExtension}");
        #endregion
        #region 解构对象
        /// <summary>
        /// 将对象解构为流，扩展名和描述
        /// </summary>
        /// <param name="Stream">用来接收流的对象</param>
        /// <param name="NameExtension">用来接收扩展名的对象</param>
        /// <param name="Describe">对数据的描述</param>
        void Deconstruct(out Stream Stream, out string NameExtension, out string? Describe)
        {
            Stream = this.Stream;
            NameExtension = this.NameExtension;
            Describe = this.Describe;
        }
        #endregion
    }
}
