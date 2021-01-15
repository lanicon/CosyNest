using System.Threading.Tasks;
using System.Design.Direct;

namespace System.DataFrancis
{
    /// <summary>
    /// 所有实现这个接口的类型，
    /// 都可以作为一个支持添加数据的管道
    /// </summary>
    public interface IDataPipeAdd
    {
        #region 说明文档
        /*问：在本接口的早期版本中，
          曾经允许对待添加的数据进行筛选，
          而现在为什么删除了这个功能？
          答：因为这会导致实际被添加的数据和传入的参数不相同，
          如果需要对数据进行绑定，实现这个功能会相当复杂，
          如果你确实需要这个功能，可以考虑使用装饰器模式，
          用一个IDataPipeAdd嵌套另一个IDataPipeAdd，
          然后自行完成所有适配工作*/
        #endregion
        #region 获取是否支持绑定
        /// <summary>
        /// 如果这个值为<see langword="true"/>，代表该数据管道支持绑定
        /// </summary>
        bool CanBinding => false;
        #endregion
        #region 关于添加数据
        #region 同步添加数据
        /// <summary>
        /// 通过管道将数据添加到数据源
        /// </summary>
        /// <param name="Data">待添加的数据</param>
        /// <param name="Binding">如果这个值为<see langword="true"/>，
        /// 则在添加数据的时候，还会将数据与数据源绑定，但数据源可能不支持绑定</param>
        void Add(IDirectView<IData> Data, bool Binding = false);
        #endregion
        #region 异步添加数据
        /// <summary>
        /// 通过管道将数据异步添加到数据源
        /// </summary>
        /// <param name="Data">待添加的数据</param>
        /// <param name="Binding">如果这个值为<see langword="true"/>，
        /// 则在添加数据的时候，还会将数据与数据源绑定，但数据源可能不支持绑定</param>
        /// <returns>一个异步对象，等待它以完成添加数据操作</returns>
        Task AddAsyn(IDirectView<IData> Data, bool Binding = false)
           => Task.Run(() => Add(Data, Binding));
        #endregion
        #endregion 
    }
}
