using System.DrawingFrancis;

using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个对象代表一个Excel图片
    /// </summary>
    class ExcelImageObj : ExcelObj<IImage>
    {
        #region Excel对象的内容
        public override IImage Content
            => throw new NotImplementedException("由于底层API限制，不支持此操作");
        #endregion
        #region 复制图像
        #region 复制到工作表
        public override IExcelObj<IImage> Copy(IExcelSheet Target)
            => Target == Sheet ?
                new ExcelImageObj(Sheet, PackShape.Duplicate()) :
                throw new NotSupportedException("由于底层API限制，只支持将图像复制到本工作表");
        #endregion
        #region 复制到文档
        public override Word.IWordParagraphObj<IImage> Copy(Word.IWordDocument Target, Index? Pos = null)
            => throw new NotSupportedException("不支持此API");
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的工作表和Excel形状初始化对象
        /// </summary>
        /// <param name="Sheet">Excel形状所在的工作表</param>
        /// <param name="PackShape">被封装的Excel形状</param>
        public ExcelImageObj(IExcelSheet Sheet, Shape PackShape)
            : base(Sheet, PackShape)
        {
            if (!PackShape.IsImage())
                throw new ArgumentException("这个形状对象不是图片");
        }
        #endregion
    }
}
