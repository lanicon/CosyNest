using System.Collections.Generic;
using System.DrawingFrancis;
using System.DrawingFrancis.Graphics;
using System.Office.Chart;
using System.Office.Excel.Realize;

using OfficeOpenXml;

namespace System.Office.Excel
{
    class ExcelSheetEPPlus : ExcelSheet
    {
        #region 封装的工作表
        /// <summary>
        /// 获取封装的工作表，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private ExcelWorksheet Sheet { get; }
        #endregion
        #region 未实现的成员
        public override IExcelCells RangUser => throw new NotImplementedException();

        public override IExcelRC GetRC(int begin, int? end, bool isRow)
        {
            throw new NotImplementedException();
        }

        public override string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public override IExcelSheet Copy(IExcelSheetCollection collection)
        {
            throw new NotImplementedException();
        }

        public override IPageSheet Page => throw new NotImplementedException();

        public override ICreateExcelChart CreateChart => throw new NotImplementedException();

        public override IEnumerable<IExcelObj<IOfficeChart>> Charts => throw new NotImplementedException();

        public override IEnumerable<IExcelObj<IImage>> Images => throw new NotImplementedException();

        public override IExcelObj<IImage> CreateImage(IImage image)
        {
            throw new NotImplementedException();
        }

        public override ICanvas Canvas => throw new NotImplementedException();
        #endregion
        #region 构造函数
        /// <inheritdoc cref="ExcelSheet(IExcelBook)"/>
        /// <param name="sheet">封装的工作表，本对象的功能就是通过它实现的</param>
        public ExcelSheetEPPlus(IExcelBook book, ExcelWorksheet sheet)
            : base(book)
        {
            Sheet = sheet;
        }
        #endregion
    }
}
