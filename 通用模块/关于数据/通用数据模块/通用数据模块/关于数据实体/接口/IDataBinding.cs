namespace System.DataFrancis
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以将数据与数据源绑定
    /// </summary>
    public interface IDataBinding
    {
        #region 数据通知数据源
        #region 通知修改
        /// <summary>
        /// 当数据发生修改时，可以调用这个方法通知数据源
        /// </summary>
        /// <param name="ColumnName">发生修改的列名</param>
        /// <param name="NewValue">修改后的新值</param>
        void NoticeUpdateToSource(string ColumnName, object? NewValue);
        #endregion
        #region 通知删除
        /// <summary>
        /// 当数据被删除时，可以调用这个方法通知数据源
        /// </summary>
        void NoticeDeleteToSource();
        #endregion
        #endregion
        #region 数据源通知数据
        #region 说明文档
        /*实现本API请遵循以下规范：
          如果不支持数据源通知数据，
          那么就声明两个Add和Remove访问器为空的事件，而不是抛出异常
          因为在设置IData.Binding属性时会注册这两个事件，
          抛出异常会导致程序崩溃*/
        #endregion
        #region 通知修改
        /// <summary>
        /// 当数据源发生修改时，会调用这个事件通知数据，
        /// 事件的参数分别是发生修改的列名，以及修改后的新值
        /// </summary>
        event Action<string, object?>? NoticeUpdateToData;
        #endregion
        #region 通知删除
        /// <summary>
        /// 当数据源删除数据时，会调用这个事件通知数据
        /// </summary>
        event Action? NoticeDeleteToData;
        #endregion
        #endregion 
    }
}
