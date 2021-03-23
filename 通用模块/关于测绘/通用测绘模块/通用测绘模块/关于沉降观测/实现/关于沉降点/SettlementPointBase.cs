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
        protected ISettlementPoint? Closed { get; set; }
        #endregion
        #region 闭合/附合差
        public IUnit<IUTLength> ClosedDifference { get; protected set; } = IUnit<IUTLength>.Zero;
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
                var known = this.To<INode>().Ancestors.To<SettlementPointRoot>().Known;
                var inKnown = known.ContainsKey(Name);
                foreach (var item in this.To<INode>().AncestorsAll.OfType<SettlementPointBase>())
                {
                    list.AddLast(item);
                    if (item.Name == Name || (inKnown && known.ContainsKey(item.Name)))
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
                var difference = (HighOriginal - closedPoint.HighOriginal) / closed.Length;
                closed.Append(this).ForEach(x => x.ClosedDifference = difference);
            }
        }
        #endregion
        #endregion
        #region 关于添加和移除后代
        #region 添加后代观测站
        public ISettlementObservatory Add(IUnit<IUTLength> Recording)
        {
            var son = new SettlementObservatory(Recording, this);
            SonField.AddLast(son);
            return son;
        }
        #endregion
        #region 移除所有后代
        public override void RemoveOffspring()
        {

        }
        #endregion
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
