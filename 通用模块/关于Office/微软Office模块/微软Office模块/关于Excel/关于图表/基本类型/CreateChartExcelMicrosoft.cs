﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Office.Excel.Realize;
using Microsoft.Office.Interop.Excel;
using System.DrawingFrancis.Chart;
using System.Office.Excel.Chart;
using System.Office.Realize;
using System.Office.Chart;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个类型可以帮助创建Excel图表
    /// </summary>
    class CreateChartExcelMicrosoft : CreateChartExcel
    {
        #region 创建图表所需要的信息
        #region 封装的图形集合
        /// <summary>
        /// 获取封装的图形集合，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private Shapes PackShapes
            => Sheet.PackSheet.Shapes;
        #endregion
        #region 图表创建器所在的工作表
        /// <summary>
        /// 图表创建器所在的工作表
        /// </summary>
        private ExcelSheetMicrosoft Sheet { get; }
        #endregion
        #endregion
        #region 创建图表
        #region 辅助方法
        /// <summary>
        /// 指定图表的类型，并创建图表
        /// </summary>
        /// <typeparam name="ChartType">图表的类型</typeparam>
        /// <param name="Type">COM组件所能识别的图表类型</param>
        /// <returns></returns>
        private IExcelObj<ChartType> CreateChart<ChartType>(XlChartType Type)
            where ChartType : IOfficeChart
            => (IExcelObj<ChartType>)PackShapes.AddChart2(XlChartType: Type).ToChart(Sheet);
        #endregion
        #region 创建折线图
        public override IExcelObj<IOfficeChartLine> CreateLineChart()
            => CreateChart<IOfficeChartLine>(XlChartType.xlLine);
        #endregion
        #endregion 
        #region 构造函数
        /// <summary>
        /// 使用指定的图形集合初始化对象
        /// </summary>
        /// <param name="Sheet">图表创建器所在的工作表</param>
        public CreateChartExcelMicrosoft(ExcelSheetMicrosoft Sheet)
        {
            this.Sheet = Sheet;
        }
        #endregion
    }
}
