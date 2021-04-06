using System.IOFrancis.FileSystem;
using System.Linq;
using System.Threading.Tasks;
using System.Underlying;

using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个类型是<see cref="IOfficePrint"/>的实现，
    /// 可以用来打印Excel工作簿
    /// </summary>
    class WorkBookPrint : IOfficePrint
    {
        #region 封装的工作簿
        /// <summary>
        /// 获取封装的工作簿，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private Workbook PackBook { get; }
        #endregion
        #region 返回页面数量
        public int PageCount
            => PackBook.Worksheets.OfType<Worksheet>().
            Sum(x => x.PageSetup.Pages.Count);
        #endregion
        #region 打印工作簿
        #region 基础方法
        /// <summary>
        /// 打印工作簿的基础方法
        /// </summary>
        /// <param name="Page">打印的页码范围</param>
        /// <param name="Number">打印的份数</param>
        /// <param name="Printer">执行打印的打印机</param>
        /// <param name="FilePath">打印到文件的路径</param>
        /// <returns></returns>
        private async Task PrintBase(Range? Page = null, int Number = 1, IPrinter? Printer = null, PathText? FilePath = null)
        {
            var Application = PackBook.Application;
            var (Beg, End) = (Page ?? Range.All).GetOffsetAndEnd(PageCount);
            var ActivePrinter = Application.ActivePrinter;
            await Task.Run(() => PackBook.PrintOut
                  (Beg + 1, End + 1, Number,
                  ActivePrinter: MSOfficeRealize.PrinterName(ActivePrinter, Printer, FilePath),
                  PrintToFile: FilePath == null ? null : true,
                  PrToFileName: FilePath?.Path));
            Application.ActivePrinter = ActivePrinter;      //还原默认打印机，不破坏设置
        }
        #endregion
        #region 打印到纸张
        public Task PrintFromPage(Range? Page = null, int Number = 1, IPrinter? Printer = null)
        {
            ExceptionIntervalOut.Check(1, null, Number);
            return PrintBase(Page, Number, Printer, null);
        }
        #endregion
        #region 打印到文件
        public Task PrintFromPageToFile(Range? Page, PathText FilePath)
            => PrintBase(Page, FilePath: FilePath);
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 将指定的工作簿封装进对象
        /// </summary>
        /// <param name="PackBook">指定的工作簿</param>
        public WorkBookPrint(Workbook PackBook)
        {
            this.PackBook = PackBook;
        }
        #endregion
    }
}
