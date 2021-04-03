using System.Office.Excel.Realize;

namespace System.Office.Excel
{
    class ExcelBookNpoi : ExcelBook
    {
        #region 未实现的成员
        public override IOfficePrint Print => throw new NotImplementedException();

        public override bool AutoCalculation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IExcelSheetCollection Sheets => throw new NotImplementedException();

        protected override void SaveRealize(string Path)
        {
            throw new NotImplementedException();
        }

        protected override void DisposeOfficeRealize()
        {
            throw new NotImplementedException();
        }
        #endregion 
    }
}
