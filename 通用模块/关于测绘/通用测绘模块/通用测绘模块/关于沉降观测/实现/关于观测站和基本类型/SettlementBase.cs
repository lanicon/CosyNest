﻿using System.Collections.Generic;
using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 这个类型是所有沉降观测的抽象类型
    /// </summary>
    abstract class SettlementBase : ISettlement
    {
        #region 原始高程
        /// <summary>
        /// 获取未经平差的原始高程
        /// </summary>
        protected IUnit<IUTLength> HighOriginal { get; }
        #endregion
        #region 高程
        public abstract IUnit<IUTLength> High { get; }
        #endregion
        #region 记录
        public IUnit<IUTLength>? Recording { get; init; }
        #endregion
        #region 父节点
        public INode? Father { get; protected set; }
        #endregion
        #region 子节点
        /// <summary>
        /// 枚举该沉降观测点或观测站的所有后代
        /// </summary>
        protected LinkedList<SettlementBase> SonField { get; } = new();

        public IEnumerable<INode> Son => SonField;
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="HighOriginal">未经平差的原始高程</param>
        public SettlementBase(IUnit<IUTLength> HighOriginal)
        {
            this.HighOriginal = HighOriginal;
        }
        #endregion
    }
}
