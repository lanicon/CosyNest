using System.IOFrancis;
using System.IOFrancis.FileSystem;

namespace System.Office
{
    /// <summary>
    /// 这个静态类为创建使用EPPlus实现的Office对象提供帮助
    /// </summary>
    public static class CreateEPPlusOffice
    {
        #region 返回受支持的文件类型
        /// <summary>
        /// 返回受本模块支持的Excel文件类型
        /// </summary>
        public static IFileType SupportExcel { get; }
        = CreateIO.FileType("受EPPlusOffice模块支持的Excel文件类型",
            OfficeFileCom.Excel2007,
            OfficeFileCom.Excel2007Macro);
        #endregion
    }
}
