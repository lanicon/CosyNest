using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace System.IO
{
    /// <summary>
    /// 所有实现这个接口的类型，
    /// 都可以视为一个目录
    /// </summary>
    public interface IDirectory : IIO
    {
        #region 有关创建文件或目录
        #region 说明文档
        /*实现这些API请遵循以下规范：
          #如果新文件或目录的名称已经存在，
          不要引发异常，而是自动将名称重命名*/
        #endregion
        #region 在目录下创建目录
        /// <summary>
        /// 在目录下创建新目录
        /// </summary>
        /// <param name="Name">新目录的名称，如果为<see langword="null"/>，
        /// 则给予一个不重复的随机名称</param>
        /// <returns>新创建的目录</returns>
        IDirectory CreateDirectory(string? Name = null);
        #endregion
        #region 在目录下创建文件
        /// <summary>
        /// 在目录下创建新文件
        /// </summary>
        /// <param name="Name">新文件的名称，如果为<see langword="null"/>，则给予一个不重复的随机名称</param>
        /// <param name="Extension">新文件的扩展名</param>
        /// <returns>新创建的文件</returns>
        IFile CreateFile(string? Name = null, string Extension = "");
        #endregion
        #endregion
        #region 有关获取文件或目录
        #region 获取文件
        /// <summary>
        /// 从目录下搜索具有指定名称的文件
        /// </summary>
        /// <param name="Name">要搜索的文件全名</param>
        /// <param name="IsRecursive">如果这个值为<see langword="true"/>，
        /// 则递归搜索目录下的所有文件，否则只搜索直接子文件</param>
        /// <returns>搜索到的文件，如果没有找到，则返回<see langword="null"/></returns>
        IFile? FindFile(string Name, bool IsRecursive = false)
            => (IsRecursive ? SonAll : Son).OfType<IFile>().
            FirstOrDefault(x => x.NameFull == Name);
        #endregion
        #region 获取目录
        /// <summary>
        /// 从目录下搜索具有指定名称的目录
        /// </summary>
        /// <param name="Name">要搜索的目录名称</param>
        /// <param name="IsRecursive">如果这个值为<see langword="true"/>，
        /// 则递归搜索目录下的所有目录，否则只搜索直接子目录</param>
        /// <returns>搜索到的目录，如果没有找到，则返回<see langword="null"/></returns>
        IDirectory? FindDirectory(string Name, bool IsRecursive = false)
            => (IsRecursive ? SonAll : Son).OfType<IDirectory>().
            FirstOrDefault(x => x.NameFull == Name);
        #endregion
        #endregion
    }
}
