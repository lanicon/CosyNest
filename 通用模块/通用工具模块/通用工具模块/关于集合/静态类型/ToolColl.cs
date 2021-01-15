using System.Collections.Specialized;
using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// 关于集合的工具类
    /// </summary>
    public static class ToolColl
    {
        #region 帮助创建NotifyCollectionChangedEventArgs 
        #region 创建描述添加或删除的Args 
        /// <summary>
        /// 帮助创建一个<see cref="NotifyCollectionChangedEventArgs"/>，
        /// 只能创建描述添加或删除的事件数据
        /// </summary>
        /// <param name="IsAdd">如果这个值为<see langword="true"/>，创建描述添加的事件数据，
        /// 否则创建描述删除的事件数据</param>
        /// <param name="Elements">受影响的对象，可以是集合（代表多项更改），
        /// 也可以是集合中的元素（代表单项更改）</param>
        /// <returns></returns>
        public static NotifyCollectionChangedEventArgs CreateNCCE(bool IsAdd, object Elements)
        {
            var Action = IsAdd ? NotifyCollectionChangedAction.Add : NotifyCollectionChangedAction.Remove;
            var list = Elements is IEnumerable List ? List.ToList<object>() : (IList)new object[] { Elements };
            return new NotifyCollectionChangedEventArgs(Action, list);
        }
        #endregion
        #endregion
        #region 创建包含指定数量元素的迭代器
        /// <summary>
        /// 创建一个包含指定数量元素的迭代器
        /// </summary>
        /// <typeparam name="Obj">集合的元素类型</typeparam>
        /// <param name="Count">集合的元素数量</param>
        /// <param name="GetElements">用来生成元素的委托，委托参数就是元素的索引</param>
        /// <returns></returns>
        public static IEnumerable<Obj> Range<Obj>(int Count, Func<int, Obj> GetElements)
        {
            for (int i = 0; i < Count; i++)
            {
                yield return GetElements(i);
            }
        }
        #endregion
    }
}
