using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace System
{
    /// <summary>
    /// 所有有关PC的扩展方法全部放在这里
    /// </summary>
    public static class ExtenPC
    {
        #region 关于IO
        #region 把文件或目录移动到回收站
        /// <summary>
        /// 将文件或目录移动到回收站
        /// </summary>
        /// <param name="IO">待删除的文件或目录</param>
        public static void DeleteToRecycling(this IIO IO)
        {
            var DeleteMod = RecycleOption.SendToRecycleBin;
            var DialogMod = UIOption.OnlyErrorDialogs;
            switch (IO)
            {
                case IFile f:
                    FileSystem.DeleteFile(f.Path, DialogMod, DeleteMod);
                    break;
                case IDirectory d:
                    FileSystem.DeleteDirectory(d.Path, DialogMod, DeleteMod);
                    break;
                default:
                    throw new ExceptionTypeUnlawful(IO, typeof(IFile), typeof(IDirectory));
            }
        }
        #endregion
        #endregion
    }
}
