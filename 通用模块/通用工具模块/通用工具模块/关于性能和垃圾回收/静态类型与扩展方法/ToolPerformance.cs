using System.Diagnostics;
using System.IOFrancis;
using System.IOFrancis.FileSystem;
using System.Maths;

namespace System.Performance
{
    /// <summary>
    /// 关于性能的工具类
    /// </summary>
    public static class ToolPerformance
    {
        #region 关于临时文件
        #region 缓存目录
        /// <summary>
        /// 获取缓存目录，
        /// 在这个目录中的文件会在程序退出时自动删除
        /// </summary>
        private static IDirectory TemporaryDirectory { get; }
        = CreateIO.Directory(Guid.NewGuid().ToString(), false);
        #endregion
        #region 指定临时文件上限
        /// <summary>
        /// 指定临时文件的大小上限，
        /// 当临时文件达到这个上限后，会将其清空，
        /// 如果为<see langword="null"/>，代表没有上限
        /// </summary>
        public static IUnit<IUTStorage>? TemporaryLimit { get; set; }
        #endregion
        #region 创建临时文件
        /// <summary>
        /// 创建一个临时文件
        /// </summary>
        /// <param name="extension">临时文件的扩展名</param>
        /// <returns>新创建的临时文件，该文件会在程序退出时自动删除</returns>
        public static IFile CreateTemporaryFile(string extension = "")
        {
            if (TemporaryLimit is { } limit && TemporaryDirectory.Size > TemporaryLimit)
                TemporaryDirectory.Clear();
            return TemporaryDirectory.CreateFile(null, extension);
        }
        #endregion
        #endregion
        #region 关于垃圾回收
        #region 强制进行垃圾回收
        /// <summary>
        /// 强制挂起当前线程，并且进行一次垃圾回收，警告：
        /// 频繁使用这个方法反而会影响性能
        /// </summary>
        public static void GcColl()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        #endregion
        #endregion
        #region 关于弱事件
        #region 向弱事件中注册委托
        /// <summary>
        /// 向一个弱事件中注册委托，
        /// 如果这个对象为<see langword="null"/>，
        /// 则将会其实例化一个新<see cref="WeakDelegate{Del}"/>
        /// </summary>
        /// <typeparam name="Del">弱事件封装的委托类型</typeparam>
        /// <param name="weak">要添加委托的弱事件</param>
        /// <param name="delegate">要添加的委托</param>
        public static void AddWeakDel<Del>(ref WeakDelegate<Del>? weak, Del @delegate)
            where Del : Delegate
            => (weak ??= new WeakDelegate<Del>()).Add(@delegate);
        #endregion
        #endregion
        #region 计算程序运行所需时间
        /// <summary>
        /// 测量程序运行所花费的时间
        /// </summary>
        /// <param name="code">函数会执行这个委托，然后计算所花费的时间</param>
        /// <returns>执行<paramref name="code"/>所花费的时间</returns>
        public static TimeSpan Timing(Action code)
        {
            var clock = new Stopwatch();
            clock.Start();
            code();
            clock.Stop();
            return clock.Elapsed;
        }
        #endregion
        #region 静态构造函数
        static ToolPerformance()
        {
            #region 清除缓存的本地函数
            static void Clear(object x, EventArgs y)
                => TemporaryDirectory.Delete();
            #endregion
            var app = AppDomain.CurrentDomain;
            app.ProcessExit += Clear!;               //退出或出现异常时清除缓存
            app.UnhandledException += Clear;
        }
        #endregion
    }
}
