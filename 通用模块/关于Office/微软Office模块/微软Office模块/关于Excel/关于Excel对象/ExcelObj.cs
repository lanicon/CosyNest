using System;
using System.Collections.Generic;
using System.Text;
using System.Maths;
using IPoint = System.Maths.IPoint;
using Microsoft.Office.Interop.Excel;
using System.Office.Word;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个类型是所有Excel对象的基类
    /// </summary>
    /// <typeparam name="Obj">Excel对象封装的对象类型</typeparam>
    abstract class ExcelObj<Obj> : IExcelObj<Obj>
    {
        #region 被封装的Excel形状
        /// <summary>
        /// 获取被封装的Excel形状
        /// </summary>
        protected Shape PackShape { get; }
        #endregion
        #region 对象的信息
        #region Excel对象的内容
        public abstract Obj Content { get; }
        #endregion
        #region 对象所在的工作表
        public IExcelSheet Sheet { get; }
        #endregion
        #region 对象的位置
        public IPoint Position
        {
            get => CreateMathObj.Point(PackShape.Left, -PackShape.Top);
            set
            {
                var (R, T) = value;
                PackShape.Left = R;
                PackShape.Top = ToolArithmetic.Abs(T);
            }
        }
        #endregion
        #region 对象的大小
        public ISize Size
        {
            get => PackShape.GetSize();
            set => PackShape.SetSize(value);
        }
        #endregion
        #endregion
        #region 操作对象
        #region 删除Excel对象
        public void Delete()
            => PackShape.Delete();
        #endregion
        #region 复制Excel对象
        #region 复制到工作表
        public abstract IExcelObj<Obj> Copy(IExcelSheet Target);
        #endregion
        #region 复制到文档
        public abstract IWordParagraphObj<Obj> Copy(IWordDocument Target, Index? Pos = null);
        #endregion
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Sheet">Excel形状所在的工作表</param>
        /// <param name="PackShape">被封装的Excel形状</param>
        public ExcelObj(IExcelSheet Sheet, Shape PackShape)
        {
            this.Sheet = Sheet;
            this.PackShape = PackShape;
        }
        #endregion
    }
}
