using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.DataFrancis
{
    /// <summary>
    /// 这个类型可以从集合中获取数据
    /// </summary>
    class DataQueryFromCollection : IDataPipeQuery
    {
        #region 说明文档
        /*问：这个类型直接从集合中获取数据，
          这有什么意义？为什么不直接访问那个集合呢？
          答：主要有两个意义，
          第一，可以用来做数据绑定，
          第二，可以利用迭代器无限返回假数据，为测试提供方便*/
        #endregion
        #region 用来获取数据的集合
        /// <summary>
        /// 这个集合将被作为数据管道的数据源
        /// </summary>
        private IEnumerable<IData> Source { get; set; }
        #endregion
        #region 同步查询数据
        public IDirectView<IData> Query(Expression<Func<PlaceholderData, bool>>? Expression, bool Binding)
            => IDataPipeQuery.QueryTemplate(Source, Expression);
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的数据源初始化对象
        /// </summary>
        /// <param name="Source">这个集合将被作为数据管道的数据源</param>
        public DataQueryFromCollection(IEnumerable<IData> Source)
        {
            this.Source = Source;
        }
        #endregion
    }
}
