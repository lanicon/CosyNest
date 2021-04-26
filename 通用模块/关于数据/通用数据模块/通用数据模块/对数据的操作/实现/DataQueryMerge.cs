using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System.DataFrancis
{
    /// <summary>
    /// 这个类型可以合并其他数据管道的输出，
    /// 并将其作为一个新的管道
    /// </summary>
    class DataQueryMerge : IDataPipeQuery
    {
        #region 数据源
        /// <summary>
        /// 本对象实际上通过这些管道获取数据
        /// </summary>
        private IEnumerable<IDataPipeQuery> DataSource { get; }
        #endregion
        #region 获取数据
        public IDirectView<IData> Query(Expression<Func<PlaceholderData, bool>>? Expression, bool Binding)
            => DataSource.Select(x => x.Query(Expression, Binding)).UnionNesting(false).ToDirectView();
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="DataSource">实际用来获取数据的管道</param>
        public DataQueryMerge(params IDataPipeQuery[] DataSource)
        {
            this.DataSource = DataSource;
        }
        #endregion
    }
}
