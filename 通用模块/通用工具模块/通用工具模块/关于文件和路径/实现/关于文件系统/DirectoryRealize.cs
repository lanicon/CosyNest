using System.Collections.Generic;
using System.Linq;
using System.Maths;

namespace System.IO
{
    /// <summary>
    /// 这个类型是<see cref="IDirectory"/>的实现，
    /// 可以被视为一个目录
    /// </summary>
    class DirectoryRealize : IORealize, IDirectory
    {
        #region 封装的对象
        #region 封装的目录对象
        /// <summary>
        /// 获取封装的目录对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private new DirectoryInfo PackFS
        {
            get => (DirectoryInfo)base.PackFS;
            set => base.PackFS = value;
        }
        #endregion
        #region 封装本对象的接口形式
        /// <summary>
        /// 返回本对象的接口形式，
        /// 它使本对象的成员可以访问显式接口实现的成员
        /// </summary>
        private IDirectory Directory
            => this;
        #endregion
        #endregion
        #region 关于目录的信息
        #region 枚举子目录
        public override IEnumerable<INode> Son
            => PackFS.EnumerateFileSystemInfos().Select(x => x switch
             {
                 FileInfo a => (INode)new FileRealize(a.FullName),
                 DirectoryInfo a => new DirectoryRealize(a.FullName),
                 _ => throw new Exception("无法识别的类型")
             });
        #endregion
        #region 获取目录的大小
        public override IUnit<IUTStorage> Size
            => this.To<INode>().Father is IDrive d ?
            d.SizeUsed :
            Directory.Son.Select(x => x.Size).Sum();
        #endregion
        #endregion
        #region 关于对目录的操作
        #region 复制
        public override IIO Copy(IDirectory? Target, string? NewName = null, Func<string, int, string>? Rename = null)
        {
            Target ??= Directory.Father ?? throw new ArgumentException("父目录不能为null");
            NewName ??= Directory.NameFull;
            if (Rename != null)
                NewName = ToolPath.Distinct(Target, NewName, Rename);
            var NewPosition = new DirectoryRealize(IO.Path.Combine(Target.Path, NewName), false);                //如果父目录不存在，则会自动创建
            Directory.Son.ForEach(x => x.Copy(NewPosition));
            return NewPosition;
        }
        #endregion
        #region 关于创建文件与目录
        #region 辅助方法
        /// <summary>
        /// 创建文件或目录的辅助方法
        /// </summary>
        /// <typeparam name="IO">返回值类型</typeparam>
        /// <param name="Name">新文件或目录的名称，如果为<see langword="null"/>，
        /// 则自动赋予一个不重复的随即名称</param>
        /// <param name="Extension">新文件的扩展名，如果为<see langword="null"/>，
        /// 代表没有扩展名</param>
        /// <param name="Create">用于创建新文件或目录的委托</param>
        /// <returns></returns>
        private IO CreateAided<IO>(string? Name, string? Extension, Func<string, IO> Create)
            where IO : IIO
        {
            #region 本地函数
            IO Get(string? Name)
                => Create(System.IO.Path.Combine(Path, Name ?? Guid.NewGuid().ToString() + Extension));
            #endregion
            return Name == null ?
                Get(Name) :
                Get(Son.OfType<IIO>().Select(x => x.NameFull).Distinct(Name));        //保证文件或目录名不重复
        }
        #endregion
        #region 在目录下创建目录
        public IDirectory CreateDirectory(string? Name = null)
            => CreateAided(Name, null, x => CreateIO.Directory(x, false));
        #endregion
        #region 在目录下创建文件
        public IFile CreateFile(string? Name = null, string Extension = "")
            => CreateAided(Name, Extension, x => CreateIO.File(x, false));
        #endregion
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 用指定的路径初始化目录对象
        /// </summary>
        /// <param name="Path">指定的路径</param>
        /// <param name="CheckExist">在路径不存在的时候，如果这个值为<see langword="true"/>，会抛出一个异常，
        /// 如果为<see langword="false"/>，则不会抛出异常，而是会创建这个目录</param>
        public DirectoryRealize(PathText Path, bool CheckExist = true)
        {
            PackFS = new DirectoryInfo(Path);
            if (!PackFS.Exists)
            {
                if (CheckExist)
                    throw ExceptionIO.BecauseExist(Path.Path ?? "null");
                PackFS.Create();
                PackFS.Refresh();
            }
        }
        #endregion
    }
}
