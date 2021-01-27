using System.IOFrancis;
using System.Linq;
using System.Underlying;

using Microsoft.Office.Interop.Word;

using Task = System.Threading.Tasks.Task;

namespace System.Office.Word
{
    /// <summary>
    /// 这个类型是<see cref="IPage"/>的实现，
    /// 可以管理Word文档的页面设置和打印
    /// </summary>
    class WordPage : IPage
    {
        #region 封装的Word文档
        /// <summary>
        /// 获取封装的Word文档对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private Document PackDocument { get; }
        #endregion
        #region 返回页数
        public int PageCount
            => PackDocument.ComputeStatistics(WdStatistic.wdStatisticPages);
        #endregion
        #region 打印文档
        #region 基础方法
        /// <summary>
        /// 打印工作簿的基本方法
        /// </summary>
        /// <param name="Page">要打印的页码范围</param>
        /// <param name="Number">要打印的份数</param>
        /// <param name="Printer">执行打印的打印机</param>
        /// <param name="FilePath">如果要打印到文件，则指定这个参数以指定输出路径</param>
        /// <returns></returns>
        private async Task PrintBase(Range? Page, int Number = 1, IPrinter? Printer = null, PathText? FilePath = null)
        {
            var (Beg, End) = (Page ?? Range.All).GetOffsetAndEnd(PageCount);
            var Application = PackDocument.Application;
            var ActivePrinter = Application.ActivePrinter;
            Application.ActivePrinter = MSOfficeRealize.PrinterName(ActivePrinter, Printer, FilePath);
            var PageRange = $"{Beg + 1}-{End + 1}";
            await (FilePath == null ?
                Task.Run(() => PackDocument.PrintOut(
                Range: WdPrintOutRange.wdPrintRangeOfPages,
                Pages: PageRange, Copies: Number)) :
                Task.Run(() => PackDocument.PrintOut(
                Range: WdPrintOutRange.wdPrintRangeOfPages,
                Pages: PageRange, Copies: Number,
                PrintToFile: true,
                OutputFileName: FilePath.Path)));
            Application.ActivePrinter = ActivePrinter;      //还原默认打印机设置，不破坏原有设置
        }
        #endregion
        #region 打印到纸张
        public Task PrintFromPage(Range? Page = null, int Number = 1, IPrinter? Printer = null)
        {
            ExceptionIntervalOut.Check(1, null, Number);
            return PrintBase(Page, Number, Printer);
        }
        #endregion
        #region 打印到文件
        public Task PrintFromPageToFile(Range? Page, PathText FilePath)
            => PrintBase(Page, FilePath: FilePath);
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 将指定的Word文档封装进对象
        /// </summary>
        /// <param name="PackDocument">被封装的Word文档</param>
        public WordPage(Document PackDocument)
        {
            this.PackDocument = PackDocument;
        }
        #endregion
    }
}
