using System.IO;
using System.Linq;

namespace System
{
    /// <summary>
    /// 这个类型的方法仅用于测试
    /// </summary>
    static class Test
    {
        #region 计算代码行数
        /// <summary>
        /// 调用这个方法可以计算本项目代码行数
        /// </summary>
        /// <returns></returns>
        public static int CodeLine()
            => CreateIO.Directory(@"C:\CosyNest").SonAll.OfType<IFile>().
            Where(x => x.NameExtension is "cs" or "html" or "razor" or "cshtml").
            Sum(x => x.GetStream().Release(y => y.Stream.ReadText().Count()));
        #endregion
    }
}
