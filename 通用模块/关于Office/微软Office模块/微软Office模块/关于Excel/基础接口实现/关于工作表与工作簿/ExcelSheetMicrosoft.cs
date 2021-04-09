using System.Collections.Generic;
using System.Drawing;
using System.DrawingFrancis;
using System.DrawingFrancis.Graphics;
using System.Linq;
using System.Maths;
using System.Office.Chart;
using System.Office.Excel.Realize;
using System.Underlying.PC;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个类型代表通过微软COM组件实现的Excel工作表
    /// </summary>
    class ExcelSheetMicrosoft : ExcelSheet, IExcelSheet
    {
        #region 封装的Excel工作表
        /// <summary>
        /// 获取封装的Excel工作表，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        internal Worksheet PackSheet { get; }
        #endregion
        #region 关于工作表
        #region 读写工作表的名称
        public override string Name
        {
            get => PackSheet.Name;
            set => PackSheet.Name = value;
        }
        #endregion
        #region 删除工作表
        public override void Delete()
        {
            if (Book.Sheets.Count == 1)
                Book.DeleteBook();
            else
            {
                PackSheet.Unprotect();
                PackSheet.Delete();
            }
        }
        #endregion
        #region 复制工作表
        public override IExcelSheet Copy(IExcelSheetCollection collection)
        {
            var Book = collection.Book.To<ExcelBookMicrosoft>();
            var Sheets = Book.PackBook.Sheets;
            PackSheet.Copy(After: Sheets.Last());
            return new ExcelSheetMicrosoft(Book, Sheets.Last());
        }
        #endregion
        #endregion
        #region 获取页面对象
        private IPageSheet? PageField;
        public override IPageSheet Page
            => PageField ??= new PageSheet(PackSheet);
        #endregion
        #region 关于Range
        #region 返回行或者列
        public override IExcelRC GetRC(int Begin, int? End, bool IsRow)
        {
            var end = End ?? Begin;
            var Range = PackSheet.Range[ExcelRealize.GetAddress(Begin, end, IsRow)];
            return new ExcelRCMicrosoft(this, Range, IsRow, Begin, end);
        }
        #endregion
        #region 返回用户范围
        public override IExcelCells RangUser
        {
            get
            {
                var (_, _, ER, EC) = PackSheet.UsedRange.GetAddress();
                var Range = PackSheet.Range[ExcelRealize.GetAddress(0, 0, ER, EC)];
                return new ExcelCellsMicrosoft(this, Range);
            }
        }
        #endregion
        #endregion
        #region 关于Excel对象
        #region 关于图表
        #region 枚举所有图表
        public override IEnumerable<IExcelObj<IOfficeChart>> Charts
            => PackSheet.Shapes.GetShapes().
            Where(x => x.IsChart()).
            Select(x => x.ToChart(this));
        #endregion
        #region 获取图表创建器
        private ICreateExcelChart? CreateChartFiled;
        public override ICreateExcelChart CreateChart
            => CreateChartFiled ??= new CreateChartExcelMicrosoft(this);
        #endregion
        #endregion
        #region 关于图片
        #region 枚举所有图片
        public override IEnumerable<IExcelObj<IImage>> Images
            => PackSheet.Shapes.GetShapes().
            Where(x => x.IsImage()).
            Select(x => new ExcelImageObj(this, x));
        #endregion
        #region 添加图片
        public override IExcelObj<IImage> CreateImage(IImage image)
        {
            var path = MSOfficeRealize.SaveImage(image);
            using var i = Image.FromFile(path.Path);
            var (W, H) = (i.Width, i.Height);
            #region 转换单位的本地函数
            static float Conver(Num Num, IUTLength UT)
                => CreateBaseMathObj.Unit(Num, UT).ConvertSingle(DrawingUnitsCom.LengthPoint);
            #endregion
            var NewImage = PackSheet.Shapes.AddPicture
                (path.Path, MsoTriState.msoFalse, MsoTriState.msoTrue, 0, 0,
                Conver(W, CreateHardwarePC.Screen.LengthPixelX),
                Conver(H, CreateHardwarePC.Screen.LengthPixelY));
            return new ExcelImageObj(this, NewImage);
        }
        #endregion
        #endregion
        #region 返回画布
        private ICanvas? CanvasField;
        public override ICanvas Canvas
            => CanvasField ??= new ExcelCanvas(PackSheet.Shapes);
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的工作簿和工作表初始化对象
        /// </summary>
        /// <param name="Book">这个工作表所在的工作簿</param>
        /// <param name="Sheet">被封装的工作表</param>
        public ExcelSheetMicrosoft(IExcelBook Book, Worksheet Sheet)
            : base(Book)
        {
            this.PackSheet = Sheet;
        }
        #endregion
    }
}
