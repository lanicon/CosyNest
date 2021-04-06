using System.IO;
using System.IOFrancis.FileSystem;
using System.Office.Excel.Realize;

using OfficeOpenXml;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个类型代表使用EPPlus实现的Excel工作簿
    /// </summary>
    class ExcelBookEPPlus : ExcelBook
    {
        #region Excel封装包
        /// <summary>
        /// 获取Excel封装包对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private ExcelPackage ExcelPackage { get; }
        #endregion
        #region 关于工作簿
        #region 释放工作簿
        protected override void DisposeOfficeRealize()
            => ExcelPackage.Dispose();
        #endregion
        #region 保存工作簿
        protected override void SaveRealize(string path)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
        #region 返回打印对象
        public override IOfficePrint Print => throw new NotImplementedException();
        #endregion
        #region 返回工作表集合
        public override IExcelSheetCollection Sheets => throw new NotImplementedException();
        #endregion
        #region 构造函数
        #region 使用流
        /// <summary>
        /// 使用指定的流初始化Excel工作簿
        /// </summary>
        /// <param name="stream">Excel工作簿的流</param>
        public ExcelBookEPPlus(Stream stream)
            : base(null, CreateEPPlusOffice.SupportExcel)
        {
            ExcelPackage = new ExcelPackage(stream);
        }
        #endregion
        #region 使用路径
        /// <summary>
        /// 使用指定的路径初始化Excel工作簿
        /// </summary>
        /// <param name="path">Excel工作簿的路径，
        /// 如果为<see langword="null"/>，则不从文件中加载，而是创建一个新的工作簿</param>
        public ExcelBookEPPlus(PathText? path)
            : base(path, CreateEPPlusOffice.SupportExcel)
        {
            ExcelPackage = path is null ? new() : new(new FileInfo(path));
        }
        #endregion
        #endregion
    }
}
