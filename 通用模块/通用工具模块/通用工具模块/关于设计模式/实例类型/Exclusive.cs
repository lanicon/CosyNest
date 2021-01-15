using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace System.Design
{
    /// <summary>
    /// 表示独占一个文件的对象，
    /// 在它被回收之前，其他对象不能访问这个文件
    /// </summary>
    public abstract class Exclusive
    {
        #region 已经被占用的文件路径
        /// <summary>
        /// 获取已经被占用的文件路径
        /// </summary>
        private static ISet<string> Occupied { get; }
        = new HashSet<string>();
        #endregion
        #region 获取被占用的路径
        /// <summary>
        /// 获取被本对象所占用的路径
        /// </summary>
        protected string Path { get; }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的路径初始化对象
        /// </summary>
        /// <param name="path">指定的路径，这个对象会占用它</param>
        public Exclusive(PathText path)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            if (Occupied.Contains(Path))
                throw ExceptionIO.BecauseOccupied(path, $"另一个{nameof(Exclusive)}对象");
        }
        #endregion
        #region 析构函数
        ~Exclusive()
        {
            Occupied.Remove(Path);
        }
        #endregion
    }
}
