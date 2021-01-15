using System;
using System.Collections.Generic;
using System.Text;

namespace System.Office.Excel.Realize
{
    /// <summary>
    /// 在实现<see cref="IExcelRange"/>时，
    /// 可以继承自本类型，以减少重复的工作
    /// </summary>
    public abstract class ExcelRange : IExcelRange
    {
        #region 获取单元格所在的工作表
        public IExcelSheet Sheet { get; }
        #endregion
        #region 设置或获取样式
        public abstract IRangeStyle Style { get; set; }
        #endregion
        #region 返回单元格地址
        public abstract string AddressText(bool IsR1C1 = true, bool IsComplete = false);
        #endregion
        #region 重写的ToString
        public sealed override string ToString()
            => AddressText();
        #endregion
        #region 构造函数
        /// <summary>
        /// 用指定的工作表初始化对象
        /// </summary>
        /// <param name="Sheet">这个范围所在的工作表</param>
        public ExcelRange(IExcelSheet Sheet)
        {
            this.Sheet = Sheet;
        }
        #endregion
    }
}
