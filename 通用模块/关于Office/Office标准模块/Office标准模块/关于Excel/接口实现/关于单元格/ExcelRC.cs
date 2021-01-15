using System.Collections;
using System.Collections.Generic;

namespace System.Office.Excel.Realize
{
    /// <summary>
    /// 在实现<see cref="IExcelRC"/>时，
    /// 可以继承自本类型，以减少重复的工作
    /// </summary>
    public abstract class ExcelRC : ExcelRange, IExcelRC
    {
        #region 返回对象是否为行
        public bool IsRow { get; }
        #endregion
        #region 关于行或者列的位置和规模
        #region 返回开始和结束行列数
        public (int Begin, int End) Range { get; }
        #endregion
        #endregion
        #region 关于迭代器
        public virtual IEnumerator<IExcelRC> GetEnumerator()
        {
            #region 本地函数
            IEnumerable<IExcelRC> Get()
            {
                var (B, E) = Range;
                for (; B <= E; B++)
                {
                    yield return Sheet.GetRC(B, B, IsRow);
                }
            }
            #endregion 
            return Get().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
        #endregion
        #region 关于行与列的样式
        #region 隐藏和取消隐藏
        public abstract bool? IsHide { get; set; }
        #endregion
        #region 获取或设置高度或宽度
        public abstract double? HeightOrWidth { get; set; }
        #endregion
        #region 自动调整行高与列宽
        public abstract void AutoFit();
        #endregion
        #endregion
        #region 删除行或者列
        public abstract void Delete();
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Sheet">这个单元格行列所在的工作表</param>
        /// <param name="IsRow">如果这个值为<see langword="true"/>，
        /// 代表这个对象是行，否则代表这个对象是列</param>
        /// <param name="Begin">开始行号或列号</param>
        /// <param name="End">结束行号或列号</param>
        public ExcelRC(IExcelSheet Sheet, bool IsRow, int Begin, int End)
            : base(Sheet)
        {
            this.IsRow = IsRow;
            this.Range = (Begin, End);
        }
        #endregion
    }
}
