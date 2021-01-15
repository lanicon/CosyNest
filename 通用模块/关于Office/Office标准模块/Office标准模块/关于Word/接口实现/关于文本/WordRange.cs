using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.DrawingFrancis.Text;

namespace System.Office.Word.Realize
{
    /// <summary>
    /// 在实现<see cref="IWordRange"/>的时候可以继承自本类型，
    /// 以减少重复的工作
    /// </summary>
    public abstract class WordRange : IWordRange
    {
        #region 范围所属的文档
        public IWordDocument Document { get; }
        #endregion
        #region 范围的开始和结束
        public (IWordBookmark Begin, IWordBookmark End) Range { get; }
        #endregion
        #region 获取或设置超链接
        public abstract string? Link { get; set; }
        #endregion
        #region 关于文本和格式
        #region 范围的文本
        public abstract string Text { get; set; }
        #endregion
        #region 范围的文本格式
        public abstract ITextStyleVar Style { get; set; }
        #endregion
        #endregion
        #region 重写ToString
        public override string ToString()
            => Text;
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的文档，开始位置和结束位置初始化范围
        /// </summary>
        /// <param name="Document">范围所属的文档</param>
        /// <param name="Begin">范围的开始位置</param>
        /// <param name="End">范围的结束位置</param>
        public WordRange(IWordDocument Document, IWordBookmark Begin, IWordBookmark End)
        {
            this.Document = Document;
            Range = (Begin, End);
        }
        #endregion
    }
}
