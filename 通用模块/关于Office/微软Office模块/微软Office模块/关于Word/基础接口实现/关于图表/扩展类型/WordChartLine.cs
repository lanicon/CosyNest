using System.Office.Chart;

using Microsoft.Office.Interop.Word;

namespace System.Office.Word.Chart
{
    /// <summary>
    /// 这个类型代表一个Word折线图
    /// </summary>
    class WordChartLine : WordChartBase, IOfficeChartLine
    {
        #region 构造函数
        /// <summary>
        /// 使用指定的形状初始化段落
        /// </summary>
        /// <param name="PackShape">这个段落所封装的形状对象</param>
        public WordChartLine(InlineShape PackShape)
            : base(PackShape)
        {

        }
        #endregion
    }
}
