using System;
using System.Linq;

namespace System.IO
{
    /// <summary>
    /// 这个静态类为创建IO对象提供帮助
    /// </summary>
    public static class CreateIO
    {
        #region 返回当前文件系统
        /// <summary>
        /// 返回当前文件系统，
        /// 它是所有文件，目录，驱动器的根
        /// </summary>
        public static IFileSystem FileSystem { get; }
        = new FileSystemRealize();
        #endregion
        #region 创建文件或目录对象
        #region 创建一个文件对象
        /// <summary>
        /// 用指定的路径初始化文件对象，
        /// 不允许指定不存在的路径
        /// </summary>
        /// <param name="Path">指定的路径</param>
        /// <param name="CheckExist">在文件不存在的时候，如果这个值为<see langword="true"/>，
        /// 则抛出一个异常，为<see langword="false"/>，则不会抛出异常，而是会创建一个新文件</param>
        public static IFile File(PathText Path, bool CheckExist = true)
            => new FileRealize(Path, CheckExist);
        #endregion
        #region 创建一个目录对象
        /// <summary>
        /// 用指定的路径初始化目录对象
        /// </summary>
        /// <param name="Path">指定的路径</param>
        /// <param name="CheckExist">在路径不存在的时候，如果这个值为<see langword="true"/>，会抛出一个异常，
        /// 如果为<see langword="false"/>，则不会抛出异常，而是会创建这个目录</param>
        public static IDirectory Directory(PathText Path, bool CheckExist = true)
            => new DirectoryRealize(Path, CheckExist);
        #endregion
        #region 根据路径，返回IO对象
        #region 返回IIO
        /// <summary>
        /// 如果一个路径是文件，返回文件对象，
        /// 是目录，返回目录对象，
        /// 不存在，返回<see langword="null"/>
        /// </summary>
        /// <param name="path">要检查的路径</param>
        /// <returns></returns>
        public static IIO? IO(PathText path)
            => ToolPath.GetPathState(path) switch
            {
                PathState.ExistDirectory => new DirectoryRealize(path),
                PathState.ExistFile => new FileRealize(path),
                _ => null
            };
        #endregion
        #region 返回Dynamic
        /// <summary>
        /// 如果一个路径是文件，返回文件对象，
        /// 是目录，返回目录对象，
        /// 不存在，返回<see langword="null"/>
        /// </summary>
        /// <param name="path">要检查的路径</param>
        /// <returns></returns>
        public static dynamic? IODy(PathText path)
            => IO(path);
        #endregion
        #endregion
        #endregion
        #region 创建IBitPipe
        #region 指定流
        /// <summary>
        /// 创建一个<see cref="IBitPipe"/>对象，
        /// 它可以通过<see cref="IO.Stream"/>来读写二进制数据
        /// </summary>
        /// <param name="Stream">封装的<see cref="IO.Stream"/>对象，本对象的功能就是通过它实现的</param>
        /// <param name="BufferSize">缓冲区的大小，每次读取数据时，
        /// 至多返回该数量的字节，如果为<see langword="null"/>，则一次读取全部数据</param>
        /// <param name="Format">二进制数据的格式，如果格式未知，则为<see langword="null"/></param>
        /// <returns></returns>
        public static IBitPipe BitPipe(Stream Stream, long? BufferSize = null, string? Format = null)
            => new BitPipeStream(Stream, BufferSize, Format);
        #endregion
        #endregion
        #region 创建IStrongTypeStream
        #region 指定扩展名和说明
        /// <summary>
        /// 创建一个强类型的流，并返回
        /// </summary>
        /// <param name="Stream">用来枚举数据的流</param>
        /// <param name="NameExtension">数据的扩展名，不带点号
        /// 它可以告诉调用者，如何解释数据</param>
        /// <param name="Describe">对数据的描述</param>
        /// <returns></returns>
        public static IStrongTypeStream Stream(Stream Stream, string NameExtension, string? Describe = null)
            => new StrongTypeStream(Stream, NameExtension, Describe);
        #endregion
        #region 指定全名
        /// <summary>
        /// 创建一个强类型的流，并返回
        /// </summary>
        /// <param name="Stream">用来枚举数据的流</param>
        /// <param name="NameFull">流的全名，带点号和扩展名</param>
        /// <returns></returns>
        public static IStrongTypeStream StreamFull(Stream Stream, string NameFull)
        {
            var span = NameFull.Split(".");
            return CreateIO.Stream(Stream, span.Length > 1 ? span[1] : "", span[0]);
        }
        #endregion
        #endregion
        #region 创建IFileType
        #region 传入文件名
        /// <summary>
        /// 用指定的扩展名集合创建文件类型，
        /// 注意：执行此方法会将文件类型注册
        /// </summary>
        /// <param name="Description">对文件类型的说明</param>
        /// <param name="Extension">指定的扩展名集合，不带点号</param>
        public static IFileType FileType(string Description, params string[] Extension)
            => new FileType(Description, Extension);
        #endregion
        #region 传入文件类型
        /// <summary>
        /// 用指定的文件类型创建文件类型，
        /// 注意：执行此方法会将文件类型注册
        /// </summary>
        /// <param name="Description">对文件类型的说明</param>
        /// <param name="Compatible">初始化后的新对象将会兼容以上所有文件类型</param>
        public static IFileType FileType(string Description, params IFileType[] Compatible)
            => FileType(Description,
                 Compatible.Select(x => x.ExtensionName).UnionNesting(true).ToArray());
        #endregion
        #endregion
    }
}
