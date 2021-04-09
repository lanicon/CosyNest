using System.Collections.Generic;
using System.Maths;

using OfficeOpenXml;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个类型是使用EPPlus实现的Excel单元格
    /// </summary>
    class ExcelCellsEPPlus : Realize.ExcelCells
    {
        #region 封装的Excel单元格
        /// <summary>
        /// 获取封装的Excel单元格，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private ExcelRange Range { get; }
        #endregion
        #region 未实现的成员
        public override RangeValue? Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string? FormulaR1C1 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string? Link { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override ISizePos VisualPosition => throw new NotImplementedException();

        public override IExcelCells MergeRange => throw new NotImplementedException();

        public override bool IsMerge { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IExcelCells this[int beginRow, int beginColumn, int endRow = -1, int endColumn = -1] => throw new NotImplementedException();

        public override IEnumerable<IExcelCells> CellsAll => throw new NotImplementedException();

        public override IRangeStyle Style { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override (int BeginRow, int BeginCol, int EndRwo, int EndCol) Address => throw new NotImplementedException();
        public override string AddressText(bool isR1C1 = true, bool isComplete = false)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region 构造函数
        /// <inheritdoc cref="Realize.ExcelCells.ExcelCells(IExcelSheet)"/>
        /// <param name="range">封装的Excel单元格，本对象的功能就是通过它实现的</param>
        public ExcelCellsEPPlus(IExcelSheet sheet, ExcelRange range)
            : base(sheet)
        {
            Range = range;
        }
        #endregion
    }
}
