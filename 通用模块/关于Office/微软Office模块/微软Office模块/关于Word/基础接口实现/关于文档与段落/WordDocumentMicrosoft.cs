using System.Collections.Generic;
using System.DrawingFrancis;
using System.IO;
using System.IOFrancis.FileSystem;
using System.Linq;
using System.Office.Word.Chart;
using System.Office.Word.Realize;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;

namespace System.Office.Word
{
    /// <summary>
    /// 这个类型代表由微软COM组件实现的Word文档
    /// </summary>
    class WordDocumentMicrosoft : WordDocument, IWordDocument
    {
        #region 封装的对象
        #region 封装的Application
        /// <summary>
        /// 获取封装的Application对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        internal Application PackApp
            => PackDocument.Application;
        #endregion
        #region 封装的Word文档
        /// <summary>
        /// 返回Application对象的唯一一个文档
        /// </summary>
        internal Document PackDocument { get; }

        //根据设计，每个WordDocument对象独占一个Word文档
        #endregion
        #endregion
        #region 关于文档
        #region 释放文档
        protected override void DisposeOfficeRealize()
        {
#if DEBUG
            if (FromActive)
                return;
#endif
            PackApp.DisplayAlerts = WdAlertLevel.wdAlertsAll;
            PackDocument.Close();
            PackApp.Quit();
        }
        #endregion
        #region 保存文档
        protected override void SaveRealize(string Path, bool isSitu)
        {
            if (isSitu)
            {
                if (!PackDocument.Saved)
                    PackDocument.Save();
            }
            else PackDocument.SaveAs(Path);
        }
        #endregion
        #region 返回页面对象
        private IPage? PageField;
        public override IPage Page
            => PageField ??= new WordPage(PackDocument);
        #endregion
        #region 枚举所有段落
        public override IEnumerable<IWordParagraph> Paragraphs
        {
            get
            {
                var Paragraphs = PackDocument.Paragraphs.OfType<Paragraph>().Select(x => ((object)x, x.Range.Start));
                var Shapes = PackDocument.InlineShapes.OfType<InlineShape>().Select(x => ((object)x, x.Range.Start));
                return Paragraphs.Union(Shapes).Sort(x => x.Start).Select(x => x.Item1 switch
                {
                    Paragraph p => new WordParagraphTextMicrosoft(this, p),
                    InlineShape { HasChart: MsoTriState.msoTrue } s => s.ToChart(this),
                    _ => (IWordParagraph?)null
                }).Where(x => x is { })!;
            }
        }
        #endregion
        #endregion
        #region 关于文本
        #region 返回无格式文本
        public override string Text
             => PackDocument.Content.Text;
        #endregion
        #region 插入文本
        public override IWordRange CreateWordRange(string Text, Index? Begin = null)
        {
            var Pos = ToIndexActual(Begin);
            var Range = PackDocument.Range(ToUnderlying(Pos, false));
            Range.Text += Text;
            if (Range.Text.Trim().Length != Text.Trim().Length)               //如果Text就是文档的全部内容，则不移动开始位置
                Range.Start++;
            CallLengthChange(Pos, NewText: Text);
            return new WordRangeMicrosoft(this, Range, false);
        }
        #endregion
        #region 插入段落
        public override IWordParagraphText CreateParagraph(string Text, Index? Begin = null)
        {
            var Pos = ToIndexActual(Begin) + 1;     //由于Paragraphs.Add()方法是将段落添加到范围前面
            Paragraph Paragraph;                    //但根据规范，新文本应该出现在段落后面，所以位置应该加1
            if (Pos == Length)
            {
                Paragraph = PackDocument.Paragraphs.Add();
            }
            else
            {
                var Range = PackDocument.Range(ToUnderlying(Pos, false));
                Paragraph = PackDocument.Paragraphs.Add(Range);
            }
            Paragraph.Range.Text += Text;
            CallLengthChange(Pos..(Pos + 1), Text.Length + 1);      //由于新段落的文本自带一个换行符，所以实际增加的文本应加1
            return new WordParagraphTextMicrosoft(this, Paragraph);
        }
        #endregion
        #region 获取Word范围
        public override IWordRange this[Range Range]
        {
            get
            {
                var (B, E) = Range.GetOffsetAndEnd(Length);
                return new WordRangeMicrosoft(this,
                    PackDocument.Range(ToUnderlying(B, true), ToUnderlying(E, false)), true);
            }
        }
        #endregion
        #endregion
        #region 关于位置
        #region 将索引计算为实际位置
        /// <summary>
        /// 将索引计算为实际位置
        /// </summary>
        /// <param name="index">待计算的索引，
        /// 如果为<see langword="null"/>，默认为^1</param>
        /// <returns></returns>
        public int ToIndexActual(Index? index)
            => (index == null || index.Value.Equals(^0) ? ^1 : index.Value).GetOffset(Length);
        #endregion
        #region 获取文档的长度
        public override int Length
            => Text.Length;

