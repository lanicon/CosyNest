using System;
using System.Collections.Generic;
using System.Office.Chart;
using System.Office.Word;
using System.Text;

using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个类型代表了封装图表的Excel对象
    /// </summary>
    /// <typeparam name="ChartType">图表的类型</typeparam>
    class ExcelObjChart<ChartType> : ExcelObj<ChartType>
        where ChartType : IOfficeChart
    {
        #region Excel对象的内容
        public override ChartType Content { get; }
        #endregion
        #region 复制Excel对象
        #region 复制到工作表
        public override IExcelObj<ChartType> Copy(IExcelSheet Target)
        {
            PackShape.Copy();
            var NewChart = Target.To<ExcelSheetMicrosoft>().PackSheet.Shapes.AddChart2();
            NewChart.Chart.Paste();
            NewChart.Chart.ChartType = PackShape.Chart.ChartType;
            return (IExcelObj<ChartType>)NewChart.ToChart(Sheet);
        }
        #endregion
        #region 复制到Word文档
        public override IWordParagraphObj<ChartType> Copy(IWordDocument Target, Index? Pos = null)
        {
            var Doc = Target.To<WordDocumentMicrosoft>();
            var Index = Doc.ToUnderlying(Doc.ToIndexActual(Pos), false);
            var PackDoc = Doc.PackDocument;
            var NewChart = PackDoc.InlineShapes.AddChart2(Range: PackDoc.Range(Index));
            PackShape.Copy();
            NewChart.Chart.Paste();
            NewChart.Chart.ChartType = PackShape.Chart.ChartType.To<Microsoft.Office.Core.XlChartType>();
            return (IWordParagraphObj<ChartType>)NewChart.ToChart(Doc);
        }
        #endregion
        #endregion
        #region 关于构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Sheet">Excel形状所在的工作表</param>
        /// <param name="PackShape">被封装的Excel形状</param>
        /// <param name="Content">被封装的Office图表</param>
        public ExcelObjChart(IExcelSheet Sheet, Shape PackShape, ChartType Content)
            : base(Sheet, PackShape)
        {
            if (!PackShape.IsChart())
                throw new ArgumentException("指定的形状不包含图表");
            this.Content = Content;
        }
        #endregion
    }
}
