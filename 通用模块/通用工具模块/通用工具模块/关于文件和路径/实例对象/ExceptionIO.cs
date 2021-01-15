using System;
using System.Collections.Generic;
using System.Text;

namespace System.IO
{
    /// <summary>
    /// 表示在进行IO操作时引发的异常
    /// </summary>
    public class ExceptionIO : IOException
    {
        #region 获取和检查异常的静态方法
        #region 错误原因：源文件或目录不存在
        #region 返回异常
        /// <summary>
        /// 获取一个异常预设值，错误原因是源文件不存在，或路径不合法
        /// </summary>
        /// <param name="Path">发生异常的路径</param>
        /// <returns></returns>
        public static ExceptionIO BecauseExist(string Path)
            => new ExceptionIO(Path, "源文件或目录不存在，或者路径不合法");
        #endregion
        #region 抛出异常
        /// <summary>
        /// 检查一个路径，如果它不存在，则抛出异常
        /// </summary>
        /// <param name="Path">要检查的路径</param>
        public static void CheckExist(string Path)
        {
            if (!ToolPath.CheckPathAvailable(Path))
                throw BecauseExist(Path);
        }
        #endregion
        #endregion
        #region 错误原因：路径文本不合法
        #region 返回异常
        /// <summary>
        /// 获取一个异常预设值，错误原因是路径不合法
        /// </summary>
        /// <param name="Path">发生错误的路径</param>
        /// <returns></returns>
        public static ExceptionIO BecauseNotLegal(string Path)
            => new ExceptionIO(Path, "该路径不是合法路径");
        #endregion
        #region 抛出异常
        /// <summary>
        /// 检查一个路径文本，如果它不合法，则抛出一个异常
        /// </summary>
        /// <param name="Path">发生错误的路径</param>
        public static void CheckNotLegal(string Path)
        {
            if (ToolPath.GetPathState(Path) == PathState.NotLegal)
                throw BecauseNotLegal(Path);
        }
        #endregion
        #endregion
        #region 错误原因：文件不是指定的类型
        #region 获取异常
        /// <summary>
        /// 获取一个异常预设值，错误原因是文件类型不受支持
        /// </summary>
        /// <param name="path">发生异常的路径</param>
        /// <param name="fileType">受支持的文件类型</param>
        /// <returns></returns>
        public static ExceptionIO BecauseFileType(string path, IFileType fileType)
            => new ExceptionIO(path, "文件不是受支持的类型",
                $"该文件扩展名：{(IO.Path.GetExtension(path) ?? "该IO对象不是文件，或没有扩展名").TrimStart('.')}",
                $"受支持的文件扩展名：{fileType.ExtensionName.Join("、")}");
        #endregion
        #region 抛出异常
        /// <summary>
        /// 检查一个路径，如果是目录，
        /// 或者它不与指定的文件类型兼容，则抛出一个异常
        /// </summary>
        /// <param name="Path">要检查的路径</param>
        /// <param name="Supported">受支持的文件类型</param>
        public static void CheckFileType(string Path, IFileType Supported)
        {
            if (!Supported.IsCompatiblePath(Path))
                throw BecauseFileType(Path, Supported);
        }
        #endregion
        #endregion
        #region 错误原因：路径已被占用
        /// <summary>
        /// 获取一个异常预设值，错误原因是路径已被占用
        /// </summary>
        /// <param name="Path">被占用的路径</param>
        /// <param name="Another">指示路径被谁占用了，如果不填，在错误描述中不会出现</param>
        /// <returns></returns>
        public static ExceptionIO BecauseOccupied(string Path, string? Another = null)
            => new ExceptionIO(Path, $"该路径已被{Another}占用");
        #endregion
        #endregion
        #region 发生异常的路径
        /// <summary>
        /// 获取发生异常的路径
        /// </summary>
        public string Path { get; }
        #endregion
        #region 构造函数
        /// <summary>
        /// 用指定的路径和异常原因初始化IO异常
        /// </summary>
        /// <param name="Path">发生异常的路径</param>
        /// <param name="Reason">发生异常的原因</param>
        /// <param name="Other">关于异常的其他信息，在显示的时候，会用换行符连接起来</param>
        public ExceptionIO(string Path, string Reason, params string[] Other)
            : base(new[] { $"发生异常的路径：{Path}" ,
            $"异常原因：{Reason}",
            $"{Other.Join(Environment.NewLine)}"}.
                  Join(Environment.NewLine))
        {
            this.Path = Path;
        }
        #endregion
    }
}
