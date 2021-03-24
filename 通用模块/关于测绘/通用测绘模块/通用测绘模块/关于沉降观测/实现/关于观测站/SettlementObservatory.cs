using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 这个类型代表一个沉降观测站
    /// </summary>
    class SettlementObservatory : SettlementBase, ISettlementObservatory
    {
        #region 高程
        public override IUnit<IUTLength> High
            => Father.To<SettlementBase>()!.High + Recording!;
        #endregion
        #region 添加后代
        public ISettlementPoint Add(string Name, IUnit<IUTLength> Recording)
        {
            var known = this.To<INode>().Ancestors.To<SettlementPointRoot>().Known;
            SettlementPointBase son = known.TryGetValue(Name, out var high) ?
                new SettlementPointFixed(Name, Recording, high, this) :
                new SettlementPoint(Name, Recording, this);
            SonField.AddLast(son);
            return son;
        }
        #endregion 
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Recording">原始记录</param>
        /// <param name="Father">父节点</param>
        public SettlementObservatory(IUnit<IUTLength> Recording, INode Father)
        {
            this.Recording = Recording;
            this.Father = Father;
        }
        #endregion
    }
}
