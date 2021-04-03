﻿using System.IO;

namespace System.Office.Excel
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以被视作一个Excel工作簿
    /// </summary>
    public interface IExcelBook : IOfficeFile
    {
        #region 说明文档
        /*实现本接口请遵循以下规范：
          #在创建对象时，如果指定了路径，则自动从这个路径中加载工作簿，
          如果没有指定，则创建一个空白工作簿*/
        #endregion
        #region 关于工作簿
        #region 开启或关闭自动计算
        /// <summary>
        /// 如果这个值为<see langword="true"/>，则会启用自动计算，
        /// 将本属性设置为<see langword="false"/>可以改善性能
        /// </summary>
        bool AutoCalculation { get; set; }
        #endregion
        #region 删除工作簿
        /// <summary>
        /// 释放工作簿所占用的资源，并删除文件
        /// </summary>
        void DeleteBook()
        {
            Dispose();
            if (Path != null)
                File.Delete(Path);
        }
        #endregion
        #region 返回打印对象
        /// <summary>
        /// 返回Office打印对象，
        /// 它可以用来打印整个工作簿
        /// </summary>
        IOfficePrint Print { get; }
        #endregion
        #endregion
        #region 关于工作表
        #region 根据索引返回工作表
        /// <summary>
        /// 根据索引，返回工作表
        /// </summary>
        /// <param name="index">工作表的索引</param>
        /// <returns></returns>
        IExcelSheet this[int index]
            => Sheets[index];
        #endregion
        #region 根据名称返回工作表
        #region 不会返回null
        /// <summary>
        /// 根据工作表名，获取工作表
        /// </summary>
        /// <param name="Name">工作表名称</param>
        /// <param name="CreateTable">当工作簿内不存在指定名称的工作表的时候，
        /// 如果这个值为<see langword="true"/>，则创建新表，否则抛出异常</param>
        /// <returns>具有指定名称的工作表，它不可能为<see langword="null"/></returns>
        IExcelSheet this[string Name, bool CreateTable]
            => Sheets[Name, CreateTable];
        #endregion
        #region 可能返回null
        /// <summary>
        /// 根据名称，获取工作表
        /// </summary>
        /// <param name="Name">工作表名称</param>
        /// <returns>具有指定名称的工作表，当不存在具有该名称的工作表时，返回<see langword="null"/></returns>
        IExcelSheet? this[string Name]
            => Sheets[Name];
        #endregion
        #endregion
        #region 返回工作表的容器
        /// <summary>
        /// 返回工作表的容器，
        /// 它可以枚举，添加，删除工作簿中的工作表
        /// </summary>
        IExcelSheetCollection Sheets { get; }
        #endregion
        #endregion
    }
}