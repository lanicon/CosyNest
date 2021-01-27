using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Design;
using System.IOFrancis;

namespace System.Office.Realize
{
    /// <summary>
    /// 这个类型代表一个Office文件，
    /// Office对象就是通过它加载的
    /// </summary>
    public abstract class OfficeFile : AutoRelease
    {
        #region 关于文件路径
        #region 说明文档
        /*问：为什么要检查文件是否被占用？
           答：如果同一个文件被两个Office对象加载，
           那么它们保存的时候，结果会互相干扰，为了避免这种情况，
           特意做出如下设计：一个Office对象只能独占一个文件
           
           问：既然尝试加载被占用的Office文件会引发异常，
           那么有没有一个安全的方法可以避免这个异常？
           答：你可以使用GetOfficeFile方法来获取Office对象，
           如果它已经注册，会从字典中直接返回，不会引发异常和对象重新创建*/
        #endregion
        #region 缓存文件路径的字典
        /// <summary>
        /// 这个属性保存文件路径与Office对象的弱引用，
        /// 用来确定该文件是否已被占用
        /// </summary>
        private static IDictionary<string, WeakReferenceGen<OfficeFile>> PathCache { get; }
        = new ConcurrentDictionary<string, WeakReferenceGen<OfficeFile>>();
        #endregion
        #region 通过路径提取Office文件
        /// <summary>
        /// 通过路径从缓存字典中提取Office文件，
        /// 不会引发文件被占用的异常
        /// </summary>
        /// <typeparam name="Obj">返回值类型</typeparam>
        /// <param name="path">Office文件的路径</param>
        /// <param name="Del">如果该Office文件未被创建，
        /// 则通过这个委托创建文件并返回，它的参数就是文件路径</param>
        /// <returns>提取到的Office文件</returns>
        private protected static Obj GetOfficeFile<Obj>(PathText path, Func<PathText, Obj> Del)
            where Obj : OfficeFile
        {
            if (PathCache.TryGetValue(path, out var file))
            {
                if (file.Target != null)
                    return file.Target.To<Obj>();
                PathCache.Remove(path);
            }
            return Del(path);
        }
        #endregion
        #region 文件路径
        private string? PathField;
        /// <summary>
        /// 获取该Office文件的绝对路径文本，
        /// 如果为<see langword="null"/>，代表尚未保存
        /// </summary>
        public string? Path
        {
            get => PathField;
            private set
            {
                if (PathField is { })
                    throw new NotSupportedException($"在{nameof(Path)}不为null的情况下无法写入这个属性");
                if (value is { })
                {
                    if (PathCache.ContainsKey(value))                                       //检查路径是否已被占用
                        throw ExceptionIO.BecauseOccupied(value, "另一个Office文件");
                    else PathCache.Add(value, this);                                    //如果没有被占用，则注册路径
                }
                PathField = value;
            }
        }
        #endregion
        #endregion
        #region 关于保存文件
        #region 是否自动保存
        /// <summary>
        /// 如果这个值为<see langword="true"/>，
        /// 则在执行<see cref="IDisposable.Dispose"/>方法时，还会自动保存文件，
        /// 前提是文件的路径不为<see langword="null"/>
        /// </summary>
        public bool AutoSave { get; set; } = true;
        #endregion
        #region 保存文件
        /// <summary>
        /// 在指定的路径保存这个Office文件
        /// </summary>
        /// <param name="Path">指定的保存路径，
        /// 如果为<see langword="null"/>，代表原地保存</param>
        public void Save(PathText? Path = null)
        {
            switch ((this.Path, Path?.Path))
            {
                case (null, null):
                    throw new Exception("该文件没有指定保存目录");
                case (string p, null):
                    SaveRealize(p); break;
                case (null, string p):
                    this.Path = p;
                    SaveRealize(p);
                    break;
                case (_, string p):
                    SaveRealize(p); break;
            }
        }
        #endregion
        #region 保存文件的抽象方法
        /// <summary>
        /// 保存文件的实际逻辑在这个方法中执行
        /// </summary>
        /// <param name="Path">保存文件的目录</param>
        protected abstract void SaveRealize(string Path);
        #endregion
        #endregion
        #region 关于释放资源
        #region 释放对象所占用的资源
        protected sealed override void DisposeRealize()
        {
            try
            {
                if (Path is { })
                {
                    PathCache.Remove(Path);                     //在释放资源时，也会取消对Office文件的占用
                    if (AutoSave)
                        Save();
                }
            }
            finally
            {
                DisposeOfficeRealize();
            }
        }
        #endregion
        #region 释放资源的抽象方法
        /// <summary>
        /// 这个方法是释放资源的实际执行过程
        /// </summary>
        protected abstract void DisposeOfficeRealize();
        #endregion
        #endregion
        #region 重写ToString方法
        public override string ToString()
            => Path ?? "此Office对象尚未保存到文件中";
        #endregion
        #region 构造方法
        /// <summary>
        /// 用指定的文件路径初始化Office文件
        /// </summary>
        /// <param name="Path">文件路径， 如果该Office对象不是通过文件创建的，则为<see langword="null"/></param>
        /// <param name="Supported">这个Office对象所支持的文件类型</param>
        public OfficeFile(PathText? Path, IFileType Supported)
        {
            if (Path is { })                                       //如果文件路径不为null，则检查扩展名是否受支持
            {
                ExceptionIO.CheckFileType(Path, Supported);
                this.Path = Path;                                   //还会检查路径是否被占用
            }
        }
        #endregion
    }
}
