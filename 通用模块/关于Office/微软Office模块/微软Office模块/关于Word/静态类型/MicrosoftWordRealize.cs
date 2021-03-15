using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Word;
using WordRange = Microsoft.Office.Interop.Word.Range;
using System.Maths;
using System.Office.Word.Realize;
using System.Office.Chart;
using Microsoft.Office.Core;
using System.Office.Word.Chart;

namespace System.Office.Word
{
    /// <summary>
    /// 这个类型为实现微软Word模块提供帮助
    /// </summary>
    static class MicrosoftWordRealize
    {
        #region 关于Range
        #region 将范围按照文本样式拆分
        /// <summary>
        /// 将范围按照文本样式拆分成若干部分，
        /// 每个部分的都具有相同的文本格式
        /// </summary>
        /// <param name="Range">待拆分的范围</param>
        /// <returns></returns>
        public static IEnumerable<WordRange> SplitFromStyle(this WordRange Range)
        {
            var End = Range.End - 1;
            var Chars = Range.Characters.ToArray<WordRange>();
            var R = Chars[0];                                   //这个变量用来保存待迭代的Range
            if (Chars.Length < 3)
            {
                yield return R;
                yield break;
            }
            var Style = new WordTextStyle(Range);
            foreach (var item in Chars[1..^1])              //跳过片段的开头和最后部分，因为开头已经遍历过，而末尾一定是不可见的换行符
            {
                var NewStyle = new WordTextStyle(item);
                if (Style.Equals(NewStyle) && R.Hyperlinks.Count == item.Hyperlinks.Count)      //如果待迭代的Range与当前字符格式一样
                    R.MoveEnd();                   //则将它扩展一个字符
                else
                {
                    yield return R;      //如果格式不同，则将Range迭代
                    R = item;
                    Style = NewStyle;
                }
                if (R.End == End)                   //如果遍历到了片段的末尾，则立即进行一次迭代，并退出
                {
                    yield return R;
                    break;
                }
            }
        }
        #endregion
        #region 返回范围是否具有一致的格式
        /// <summary>
        /// 返回范围是否具有一致的格式，
        /// 也就是它的每一部分文本都格式相同
        /// </summary>
        /// <param name="Range">待判断的范围</param>
        /// <returns></returns>
        public static bool IsConsistent(this WordRange Range)
        {
            var Style = new WordTextStyle(Range.Characters[1]);
            return !Range.SplitFromStyle().Any(x => !Style.Equals(new WordTextStyle(x)));
        }
        #endregion
        #endregion
        #region 关于格式
        #region 用于映射对齐方式的字典
        /// <summary>
        /// 获取一个用于双向映射对齐方式的字典
        /// </summary>
        public static ITwoWayMap<OfficeAlignment, WdParagraphAlignment> MapAlignment { get; }
        = CreateEnumerable.TwoWayMap(
            (OfficeAlignment.Center, WdParagraphAlignment.wdAlignParagraphCenter),
            (OfficeAlignment.LeftOrTop, WdParagraphAlignment.wdAlignParagraphLeft),
            (OfficeAlignment.RightOrBottom, WdParagraphAlignment.wdAlignParagraphRight),
            (OfficeAlignment.Ends, WdParagraphAlignment.wdAlignParagraphJustify));
        #endregion
        #endregion
        #region 关于形状
        #region 关于形状的大小
        #region 获取形状的大小
        /// <summary>
        /// 获取形状的大小
        /// </summary>
        /// <param name="shape">待获取大小的形状</param>
        /// <returns></returns>
        public static ISize GetSize(this InlineShape shape)
            => CreateMathObj.Size(shape.Width, shape.Height);
        #endregion
        #region 写入形状的大小
        /// <summary>
        /// 写入形状的大小
        /// </summary>
        /// <param name="shape">待写入大小的形状</param>
        /// <param name="size">要写入的新大小</param>
        public static void SetSize(this InlineShape shape, ISize size)
        {
            var (Width, Height) = size;
            shape.Width = Width;
            shape.Height = Height;
        }
        #endregion
        #endregion
        #region 判断是否为图表
        /// <summary>
        /// 如果形状对象是图表，则返回<see langword="true"/>
        /// </summary>
        /// <param name="Shape">待判断的形状对象</param>
        /// <returns></returns>
        public static bool IsChart(this InlineShape Shape)
            => Shape.HasChart is MsoTriState.msoTrue;
        #endregion
        #endregion
        #region 关于图表
        #region 将形状对象转换为Word图表
        /// <summary>
        /// 将形状对象转换为Word图表
        /// </summary>
        /// <param name="Shape">封装Word图表的形状对象</param>
        /// <param name="Document">Word图表所在的文档</param>
        /// <exception cref="ArgumentException">该形状没有包含图表</exception>
        /// <exception cref="ArgumentException">图表的类型无法识别</exception>
        /// <returns></returns>
        public static IWordParagraphObj<IOfficeChart> ToChart(this InlineShape Shape, WordDocument Document)
            => Shape.Chart.ChartType switch
            {
                XlChartType.xlLineMarkers or XlChartType.xlLine => new WordParagraphChart<IOfficeChartLine>(Document, Shape, new WordChartLine(Shape)),
                _ => new WordParagraphChart<IOfficeChart>(Document, Shape, new WordChartBase(Shape))
            };
        #endregion
        #endregion
        #region 静态构造函数
        static MicrosoftWordRealize()
        {
            MapAlignment.RegisteredOne(OfficeAlignment.Ends,
                WdParagraphAlignment.wdAlignParagraphJustifyHi,
                WdParagraphAlignment.wdAlignParagraphJustifyLow,
                WdParagraphAlignment.wdAlignParagraphJustifyMed,
                WdParagraphAlignment.wdAlignParagraphThaiJustify);
        }
        #endregion
    }
}
