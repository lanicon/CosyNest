using System.DrawingFrancis;
using System.Office.Excel;
using System.Office.Word.Realize;

using Microsoft.Office.Interop.Word;

namespace System.Office.Word
{
    /// <summary>
    /// 这个类型代表一个封装了图片的Word段落
    /// </summary>
    class WordParagraphImageMicrosoft : WordParagraphObjMicrosoft<IImage>
    {
        #region 复制Office对象
        #region 复制到工作表
        public override IExcelObj<IImage> Copy(IExcelSheet Target)
            => throw new Exception("不支持此API");
        #endregion
        #region 复制到文档
        public override IWordParagraphObj<IImage> Copy(IWordDocument Target, Index? Pos = null)
            => throw new Exception("不支持此API");
        #endregion
        #endregion
        #region 返回封装的图片
        public override IImage Content
            => throw new NotImplementedException("由于底层API限制，不支持这个属性");
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的文档和形状初始化段落
        /// </summary>
        /// <param name="doc">这个段落所在的文档</param>
        /// <param name="PackShape">这个段落所封装的形状对象</param>
        public WordParagraphImageMicrosoft(WordDocument doc, InlineShape PackShape)
            : base(doc, PackShape)
        {

        }
        #endregion
    }
}
