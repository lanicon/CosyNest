using System;
using System.Collections.Generic;
using System.Text;
using System.Office;
using System.Office.Word.Realize;
using System.DrawingFrancis.Text;
using System.Linq;
using WordRange = Microsoft.Office.Interop.Word.Range;
using Microsoft.Office.Interop.Word;

namespace System.Office.Word
{
    /// <summary>
    /// 这个类型代表由微软COM组件实现的Word范围
    /// </summary>
    class WordRangeMicrosoft : Realize.WordRange
    {
        #region 封装的对象
        /// <summary>
        /// 获取封装的Range对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        internal WordRange PackRange { get; private set; }
        #endregion
        #region 获取或设置超链接
        public override string? Link
        {
            get
            {
                var Hyperlinks = PackRange.Hyperlinks;
                if (Hyperlinks.Count != 1)
                    return null;
                var link = Hyperlinks[1];
                return link.Range.Text == Text ? link.Address : null;
            }
            set
            {
                var oldText = Text;
                var Hyperlinks = PackRange.Hyperlinks;
                Hyperlinks.OfType<Hyperlink>().ForEach(x => x.Delete());
                if (value != null)
                {
                    var NewLink = Hyperlinks.Add(PackRange, value, TextToDisplay: oldText ?? value);
                    PackRange = NewLink.Range;
                }
            }
        }
        #endregion
        #region 范围的文本
        public override string Text
        {
            get => PackRange.Text;
            set
            {
                var old = Text;
                PackRange.Text = value;
                Document.To<WordDocument>().
                    CallLengthChange(Range.Begin.Pos, old, value);
            }
        }
        #endregion
        #region 范围的文本格式
        #region 返回是否具有多种样式
        /// <summary>
        /// 如果这个值为<see langword="true"/>，
        /// 代表本范围内的文本具有多种样式
        /// </summary>
        private bool MultipleStyles { get; set; }
        #endregion
        #region 正式属性
        private ITextStyleVar? StyleField;

        public override ITextStyleVar Style
        {
            get => MultipleStyles ? OfficeTextStyleCom.Multiple : StyleField ??= new WordTextStyle(PackRange);
            set
            {
                if (value == null || value == OfficeTextStyleCom.Multiple)
                    throw new Exception($"本属性禁止写入null值或{nameof(OfficeTextStyleCom.Multiple)}");
                StyleField ??= new WordTextStyle(PackRange);
                OfficeTextStyleCom.CacheStylePro.ForEach(pro => pro.Copy(value, StyleField));
                MultipleStyles = false;
            }
        }
        #endregion 
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Document">这个片段所隶属的段落</param>
        /// <param name="Range">这个片段所封装的Range</param>
        /// <param name="NeedCheckStyle">如果这个值为真，代表需要检查样式的一致性，把这个值设为假可以节约性能</param>
        public WordRangeMicrosoft(WordDocumentMicrosoft Document, WordRange Range, bool NeedCheckStyle)
            : base(Document,
                  Document.Interface.GetBookmark(Document.FromUnderlying(Range.Start, true)),
                  Document.Interface.GetBookmark(Document.FromUnderlying(Range.End, false)))
        {
            PackRange = Range;
            MultipleStyles = NeedCheckStyle && !Range.IsConsistent();
        }
        #endregion
    }
}
