using System.Office.Excel;

namespace System.DataFrancis.Excel.Block
{
    /// <summary>
    /// 这个类型封装块属性的读写方式
    /// </summary>
    class BlockProperty : IBlockProperty
    {
        #region 读取属性
        public Func<IExcelCells, object?> GetValue { get; }
        #endregion
        #region 写入属性
        public Action<IExcelCells, object?> SetValue { get; }
        #endregion
        #region 写入标题
        public Action<IExcelCells>? SetTitle { get; }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="GetValue">用来读取属性的委托</param>
        /// <param name="SetValue">用来写入属性的委托</param>
        /// <param name="SetTitle">用来写入标题的委托</param>
        public BlockProperty(Func<IExcelCells, object?> GetValue, Action<IExcelCells, object?> SetValue, Action<IExcelCells>? SetTitle = null)
        {
            this.GetValue = GetValue;
            this.SetValue = SetValue;
            this.SetTitle = SetTitle;
        }
        #endregion
    }
}
