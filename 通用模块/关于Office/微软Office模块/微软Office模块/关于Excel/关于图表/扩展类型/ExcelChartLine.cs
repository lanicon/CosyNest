using Microsoft.Office.Interop.Excel;

using System.Office.Chart;

namespace System.Office.Excel.Chart
{
    /// <summary>
    /// 这个类型代表一个Excel折线图
    /// </summary>
    class ExcelChartLine : ExcelChartBase, IOfficeChartLine
    {
        #region 构造函数
        /// <summary>
        /// 使用指定的形状对象初始化折线图
        /// </summary>
        /// <param name="PackShape">被封装的图表</param>
        public ExcelChartLine(Shape PackShape)
            : base(PackShape)
        {

        }
        #endregion 
    }
}
