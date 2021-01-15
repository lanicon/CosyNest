using System.IO;
using System.Threading.Tasks;
using System.Underlying;

namespace System.Office
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个Office打印对象，
    /// 它可以打印工作表，工作簿或Word文档，
    /// 但是不提供页面设置功能
    /// </summary>
    public interface IOfficePrint
    {
        #region 返回页数
        /// <summary>
        /// 返回全部页数
        /// </summary>
        int PageCount { get; }
        #endregion
        #region 打印到纸张
        /// <summary>
        /// 按照页码将这个Excel工作表，工作簿，或Word文档打印到纸张
        /// </summary>
        /// <param name="Page">这个参数指示打印的页码范围，
        /// 如果为<see langword="null"/>，代表全部打印，页码从0开始</param>
        /// <param name="Number">打印的份数</param>
        /// <param name="Printer">执行打印的打印机，如果为<see langword="null"/>，则使用默认打印机</param>
        /// <returns>一个用于等待打印任务完成的<see cref="Task"/></returns>
        Task PrintFromPage(Range? Page = null, int Number = 1, IPrinter? Printer = null);
        #endregion
        #region 打印到文件
        /// <summary>
        /// 按照页码将这个Excel工作表，工作簿或Word文档打印到文件
        /// </summary>
        /// <param name="Page">这个参数指示打印的页码范围，
        /// 如果为<see langword="null"/>，代表全部打印，页码从0开始</param>
        /// <param name="FilePath">指定打印的目标文件路径，
        /// 函数会根据该路径的扩展名自动判断应使用哪个打印机</param>
        /// <returns>一个用于等待打印任务完成的<see cref="Task"/></returns>
        Task PrintFromPageToFile(Range? Page, PathText FilePath);
        #endregion
    }
}
