using System.IOFrancis;
using System.IOFrancis.FileSystem;
using System.Linq;

namespace System
{
    /// <summary>
    /// 这个类型的方法仅用于测试
    /// </summary>
    static class Test
    {
        #region 计算代码行数
        public static int CodeLine()
        {
            var readString = CreateIO.ObjReadString();
            return CreateIO.Directory(@"‪C:\CosyNest").SonAll.OfType<IFile>().
                Where(x => x.NameExtension is "cs" or "html" or "cshtml" or "razor").
                Sum(x => x.GetBitPipe().ReadComplete().Result.Length);
        }
        #endregion
    }
}
