using System;
using System.Collections.Generic;
using System.Linq;
using System.Maths;
using System.Office.Chart;
using System.Office.Excel;
using System.Office.Excel.Chart;

using Microsoft.Office.Interop.Excel;

using EXRange = Microsoft.Office.Interop.Excel.Range;

namespace System
{
    /// <summary>
    /// 所有关于微软ExcelCOM组件的扩展方法全部放在这里
    /// </summary>
    static class ExtenMSExcel
    {
        #region 关于工作表
        #region 返回最后一张工作表
        /// <summary>
        /// 返回一个工作表集合的最后一张工作表
        /// </summary>
        /// <param name="sheets">待返回最后工作表的集合</param>
        /// <returns></returns>
        public static Worksheet Last(this Sheets sheets)
            => (Worksheet)sheets[sheets.Count];
        #endregion
        #endregion
        #region 关于单元格
        #region 返回单元格的开始和结束行列数
        /// <summary>
        /// 返回单元格的开始和结束行列数
        /// </summary>
        /// <param name="Range">待返回行列数的单元格</param>
        /// <returns></returns>
        public static (int BR, int BC, int ER, int EC) GetAddress(this EXRange Range)
        {
            var BR = Range.Row - 1;
            var BC = Range.Column - 1;
            return (BR, BC,
                BR + Range.Rows.Count - 1,
                BC + Range.Columns.Count - 1);
        }
        #endregion
        #region 返回指定索引处的单元格
        /// <summary>
        /// 返回一个单元格内部指定索引处的单元格
        /// </summary>
        /// <param name="Range">待返回子单元格的单元格</param>
        /// <param name="Index">子单元格的索引</param>
        /// <returns></returns>
        public static EXRange ElementAt(this EXRange Range, int Index)
            => Range.Cells.OfType<EXRange>().ElementAt(Index);
        #endregion
        #region 获取单元格地址
        /// <summary>
        /// 获取单元格地址的辅助方法
        /// </summary>
        /// <param name="Range">封装的单元格对象</param>
        /// <param name="IsR1C1">如果这个值为<see langword="true"/>，
        /// 代表以R1C1形式返回，否则代表以A1形式返回</param>
        /// <param name="IsComplete">如果这个值为<see langword="true"/>，
        /// 代表返回完整地址，它包含了关于所在文件等信息，否则返回标准形式的地址</param>
        /// <returns></returns>
        public static string GetAddressFull(this EXRange Range, bool IsR1C1 = true, bool IsComplete = false)
            => Range.Address[ReferenceStyle: IsR1C1 ? XlReferenceStyle.xlR1C1 : XlReferenceStyle.xlA1, External: IsComplete];
        #endregion
        #endregion
        #region 关于形状
        #region 获取形状集合中的所有形状
        /// <summary>
        /// 获取形状集合中的所有形状
        /// </summary>
        /// <param name="shapes">待获取元素的形状集合</param>
        /// <returns></returns>
        public static IEnumerable<Shape> GetShapes(this Shapes shapes)
            => shapes.OfType<Shape>();
        #endregion
        #region 关于形状的大小
        #region 获取形状的大小
        /// <summary>
        /// 获取形状的大小
        /// </summary>
        /// <param name="shape">待获取大小的形状</param>
        /// <returns></returns>
        public static ISize GetSize(this Shape shape)
            => CreateMathObj.Size(shape.Width, shape.Height);
        #endregion
        #region 写入形状的大小
        /// <summary>
        /// 写入形状的大小
        /// </summary>
        /// <param name="shape">待写入大小的形状</param>
        /// <param name="size">要写入的新大小</param>
        public static void SetSize(this Shape shape, ISize size)
        {
            var (Width, Height) = size;
            shape.Width = Width;
            shape.Height = Height;
        }
        #endregion
        #endregion
        #region 对形状的判断
        #region 判断图表
        /// <summary>
        /// 如果形状对象是图表，则返回<see langword="true"/>
        /// </summary>
        /// <param name="Shape">待判断的形状对象</param>
        /// <returns></returns>
        public static bool IsChart(this Shape Shape)
            => Shape.Type is Microsoft.Office.Core.MsoShapeType.msoChart;
        #endregion
        #region 判断图片
        /// <summary>
        /// 如果形状对象是图片，则返回<see langword="true"/>
        /// </summary>
        /// <param name="Shape">待判断的形状对象</param>
        /// <returns></returns>
        public static bool IsImage(this Shape Shape)
            => Shape.Type is Microsoft.Office.Core.MsoShapeType.msoPicture;
        #endregion
        #endregion
        #endregion
        #region 关于图表
        #region 将形状对象转换为Excel图表
        /// <summary>
        /// 将形状对象转换为Excel图表
        /// </summary>
        /// <param name="Shape">封装Excel图表的形状对象</param>
        /// <param name="Sheet">Excel图表所在的工作表</param>
        /// <exception cref="ArgumentException">该形状没有包含图表</exception>
        /// <exception cref="ArgumentException">图表的类型无法识别</exception>
        /// <returns></returns>
        public static IExcelObj<IOfficeChart> ToChart(this Shape Shape, IExcelSheet Sheet)
            => Shape.Chart.ChartType switch
            {
                XlChartType.xlLineMarkers or XlChartType.xlLine => new ExcelObjChart<IOfficeChartLine>(Sheet, Shape, new ExcelChartLine(Shape)),
                _ => new ExcelObjChart<IOfficeChart>(Sheet, Shape, new ExcelChartBase(Shape))
            };
        #endregion
        #endregion
    }
}
