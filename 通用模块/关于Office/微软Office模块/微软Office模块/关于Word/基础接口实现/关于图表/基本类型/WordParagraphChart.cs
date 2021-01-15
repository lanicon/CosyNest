using System;
using System.Collections.Generic;
using System.DataFrancis;
using System.Design;
using System.Maths;
using System.Office.Chart;
using System.Office.Excel;
using System.Office.Excel.Chart;
using System.Office.Word.Realize;
using System.Text;

using Microsoft.Office.Interop.Word;

using ChartType = Microsoft.Office.Core.XlChartType;

namespace System.Office.Word.Chart
{
    /// <summary>
    /// 这个类型代表一个封装了图表的Word段落
    /// </summary>
    /// <typeparam name="TChart">图表的类型</typeparam>
    class WordParagraphChart<TChart> : WordParagraphObjMicrosoft<TChart>
        where TChart : IOfficeChart
    {
        #region 封装的图表
        /// <summary>
        /// 获取被封装的图表，
        /// 本类型的功能就是通过它实现的
        /// </summary>
        private Microsoft.Office.Interop.Word.Chart PackChart
            => PackShape.Chart;
        #endregion
        #region Word对象的内容
        public override TChart Content { get; }
        #endregion
        #region 复制Word图表
        #region 复制到工作表
        public override IExcelObj<TChart> Copy(IExcelSheet Target)
        {
            var NewChart = Target.To<ExcelSheetMicrosoft>().PackSheet.Shapes.AddChart2();
            PackChart.Copy();
            NewChart.Chart.Paste();
            NewChart.Chart.ChartType = PackChart.ChartType.To<Microsoft.Office.Interop.Excel.XlChartType>();
            return (IExcelObj<TChart>)NewChart.ToChart(Target);
        }
        #endregion
        #region 复制到文档
        public override IWordParagraphObj<TChart> Copy(IWordDocument Target, Index? Pos = null)
        {
            var Doc = Target.To<WordDocumentMicrosoft>();
            var Index = Doc.ToUnderlying(Doc.ToIndexActual(Pos), false);
            var PackDoc = Doc.PackDocument;
            var NewChart = PackDoc.InlineShapes.AddChart2(Range: PackDoc.Range(Index));
            PackChart.Copy();
            NewChart.Chart.Paste();
            NewChart.Chart.ChartType = PackChart.ChartType;
            return (IWordParagraphObj<TChart>)NewChart.ToChart(Doc);
        }
        #endregion
        #endregion
        #region 构造函数与创建对象
        /// <summary>
        /// 使用指定的文档和形状初始化段落
        /// </summary>
        /// <param name="doc">这个段落所在的文档</param>
        /// <param name="PackShape">这个段落所封装的形状对象</param>
        /// <param name="Content">Word对象所封装的图表</param>
        public WordParagraphChart(WordDocument doc, InlineShape PackShape, TChart Content)
            : base(doc, PackShape)
        {
            if (PackShape.HasChart != Microsoft.Office.Core.MsoTriState.msoTrue)
                throw new Exception("此形状没有包含图表");
            this.Content = Content;
        }
        #endregion
    }
}
