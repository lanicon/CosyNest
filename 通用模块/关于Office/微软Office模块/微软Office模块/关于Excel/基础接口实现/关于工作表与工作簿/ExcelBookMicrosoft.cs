﻿using System.Diagnostics;
using System.IO;
using System.IOFrancis.FileSystem;
using System.Office.Excel.Realize;
using System.Runtime.InteropServices;

using Microsoft.Office.Interop.Excel;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个对象代表通过微软COM组件实现的Excel工作簿
    /// </summary>
    class ExcelBookMicrosoft : ExcelBook, IExcelBook
    {
        #region 封装的对象
        #region 封装的Excel对象
        /// <summary>
        /// 获取封装的Excel对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        internal Application PackExcel
            => PackBook.Application;
        #endregion
        #region 封装的工作簿
        /// <summary>
        /// 返回Excel对象的唯一一个工作簿
        /// </summary>
        internal Workbook PackBook { get; }

        //根据设计，每个IExcelBook对象应该独占一个ExcelAPP
        #endregion
        #endregion
        #region 关于工作簿
        #region 释放工作薄
        #region 说明文档
        /*问：这个函数通过直接终止进程的方式释放资源，
           为什么要用这种笨办法？
           答：这是不得已为之，因为COM对象的回收非常复杂，
           作者尝试了很多方法都不能完美实现，
           如果后续有更好的办法，作者会将其重构*/
        #endregion
        #region 正式方法
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowThreadProcessId(IntPtr hwnd, out int processid);

        protected override void DisposeOfficeRealize()
        {
#if DEBUG
            if (FromActive)       //如果这个对象是从打开的Excel窗口中加载的
                return;                     //则不关闭它，由用户自己决定何时关闭
#endif
#pragma warning disable CA1806
            GetWindowThreadProcessId(new IntPtr(PackExcel.Hwnd), out var pid);
#pragma warning restore
            PackBook.Close();
            PackExcel.Workbooks.Close();
            PackExcel.DisplayAlerts = true;
            PackExcel.Quit();
            Process.GetProcessById(pid).Kill();
        }
        #endregion
        #endregion
        #region 保存工作簿
        protected override void SaveRealize(string Path)
        {
            if (!PackBook.Saved)
            {
                if (File.Exists(Path) && Path == this.Path)
                    PackBook.Save();
                else PackBook.SaveAs(Path);
            }
        }
        #endregion
        #region 开启或关闭自动计算
        public override bool AutoCalculation
        {
            get => PackExcel.Calculation != XlCalculation.xlCalculationManual;
            set => PackExcel.Calculation = value ? XlCalculation.xlCalculationAutomatic : XlCalculation.xlCalculationManual;
        }
        #endregion
        #region 返回打印对象
        private IOfficePrint? PrintField;

        public override IOfficePrint Print
            => PrintField ??= new WorkBookPrint(PackBook);
        #endregion
        #endregion
        #region 返回工作表集合
        public override IExcelSheetCollection Sheets { get; }
        #endregion
        #region 关于构造对象
        #region 供调试使用的成员
#if DEBUG
        #region 指示Excel对象的创建方式
        /// <summary>
        /// 如果这个值为真，
        /// 代表这个对象是从已经打开的Excel窗口中加载的，
        /// 否则代表它是通过程序创建的
        /// </summary>
        private bool FromActive { get; }
        #endregion
        #region 构造函数：指定工作簿
        /// <summary>
        /// 使用指定的工作簿初始化对象
        /// </summary>
        /// <param name="WorkBook">指定的工作簿</param>
        public ExcelBookMicrosoft(Workbook WorkBook)
            : base(WorkBook.FullName, CreateMSOffice.SupportExcel)
        {
            PackBook = WorkBook;
            PackExcel.DisplayAlerts = false;
            FromActive = true;
            Sheets = new ExcelSheetCollectionMS(this);
        }
        #endregion
#endif
        #endregion
        #region 构造函数：指定路径
        /// <summary>
        /// 通过指定的路径初始化Excel工作簿
        /// </summary>
        /// <param name="Path">工作簿所在的路径，
        /// 如果该工作簿尚未保存到文件中，则为<see langword="null"/></param>
        public ExcelBookMicrosoft(PathText? Path = null)
            : base(Path, CreateMSOffice.SupportExcel)
        {
            var PackExcel = new Application()
            {
                Visible = false,
                DisplayAlerts = false,
                FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip
            };
            var books = PackExcel.Workbooks;
            PackBook = Path is null || !File.Exists(Path) ?
                books.Add() : books.Open(Path.Path);
            Sheets = new ExcelSheetCollectionMS(this);
        }
        #endregion
        #endregion
    }
}
