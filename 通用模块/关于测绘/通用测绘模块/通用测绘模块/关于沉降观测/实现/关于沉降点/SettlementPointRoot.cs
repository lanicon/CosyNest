using System.Collections.Generic;
using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 代表一个专门用来作为起始点的沉降点，
    /// 它储存了有关已知固定高程点的信息
    /// </summary>
    class SettlementPointRoot : SettlementPointFixed
    {
        #region 索引高程已知的沉降点
        /// <summary>
        /// 索引本次沉降观测中，
        /// 高程已知的点的名称的高程
        /// </summary>
        public IReadOnlyDictionary<string, IUnit<IUTLength>> Known { get; }
        #endregion
        #region 构造函数
        /// <inheritdoc cref="SettlementPointFixed(string, IUnit{IUTLength}?, IUnit{IUTLength}, INode?)"/>
        /// <param name="Known">索引本次沉降观测中，高程已知的点的名称的高程</param>
        public SettlementPointRoot(string Name, IUnit<IUTLength> High, IReadOnlyDictionary<string, IUnit<IUTLength>> Known)
            : base(Name, null, High, null)
        {
            this.Known = Known;
        }
        #endregion
    }
}
