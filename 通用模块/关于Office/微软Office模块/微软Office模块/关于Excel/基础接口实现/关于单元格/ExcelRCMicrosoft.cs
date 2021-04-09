using System.Linq;
using System.Office.Excel.Realize;

using ExcelRange = Microsoft.Office.Interop.Excel.Range;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个类型是由微软COM组件实现的Excel行列
    /// </summary>
    class ExcelRCMicrosoft : ExcelRC
    {
        #region 封装的单元格
        /// <summary>
        /// 获取封装的单元格，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        internal ExcelRange PackRange { get; }
        #endregion
        #region 关于行与列的样式
        #region 隐藏和取消隐藏
        public override bool? IsHide
        {
            get
            {
                var (First, Other, _) = PackRange.OfType<ExcelRange>().Select(x => (object)x.Hidden).First(true);
                foreach (var item in Other)
                {
                    if (!Equals(First, item))
                        return null;
                }
                return (bool)First;
            }
            set => PackRange.Hidden = value is null ?
                throw new NotSupportedException($"{nameof(IsHide)}禁止写入null值") :
                value.Value;
        }
        #endregion
        #region 获取或设置高度或宽度
        public override double? HeightOrWidth
        {
            get => (double?)(IsRow ? PackRange.RowHeight : PackRange.ColumnWidth);
            set
            {
                if (value == null)
                    throw new NullReferenceException("禁止写入null值");
                if (IsRow)
                    PackRange.RowHeight = value.Value;
                else PackRange.ColumnWidth = value.Value;
            }
        }
        #endregion
        #region 设置或获取样式
        private IRangeStyle? StyleField;

        public override IRangeStyle Style
        {
            get => StyleField ??= new RangeStyleMicrosoft(PackRange);
            set => ExcelRealize.CopyStyle(value, Style);
        }
        #endregion
        #region 自动调整行高与列宽
        public override void AutoFit()
        {
            foreach (var item in (IsRow ? PackRange.Rows : PackRange.Columns).OfType<ExcelRange>())
            {
                item.AutoFit();
            }
        }
        #endregion
        #endregion
        #region 返回单元格地址
        public override string AddressText(bool isR1C1 = true, bool isComplete = false)
            => PackRange.GetAddressFull(isR1C1, isComplete);
        #endregion
        #region 删除行或者列
        public override void Delete()
            => PackRange.Delete();
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Sheet">范围所在的工作表</param>
        /// <param name="PackRange">被封装的范围</param>
        /// <param name="IsRow">如果这个值为<see langword="true"/>，
        /// 代表这个对象是行，否则是列</param>
        /// <param name="Begin">开始行列号</param>
        /// <param name="End">结束行列号</param>
        public ExcelRCMicrosoft(IExcelSheet Sheet, ExcelRange PackRange, bool IsRow, int Begin, int End)
            : base(Sheet, IsRow, Begin, End)
        {
            this.PackRange = IsRow ? PackRange.Rows : PackRange.Columns;
        }
        #endregion
    }
}
