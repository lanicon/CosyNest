﻿using System.Office.Chart;
using System.Office.Word.Realize;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;

namespace System.Office.Word.Chart
{
    /// <summary>
    /// 这个对象可以帮助创建Word图表
    /// </summary>
    class CreateChartWordMicrosoft : CreateChartWord
    {
        #region 封装的对象
        #region 获取所在的文档
        /// <summary>
        /// 获取这个图表创建器所在的文档
        /// </summary>
        private WordDocumentMicrosoft PackDocument { get; }
        #endregion
        #region 获取形状集合
        /// <summary>
        /// 获取文档的形状集合，本对象的功能就是通过它实现的
        /// </summary>
        private InlineShapes PackShapes
            => PackDocument.PackDocument.InlineShapes;
        #endregion
        #endregion
        #region 创建折线图
        public override IWordParagraphObj<IOfficeChartLine> CreateLineChart()
        {
            var NewChart = PackShapes.AddChart2(Type: XlChartType.xlLine);
            return (IWordParagraphObj<IOfficeChartLine>)NewChart.ToChart(PackDocument);
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的文档初始化对象
        /// </summary>
        /// <param name="Document">图形创建器所在的文档</param>
        public CreateChartWordMicrosoft(WordDocumentMicrosoft Document)
        {
            PackDocument = Document;
        }
        #endregion
    }
}
