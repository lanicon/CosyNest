using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Office.Excel;

namespace System.DataFrancis.Excel.Block
{
    /// <summary>
    /// 这个类型是<see cref="IDataPipeQuery"/>的实现，
    /// 可以通过块从Excel工作簿中提取数据
    /// </summary>
    class DataPipeQueryBlock : IDataPipeQuery
    {
        #region 获取是否支持绑定
        public bool CanBinding => true;
        #endregion
        #region 查询数据所需要的信息
        #region 数据所在的工作簿
        /// <summary>
        /// 获取数据所在的工作簿
        /// </summary>
        private IEnumerable<IExcelBook> Source { get; }
        #endregion
        #region 块地图
        /// <summary>
        /// 获取块地图，它描述块的特征
        /// </summary>
        private IBlockMap Map { get; }
        #endregion
        #region 用来提取块的委托
        /// <summary>
        /// 这个委托被用来从Excel工作簿中提取块
        /// </summary>
        private ExtractionBlock Extraction { get; }
        #endregion
        #endregion
        #region 查询数据
        public IDirectView<IData> Query(Expression<Func<PlaceholderData, bool>>? Expression, bool Binding)
            => Extraction(Source, Map).Select(cell =>
            {
                var data = CreateDataObj.Data(Map.Property.Keys.ToArray());
                foreach (var (name, (get, _, _)) in Map.Property)
                {
                    data[name] = get(cell);
                }
                if (Binding)
                    data.Binding = new BlockBinding(cell, Map.Property);
                return data;
            }).ToDirectView();
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Map">块地图，它描述块的特征</param>
        /// <param name="Extraction">这个委托被用来从Excel工作簿中提取块</param>
        /// <param name="Source">获取数据所在的工作簿</param>
        public DataPipeQueryBlock(IBlockMap Map, ExtractionBlock Extraction, params IExcelBook[] Source)
        {
            this.Map = Map;
            this.Extraction = Extraction;
            this.Source = Source;
        }
        #endregion
    }
    #region 用来提取块的委托
    /// <summary>
    /// 这个委托被用来从Excel工作簿中提取块
    /// </summary>
    /// <param name="Source">块所在的工作簿</param>
    /// <param name="Map">块地图，它描述块的特征</param>
    /// <returns></returns>
    public delegate IEnumerable<IExcelCells> ExtractionBlock(IEnumerable<IExcelBook> Source, IBlockMap Map);
    #endregion
}
