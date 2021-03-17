using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 这个类型代表一个高程已知的沉降观测点，
    /// 它通常是基准点或附合点
    /// </summary>
    class SettlementPointFixed : SettlementPointBase
    {
        #region 高程
        public override IUnit<IUTLength> High
            => HighOriginal;
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Name">基准点的名称</param>
        /// <param name="High">基准点的高程</param>
        public SettlementPointFixed(string Name, IUnit<IUTLength> High)
            : base(Name, High)
        {

        }
        #endregion
    }
}
