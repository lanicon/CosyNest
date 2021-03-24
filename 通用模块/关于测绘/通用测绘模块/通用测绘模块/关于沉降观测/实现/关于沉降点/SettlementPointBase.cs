using System.Collections.Generic;
using System.Linq;
using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 这个类型是所有沉降观测点的基类
    /// </summary>
    abstract class SettlementPointBase : SettlementBase, ISettlementPoint
    {
        #region 名称
        public string Name { get; }
        #endregion
        #region 关于闭合与附合
        #region 闭合/附合点
        /// <summary>
        /// 返回与这个点闭合或附合的点，
        /// 如果没有闭合或附合，则为<see langword="null"/>
        /// </summary>
        public ISettlementPoint? Closed { get; protected set; }
        #endregion
        #region 闭合/附合差
        public IUnit<IUTLength> ClosedDifference { get; set; } = IUnit<IUTLength>.Zero;
        #endregion
        #region 刷新闭合
        /// <summary>
        /// 调用这个方法以刷新闭合站和闭合差
        /// </summary>
        protected void RefreshClosed()
        {
            #region 用于枚举闭合环的本地函数
            SettlementPointBase[] Closed()
            {
                var list = new LinkedList<SettlementPointBase>();
                var isFixed = this is SettlementPointFixed;
                foreach (var item in this.To<INode>().AncestorsAll.OfType<SettlementPointBase>())
                {
                    list.AddLast(item);
                    if (item.Name == Name || (isFixed && item is SettlementPointFixed))        //检查是否为闭合或附合
                        return list.ToArray();
                }
                return CreateCollection.Empty(list);
            }
            #endregion
            var closed = Closed();
            if (closed.Any())
            {
                var closedPoint = closed[^1];
                this.Closed = closedPoint;
                var difference = ((this is SettlementPointFixed f ? f.HighCalculation : High) - closedPoint.High) / closed.Length;
                closed.Append(this).ForEach(x => x.ClosedDifference = difference);
            }
        }
        #endregion
        #endregion
        #region 添加后代观测站
        public ISettlementObservatory Add(IUnit<IUTLength> Recording)
        {
            var son = new SettlementObservatory(Recording, this);
            SonField.AddLast(son);
            return son;
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Name">沉降观测点的名称</param>
        public SettlementPointBase(string Name)
        {
            this.Name = Name;
        }
        #endregion
    }
}
