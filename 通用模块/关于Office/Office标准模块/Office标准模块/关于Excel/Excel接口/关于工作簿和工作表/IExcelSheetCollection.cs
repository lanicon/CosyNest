using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Office.Excel
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个Excel工作表集合，
    /// 它可以枚举，添加，删除工作簿中的工作表
    /// </summary>
    public interface IExcelSheetCollection : IList<IExcelSheet>
    {
        #region 说明文档
        /*实现本接口请遵循以下规范：
          #在实现Clear方法时，如果底层Excel引擎允许工作簿中没有工作表，
          则删除全部工作表，否则执行重置操作，也就是将工作簿恢复到仅有一个空白工作表的初始状态*/
        #endregion
        #region 返回工作簿
        /// <summary>
        /// 返回这个容器所在的工作簿
        /// </summary>
        IExcelBook ExcelBook { get; }
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
            => this.FirstOrDefault(x => x.Name == Name) ??
            (CreateTable ? Add(Name) : throw new KeyNotFoundException($"工作簿中不存在名为{Name}的工作表"));
        #endregion
        #region 可能返回null
        /// <summary>
        /// 根据名称，获取工作表
        /// </summary>
        /// <param name="Name">工作表名称</param>
        /// <returns>具有指定名称的工作表，当不存在具有该名称的工作表时，返回<see langword="null"/></returns>
        IExcelSheet? this[string Name]
            => this.FirstOrDefault(x => x.Name == Name);
        #endregion
        #endregion
        #region 添加工作表
        #region 添加空白表
        /// <summary>
        /// 添加一个具有指定名称的空白工作表
        /// </summary>
        /// <param name="Name">工作表名，如果为<see langword="null"/>，则自动为其命名一个默认名称</param>
        /// <returns>新添加的空白工作表</returns>
        IExcelSheet Add(string? Name = null);
        #endregion
        #endregion
        #region 删除工作表
        #region 根据名称
        /// <summary>
        /// 删除具有指定名称的工作表
        /// </summary>
        /// <param name="Name">指定的名称</param>
        /// <returns>如果删除成功，则为<see langword="true"/>，否则为<see langword="false"/></returns>
        bool Remove(string Name)
        {
            var sheet = this.FirstOrDefault(x => x.Name == Name);
            if (sheet == null)
                return false;
            sheet.Delete();
            return true;
        }
        #endregion
        #endregion
    }
}
