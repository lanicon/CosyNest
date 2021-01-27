using System.Collections.Generic;
using System.DrawingFrancis;
using System.IOFrancis;
using System.Linq;
using System.Office.Realize;
using System.Performance;

namespace System.Office.Word.Realize
{
    /// <summary>
    /// 在实现<see cref="IWordDocument"/>的时候，
    /// 可以继承自本类型，以减少重复的工作
    /// </summary>
    public abstract class WordDocument : OfficeFile, IWordDocument
    {
        #region 返回IWordDocument接口
        /// <summary>
        /// 返回这个对象的<see cref="IWordDocument"/>接口形式，
        /// 使其可以调用显式实现的成员
        /// </summary>
        public IWordDocument Interface
            => this;
        #endregion
        #region 关于文件与文档
        #region 返回页面对象
        public abstract IPage Page { get; }
        #endregion
        #region 枚举所有段落
        public abstract IEnumerable<IWordParagraph> Paragraphs { get; }
        #endregion
        #endregion
        #region 关于文本
        #region 返回无格式文本
        public abstract string Text { get; }
        #endregion
        #region 插入文本
        public abstract IWordRange CreateWordRange(string Text, Index? Begin = null);
        #endregion
        #region 插入段落
        public abstract IWordParagraphText CreateParagraph(string Text, Index? Begin = null);
        #endregion
        #region 获取Word范围
        public abstract IWordRange this[Range Range] { get; }
        #endregion
        #endregion
        #region 关于位置
        #region 获取文档的长度
        public abstract int Length { get; }
        #endregion
        #region 枚举非文本段落的索引
        /// <summary>
        /// 这个集合枚举文档中非文本部分的索引，
        /// 按照规范，它的元素应该按底层实现位置升序排序
        /// </summary>
        protected abstract IEnumerable<WordPos> NotTextIndex { get; }
        #endregion
        #region 有关位置转换
        #region 将文本位置转换为实际位置
        public int ToIndexActual(int IndexText)
        {
            var Close = NotTextIndex.LastOrDefault(x => IndexText >= x.IndexText.Begin);
            if (Close == null)
                return IndexText;
            var ((_, TE), (_, AE), _) = Close;
            return IndexText <= TE ? AE : IndexText + AE - TE;
        }
        #endregion
        #region 将实际位置转换为文本位置
        public int ToIndexText(int IndexActual)
        {
            var Close = NotTextIndex.LastOrDefault(x => IndexActual >= x.IndexActual.Begin);
            if (Close == null)
                return IndexActual;
            var ((_, TE), (_, AE), _) = Close;
            return IndexActual <= AE ? TE : IndexActual - (AE - TE);
        }
        #endregion
        #region 将实际位置转换为底层实现位置
        /// <summary>
        /// 将实际位置转换为底层实现位置
        /// </summary>
        /// <param name="IndexActual">待转换的实际位置</param>
        /// <param name="GetBegin">当实际位置位于一个Office对象的底层实现位置之间的时候，
        /// 如果这个参数为<see langword="true"/>，取底层实现位置的开始，否则取结束</param>
        /// <returns></returns>
        public virtual int ToUnderlying(int IndexActual, bool GetBegin)
        {
            var Close = NotTextIndex.LastOrDefault(x => IndexActual >= x.IndexActual.Begin);
            if (Close == null)
                return IndexActual;
            var (_, (_, AE), (UB, UE)) = Close;
            return IndexActual <= AE ?
                (GetBegin ? UB : UE) : UE + IndexActual - AE;
        }
        #endregion
        #region 将底层实现位置转换为实际位置
        /// <summary>
        /// 将底层实现位置转换为实际位置
        /// </summary>
        /// <param name="IndexUnderlying">待转换的底层实现位置</param>
        /// <param name="GetBegin">当底层实现位置位于一个Office对象的实际位置之间的时候，
        /// 如果这个参数为<see langword="true"/>，取实际位置的开始，否则取结束</param>
        /// <returns></returns>
        public virtual int FromUnderlying(int IndexUnderlying, bool GetBegin)
        {
            var Close = NotTextIndex.LastOrDefault(x => IndexUnderlying >= x.IndexUnderlying.Begin);
            if (Close == null)
                return IndexUnderlying;
            var (_, (AB, AE), (_, UE)) = Close;
            return IndexUnderlying <= UE ?
                (GetBegin ? AB : AE) : IndexUnderlying - (UE - AE);
        }
        #endregion
        #endregion
        #endregion
        #region 关于事件
        #region 长度被改变时引发的事件
        #region 调用事件
        #region 指定位置，新文本和旧文本
        /// <summary>
        /// 调用<see cref="LengthChange"/>事件，通知长度已更改
        /// </summary>
        /// <param name="Pos">发生修改的位置</param>
        /// <param name="OldText">修改前的旧文本，如果为<see langword="null"/>，代表是添加新文本</param>
        /// <param name="NewText">被修改后的新文本，如果为<see langword="null"/>，代表删除旧文本</param>
        public void CallLengthChange(int Pos, string? OldText = null, string? NewText = null)
        {
            var NewLen = NewText?.Length ?? 0;
            var OldLen = OldText?.Length ?? 0;
            CallLengthChange(Pos..(Pos + OldLen + 1), NewLen - OldLen);
        }
        #endregion
        #region 指定位置和改变的长度
        /// <summary>
        /// 调用<see cref="LengthChange"/>事件，通知长度已更改
        /// </summary>
        /// <param name="Range">发生修改的范围</param>
        /// <param name="Change">指示执行完改变以后，总长度变更了多少</param>
        public void CallLengthChange(Range Range, int Change)
        {
            if (LengthChangeWeak != null && Change != 0)
                LengthChangeWeak.DynamicInvoke(Range, Change);
        }
        #endregion
        #endregion
        #region 弱事件封装
        /// <summary>
        /// <see cref="LengthChange"/>的弱事件封装，
        /// 当文档的字数被改变时，触发这个事件
        /// </summary>
        private WeakDelegate<Action<Range, int>>? LengthChangeWeak;
        #endregion
        #region 正式事件
        public event Action<Range, int> LengthChange
        {
            add => ToolPerfo.AddWeakDel(ref LengthChangeWeak, value);
            remove => LengthChangeWeak?.Remove(value);
        }
        #endregion
        #endregion
        #endregion
        #region 关于Office对象
        #region 返回图表创建器
        public abstract ICreateWordChart CreateChart { get; }
        #endregion
        #region 创建图片
        public abstract IWordParagraphObj<IImage> CreateImage(IImage Image, Index? Pos = null);
        #endregion
        #endregion
        #region 构造函数与创建对象
        #region 提取Word文档
        /// <summary>
        /// 通过路径获取Word文档，不会引发路径被占用的异常
        /// </summary>
        /// <param name="path">Word文档的路径</param>
        /// <param name="Del">如果缓存中没有使用该路径的文档，
        /// 则通过这个委托创建新文档，参数就是路径</param>
        /// <returns></returns>
        public static IWordDocument GetDocument(PathText path, Func<PathText, WordDocument> Del)
            => GetOfficeFile(path, Del);
        #endregion
        #region 指定路径与受支持文件类型
        /// <summary>
        /// 用指定的文件路径初始化Word文档
        /// </summary>
        /// <param name="path">文档所在的文件路径，
        /// 如果文档不是通过文件创建的，则为<see langword="null"/></param>
        /// <param name="Supported">这个Word实现所支持的文件类型</param>
        public WordDocument(PathText? path, IFileType Supported)
            : base(path, Supported)
        {

        }
        #endregion
        #endregion
    }
}
