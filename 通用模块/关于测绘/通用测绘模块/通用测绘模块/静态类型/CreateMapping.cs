using System.Collections.Generic;
using System.Mapping.Settlement;
using System.Maths;

namespace System.Mapping
{
    /// <summary>
    /// 这个静态工具类可以用来帮助创建和测绘有关的对象
    /// </summary>
    public static class CreateMapping
    {
        #region 创建沉降观测根节点
        #region 可指定任意长度单位
        /// <summary>
        /// 创建一个沉降观测基准点，
        /// 它是整个沉降观测记录的第一站
        /// </summary>
        /// <param name="Name">基准点的名称</param>
        /// <param name="High">基准点的高程</param>
        ///  <param name="Known">索引本次沉降观测中，高程已知的点的名称和高程，
        /// 如果为<see langword="null"/>，则代表除基准点外没有已知点</param>
        /// <returns></returns>
        public static ISettlementPoint SettlementPointRoot
            (string Name, IUnit<IUTLength> High, IEnumerable<KeyValuePair<string, IUnit<IUTLength>>>? Known = null)
            => new SettlementPointRoot(Name, High, Known);
        #endregion
        #region 只能使用米
        /// <inheritdoc cref="SettlementPointRoot(string, IUnit{IUTLength}, IEnumerable{KeyValuePair{string, IUnit{IUTLength}}}?)"/>
        /// <param name="HighMetre">使用米作为单位的基准点高程</param>
        /// <returns></returns>
        public static ISettlementPoint SettlementPointRoot
            (string Name, Num HighMetre, IEnumerable<KeyValuePair<string, IUnit<IUTLength>>>? Known = null)
            => new SettlementPointRoot(Name, CreateBaseMathObj.UnitMetric<IUTLength>(HighMetre), Known);
        #endregion
        #endregion
        #region 返回沉降观测专用长度单位
        /// <summary>
        /// 返回沉降观测专用长度单位，
        /// 它等于百分之一毫米
        /// </summary>
        /// <returns></returns>
        public static IUTLength UTSettlement { get; }
            = IUTLength.Create("沉降观测长度单位", 0.00001);
        #endregion
    }
}
