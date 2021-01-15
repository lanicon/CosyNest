using System.Maths;

using static System.Maths.CreateMathObj;
using static System.Maths.ToolArithmetic;

namespace System.Office.Excel
{
    /// <summary>
    /// 有关Excel的工具类
    /// </summary>
    public static class ToolExcel
    {
        #region 创建数学地址
        #region 绝对地址
        /// <summary>
        /// 指定开始和结束单元格的行列数，
        /// 并返回该单元格的数学地址
        /// </summary>
        /// <param name="BeginRow">起始单元格的行数</param>
        /// <param name="BeginCol">起始单元格的列数</param>
        /// <param name="EndRwo">结束单元格的行数，
        /// 如果为<see langword="null"/>，则与<paramref name="BeginRow"/>相同</param>
        /// <param name="EndCol">结束单元格的列数，
        /// 如果为<see langword="null"/>，则与<paramref name="BeginCol"/>相同</param>
        /// <returns></returns>
        public static ISizePosPixel AddressMathAbs(int BeginRow, int BeginCol, int? EndRwo = null, int? EndCol = null)
        {
            var ER = EndRwo ?? BeginRow;
            var EC = EndCol ?? BeginCol;
            ExceptionIntervalOut.Check(0, null, BeginCol, EC);
            return SizePosPixel(Point(BeginCol, Abs(BeginRow)), Point(EC, Abs(ER)));
        }
        #endregion
        #region 相对地址
        /// <summary>
        /// 指定开始单元格的行列数，
        /// 以及单元格行数量和列数量,
        /// 并返回该单元格的数学地址
        /// </summary>
        /// <param name="BeginRow">起始单元格的行数</param>
        /// <param name="BeginCol">起始单元格的列数</param>
        /// <param name="RowCount">单元格的行数量</param>
        /// <param name="ColumnCount">单元格的列数量</param>
        /// <returns></returns>
        public static ISizePosPixel AddressMathRel(int BeginRow, int BeginCol, int RowCount = 1, int ColumnCount = 1)
        {
            ExceptionIntervalOut.Check(0, null, BeginRow);
            ExceptionIntervalOut.Check(1, null, RowCount, ColumnCount);
            return SizePosPixel(BeginCol, Abs(BeginRow), ColumnCount, RowCount);
        }
        #endregion
        #endregion
    }
}
