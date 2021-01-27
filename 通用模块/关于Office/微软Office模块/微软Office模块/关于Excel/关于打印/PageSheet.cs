using System.IOFrancis;
using System.Linq;
using System.Maths;
using System.Office.Excel.Realize;
using System.Threading.Tasks;
using System.Underlying;

using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个对象是<see cref="IPageSheet"/>的实现，
    /// 可以视为一个Excel页面对象
    /// </summary>
    class PageSheet : IPageSheet
    {
        #region 封装的对象
        #region 封装的工作表
        /// <summary>
        /// 获取封装的工作表，本对象的功能就是通过它实现的
        /// </summary>
        private Worksheet PackSheet { get; }
        #endregion
        #region 封装的APP对象
        /// <summary>
        /// 获取封装的工作表的APP对象
        /// </summary>
        private Application Application
            => PackSheet.Application;
        #endregion
        #endregion
        #region 获取或设置打印区域
        public ISizePosPixel? PrintRegional
        {
            get
            {
                var add = PackSheet.PageSetup.PrintArea;
                return add.IsVoid() ? null : ExcelRealize.AddressToTISizePos(add);
            }
            set => PackSheet.PageSetup.PrintArea =
                value == null ? "" : ExcelRealize.GetAddress(value);
        }
        #endregion
        #region 返回页数
        public int PageCount
            => PackSheet.PageSetup.Pages.Count;
        #endregion
        #region 打印工作表
        #region 辅助方法
        #region 执行打印
        /// <summary>
        /// 执行打印操作
        /// </summary>
        /// <param name="From">打印的起始页数，从0开始，如果为<see langword="null"/>，代表通过打印区域确定打印范围</param>
        /// <param name="To">打印的结束页数，从0开始，代表通过打印区域确定打印范围</param>
        /// <param name="Number">打印的份数</param>
        /// <param name="Printer">指定的打印机名称，如果为<see langword="null"/>，代表不打印到文件</param>
        /// <param name="FilePath">指定的打印到文件的路径，如果为<see langword="null"/>，代表不打印到纸张</param>
        /// <returns>一个用于等待打印任务完成的<see cref="Task"/></returns>
        private Task Print(int? From = null, int? To = null, int Number = 1, IPrinter? Printer = null, PathText? FilePath = null)
            => Task.Run(() => PackSheet.PrintOut
            (From is null ? 1 : From.Value + 1,
                To is null ? PageCount : To.Value + 1, Number,
                ActivePrinter: MSOfficeRealize.PrinterName(Application.ActivePrinter, Printer, FilePath),
                PrintToFile: FilePath == null ? null : (object)true,
                PrToFileName: FilePath?.Path));
        #endregion
        #region 按照页数打印的基础方法
        /// <summary>
        /// 按照页数打印的基本方法
        /// </summary>
        /// <param name="Page">要打印的页数范围</param>
        /// <param name="Number">要打印的份数</param>
        /// <param name="Printer">打印机的名称</param>
        /// <param name="FilePath">要打印到的文件路径</param>
        /// <returns>一个用于等待打印任务完成的<see cref="Task"/></returns>
        private async Task PrintFromPageBase(Range? Page = null, int Number = 1, IPrinter? Printer = null, PathText? FilePath = null)
        {
            var ActivePrinter = Application.ActivePrinter;
            var (Beg, End) = (Page ?? Range.All).GetOffsetAndEnd(PageCount);
            await Print(Beg, End, Number, Printer, FilePath);
            Application.ActivePrinter = ActivePrinter;                      //打印后还原活动打印机，不破坏原有设置
        }
        #endregion
        #region 按照范围打印的基础方法
        /// <summary>
        /// 按照范围打印的基础方法
        /// </summary>
        /// <param name="Regional">打印区域，
        /// 如果为<see langword="null"/>，则遵照<see cref="PrintRegional"/>属性设置的打印区域</param>
        /// <param name="Number">打印的份数</param>
        /// <param name="Printer">执行打印的打印机，如果为<see langword="null"/>，则使用默认打印机</param>
        /// <param name="FilePath">指定打印的目标文件路径，
        /// 函数会根据该路径的扩展名自动判断应使用哪个打印机</param>
        /// <returns>一个用于等待打印任务完成的<see cref="Task"/></returns>
        private async Task PrintFromRegionalBase(ISizePosPixel? Regional = null, int Number = 1, IPrinter? Printer = null, PathText? FilePath = null)
        {
            var ActivePrinter = Application.ActivePrinter;
            if (Regional != null)
            {
                var PS = PackSheet.PageSetup;
                var PrintArea = PS.PrintArea;
                PS.PrintArea = ExcelRealize.GetAddress(Regional);
                await Print(Number: Number, Printer: Printer, FilePath: FilePath);
                PS.PrintArea = PrintArea;                                          //打印后还原默认打印区域，不破坏原有设置
            }
            else await Print(Number: Number, Printer: Printer, FilePath: FilePath);
            Application.ActivePrinter = ActivePrinter;                  //打印后还原活动打印机，不破坏原有设置
        }
        #endregion
        #endregion
        #region 按照页数打印
        #region 打印到文件
        public Task PrintFromPageToFile(Range? Page, PathText FilePath)
            => PrintFromPageBase(Page, FilePath: FilePath);
        #endregion
        #region 打印到纸张
        public Task PrintFromPage(Range? Page = null, int Number = 1, IPrinter? Printer = null)
        {
            ExceptionIntervalOut.Check(1, null, Number);
            return PrintFromPageBase(Page, Number, Printer);
        }
        #endregion
        #endregion
        #region 按照范围打印
        #region 打印到文件
        public Task PrintFromRegionalToFile(ISizePosPixel? Regional, PathText FilePath)
            => PrintFromRegionalBase(Regional, FilePath: FilePath);
        #endregion
        #region 打印到纸张
        public Task PrintFromRegional(ISizePosPixel? Regional = null, int Number = 1, IPrinter? Printer = null)
        {
            ExceptionIntervalOut.Check(1, null, Number);
            return PrintFromRegionalBase(Regional, Number, Printer);
        }
        #endregion
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 将指定的工作表封装进对象
        /// </summary>
        /// <param name="PackSheet">指定的工作表</param>
        public PageSheet(Worksheet PackSheet)
        {
            this.PackSheet = PackSheet;
        }
        #endregion
    }
}
