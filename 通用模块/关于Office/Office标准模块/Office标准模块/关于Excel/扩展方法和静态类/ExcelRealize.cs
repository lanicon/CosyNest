using System.Collections.Generic;
using System.IOFrancis.FileSystem;
using System.Linq;
using System.Maths;
using System.Office.Realize;
using System.Reflection;
using System.Text.RegularExpressions;

namespace System.Office.Excel.Realize
{
    /// <summary>
    /// 这个类型为实现Excel对象提供帮助
    /// </summary>
    public static class ExcelRealize
    {
        #region 关于工作表
        #region 修改工作表名称以保证不重复
        /// <summary>
        /// 如果一个工作表名称在工作薄内已经存在，
        /// 则不断的修改它，直到没有重复的名称为止，
        /// 这个方法可以避免由于工作表重名导致的异常
        /// </summary>
        /// <param name="Sheets">要检查的工作薄的工作表容器</param>
        /// <param name="SheetName">要检查的工作表名称，
        /// 如果为<see langword="null"/>，则检查默认名称</param>
        /// <returns>新的工作表名称，保证不重复</returns>
        public static string SheetRepeat(IExcelSheetCollection Sheets, string? SheetName = null)
            => Sheets.Select(x => x.Name)
            .Distinct(SheetName ?? "工作表");
        #endregion
        #endregion
        #region 关于Range
        #region 复制格式
        #region 缓存属性
        /// <summary>
        /// 这个属性缓存IRangStyle中所有公开，
        /// 而且既能读取又能写入的属性，为复制格式提供方便
        /// </summary>
        private static IEnumerable<PropertyInfo> CacheStyleProperty { get; }
        = typeof(IRangeStyle).GetProperties().
            Where(x => x.GetPermissions() == null && !x.IsStatic()).ToArray();
        #endregion
        #region 复制单元格格式
        /// <summary>
        /// 复制单元格格式
        /// </summary>
        /// <param name="Source">待复制的格式</param>
        /// <param name="Target">复制的目标格式</param>
        public static void CopyStyle(IRangeStyle Source, IRangeStyle Target)
        {
            CacheStyleProperty.ForEach(x => x.Copy(Source, Target));
        }
        #endregion
        #endregion
        #region 关于地址
        #region 关于A1地址
        #region 返回A1地址
        #region 将列号转换为文本形式
        /// <summary>
        /// 将列号转换为文本形式
        /// </summary>
        /// <param name="Col">待转换的列号，从0开始</param>
        /// <returns>列号对应的A1格式地址</returns>
        public static string ColToText(int Col)
        {
            ExceptionIntervalOut.Check(0, null, Col);
            var Beg = (int)'A';
            return ToolBit.FromDecimal(Col, 26).Integer.PackIndex(true).
                Select(x => (char)(Beg + (x.Index == 0 && x.Count > 1 ? x.Elements - 1 : x.Elements))).Join();
        }
        #endregion
        #region 根据行列号
        /// <summary>
        /// 根据起止行列号，返回A1地址
        /// </summary>
        /// <param name="Begin">开始行列号，从0开始</param>
        /// <param name="End">结束行列号，从0开始</param>
        /// <param name="IsRow">如果这个值为<see langword="true"/>，
        /// 代表要返回地址的对象是行，否则是列</param>
        /// <returns>行列号对应的A1格式地址</returns>
        public static string GetAddress(int Begin, int End, bool IsRow)
        {
            ExceptionIntervalOut.Check(0, null, Begin, End);
            return IsRow ? $"{Begin + 1}:{End + 1}" :
                ColToText(Begin) + ":" + ColToText(End);
        }
        #endregion
        #region 根据坐标
        /// <summary>
        /// 根据坐标，返回A1地址，
        /// 注意：这里的坐标全部从0开始
        /// </summary>
        /// <param name="BR">起始行号</param>
        /// <param name="BC">起始列号</param>
        /// <param name="ER">结束行号，如果为-1，则与起始行号相同</param>
        /// <param name="EC">结束列号，如果为-1，则与起始列号相同</param>
        /// <returns>行列号对应的A1格式地址</returns>
        public static string GetAddress(int BR, int BC, int ER = -1, int EC = -1)
        {
            #region 本地函数
            static string Get(int R, int C)
                => ColToText(C) + (R + 1);
            #endregion
            ER = ER == -1 ? BR : ER;
            EC = EC == -1 ? BC : EC;
            return Get(BR, BC) +
                (BR == ER && BC == EC ? "" : $":{Get(ER, EC)}");
        }
        #endregion
        #region 根据ISizePos
        /// <summary>
        /// 根据一个平面，返回它的A1地址
        /// </summary>
        /// <param name="Rectangular">待返回A1地址的平面</param>
        /// <returns>平面所对应的A1格式地址</returns>
        public static string GetAddress(ISizePosPixel Rectangular)
        {
            var (TopLeft, BottomRight) = Rectangular.Boundaries;
            var (BC, BR) = TopLeft.Abs();
            var (EC, ER) = BottomRight.Abs();
            return GetAddress(BR, BC, ER, EC);
        }
        #endregion
        #endregion
        #region 解析A1地址
        #region 返回行列数
        /// <summary>
        /// 根据A1地址，返回开始和结束的行列数，
        /// 注意：返回的行列数从0开始
        /// </summary>
        /// <param name="Address">待解析的A1地址</param>
        /// <returns></returns>
        public static (int BeginRow, int BeginCol, int EndRow, int EndCol) AddressToTupts(string AddressA1)
        {
            var mathce = /*language=regex*/@"^\$?(?<bc>[A-Z]+)\$?(?<br>\d+)(?<end>:\$?(?<ec>[A-Z]+)\$?(?<er>\d+))?$".
                ToRegex().MatcheFirst(AddressA1)?.GroupsNamed;
            if (mathce is null)
                throw new Exception($"{AddressA1}不是合法的A1地址格式");
            #region 用来获取列号的本地函数
            static int Get(IMatch add)
                => ToolBit.ToDecimal(26, add.Match.Select(x => x - 64).ToArray(), null) - 1;
            #endregion
            var BC = Get(mathce["bc"]);
            var BR = mathce["br"].Match.To<int>() - 1;
            if (mathce.ContainsKey("end"))
            {
                var EC = Get(mathce["ec"]);
                var ER = mathce["er"].Match.To<int>() - 1;
                return (BR, BC, ER, EC);
            }
            return (BR, BC, BR, BC);
        }
        #endregion
        #region 返回ISizePos
        /// <summary>
        /// 解析一个A1格式的地址，并将它转换为等效的平面
        /// </summary>
        /// <param name="AddressA1">待解析的A1格式地址</param>
        /// <returns></returns>
        public static ISizePosPixel AddressToTISizePos(string AddressA1)
        {
            var (BR, BC, ER, EC) = AddressToTupts(AddressA1);
            return CreateMathObj.SizePosPixel(
                CreateMathObj.Point(BC, -BR),
                EC - BC + 1, ER - BR + 1);
        }
        #endregion
        #endregion
        #endregion
        #region 获取完整地址
        /// <summary>
        /// 获取一个单元格的完整地址，
        /// 它由文件名，工作表名和单元格地址组成
        /// </summary>
        /// <param name="File">工作簿所在的文件</param>
        /// <param name="SheetName">工作表的名称</param>
        /// <param name="Address">单元格地址</param>
        /// <returns></returns>
        public static string GetAddressFull(IFile File, string SheetName, string Address)
            => $"'{File.Father?.Path}\\[{File.NameFull}]{SheetName}'!{Address}";
        #endregion
        #endregion
        #endregion
    }
}