        /*说明文档：
          如果文档中存在InlineShape对象，
          则Word会在文本中该对象的位置加上"\"作为占位符，
          因此文本的字数和文档的长度是相同的
          PS：这个设计非常怪异*/
        #endregion
        #region 枚举非文本段落的索引
        protected override IEnumerable<WordPos> NotTextIndex
        {
            get
            {
                var arry = PackDocument.InlineShapes.OfType<object>().
                    Union(PackDocument.Hyperlinks.OfType<object>()).
                    Select(x => x switch
                    {
                        InlineShape s => ((object)s, s.Range.Start, s.Range.End),
                        Hyperlink h => (h, h.Range.Start, h.Range.End),
                        _ => default
                    }).Sort(x => x.End);
                return arry.AggregateSelect(new WordPos(default, default, default), (range, seed) =>
                  {
                      var ((_, TextBegin), (_, ActualBegin), (_, LastUnderlyingEnd)) = seed;
                      var (Range, UnderlyingBegin, UnderlyingEnd) = range;
                      var Poor = UnderlyingBegin - LastUnderlyingEnd;
                      TextBegin += Poor;
                      ActualBegin += Poor;
                      var (TextEnd, ActualEnd) = Range switch
                      {
                          InlineShape i => (TextBegin, ActualBegin + 1),
                          Hyperlink h => (TextBegin + h.Range.Text.Length, ActualBegin + h.Range.Text.Length),
                          _ => default
                      };
                      var Ret = new WordPos((TextBegin, TextEnd), (ActualBegin, ActualEnd), (UnderlyingBegin, UnderlyingEnd));
                      return (Ret, Ret);
                  });
            }
        }
        #endregion
        #endregion
        #region 关于Office对象
        #region 关于图表
        #region 返回图表创建器
        private ICreateWordChart? CreateChartField;
        public override ICreateWordChart CreateChart
            => CreateChartField ??= new CreateChartWordMicrosoft(this);
        #endregion
        #endregion
        #region 关于图片
        #region 创建图片
        public override IWordParagraphObj<IImage> CreateImage(IImage Image, Index? Pos = null)
        {
            var range = PackDocument.Range(ToUnderlying(ToIndexActual(Pos), false));
            var path = MSOfficeRealize.SaveImage(Image);
            var shape = PackDocument.InlineShapes.AddPicture(path.Path, Range: range);
            return new WordParagraphImageMicrosoft(this, shape);
        }
        #endregion
        #endregion
        #endregion
        #region 构造函数
        #region 供调试使用的成员
#if DEBUG
        #region 指示Word对象的创建方式
        /// <summary>
        /// 如果这个值为真，
        /// 代表这个对象是从已经打开的Word窗口中加载的，
        /// 否则代表它是通过程序创建的
        /// </summary>
        private bool FromActive { get; }
        #endregion
        #region 构造函数：指定文档
        /// <summary>
        /// 使用指定的文档初始化对象
        /// </summary>
        /// <param name="doc">指定的文档</param>
        public WordDocumentMicrosoft(Document doc)
            : base(doc.FullName, CreateMSOffice.SupportWord)
        {
            PackDocument = doc;
            PackApp.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            FromActive = true;
        }
        #endregion
#endif
        #endregion
        #region 指定路径
        /// <summary>
        /// 从指定的路径初始化Word文档
        /// </summary>
        /// <param name="path">文档所在的路径，如果为<see langword="null"/>，代表尚未保存在文件中</param>
        public WordDocumentMicrosoft(PathText? path = null)
            : base(path, CreateMSOffice.SupportWord)
        {
            var Word = new Application()
            {
                Visible = false,
                DisplayAlerts = WdAlertLevel.wdAlertsNone,
                FileValidation = MsoFileValidationMode.msoFileValidationSkip
            };
            var docs = Word.Documents;
            PackDocument = path == null || !File.Exists(path) ?
                docs.Add() : docs.Open(path.Path);
        }
        #endregion 
        #endregion
    }
}
