﻿using System;
using System.Collections.Generic;
using System.Text;

namespace System.Maths
{
    /// <summary>
    /// 表示一个计量单位的模板，
    /// 它规定了该单位的大小和类型
    /// </summary>
    public abstract class UT : IUT
    {
        #region 有关单位换算和比较
        #region 从本单位换算为公制单位
        /// <summary>
        /// 从本单位转换为公制单位的委托
        /// </summary>
        private Func<Num, Num> ToMetricDelegate { get; }

        public Num ToMetric(Num thisUnit)
              => ToMetricDelegate(thisUnit);
        #endregion
        #region 从公制单位换算为本单位
        /// <summary>
        /// 从公制单位转换为本单位的委托
        /// </summary>
        private Func<Num, Num> FromMetricDelegate { get; }

        public Num FromMetric(Num metricUnit)
              => FromMetricDelegate(metricUnit);
        #endregion
        #region 比较本单位与另一个单位
        public int CompareTo(IUT? other)
        {
            if (UTType.IsAssignableFrom(other ?? throw new ArgumentNullException($"{nameof(other)}不能为null")))
                return this.To<IUT>().Size.CompareTo(other.Size);
            throw new NotSupportedException($"{GetType()}和{other.GetType()}类型不同，无法进行比较");
        }
        #endregion
        #endregion
        #region 关于本单位的信息
        #region 是否为静态单位
        public bool IsStatic { get; }
        #endregion
        #region 返回单位名称
        public string Name { get; }
        #endregion
        #region 返回本单位的类型
        /// <summary>
        /// 这个属性指示单位的类型，
        /// 如长度单位，重量单位等，
        /// 该类型应该是一个继承自<see cref="IUT"/>的接口，
        /// 而且派生类应当实现该接口
        /// </summary>
        protected abstract Type UTType { get; }
        #endregion
        #endregion
        #region 重写方法
        #region 重写GetHashCode
        public override int GetHashCode()
            => Name.GetHashCode();
        #endregion
        #region 重写的Equals
        public override bool Equals(object? obj)
            => obj is IUT u &&
            Name == u.Name &&
            this.To<IUT>().Size == u.Size &&
            UTType.IsAssignableFrom(u);
        #endregion
        #region 重写ToString
        public sealed override string ToString()
            => Name;
        #endregion
        #endregion
        #region 构造函数
        #region 指定名称与转换委托
        /// <summary>
        /// 指定本单位的名称和转换方法，然后初始化单位
        /// </summary>
        /// <param name="Name">本单位的名称</param>
        /// <param name="ToMetricDelegate">从本单位转换为公制单位的委托</param>
        /// <param name="FromMetricDelegate">从公制单位转换为本单位的委托</param>
        /// <param name="IsStatic">如果这个值为<see langword="true"/>，
        /// 代表本单位为静态单位，否则为动态单位</param>
        public UT(string Name, Func<Num, Num> ToMetricDelegate, Func<Num, Num> FromMetricDelegate, bool IsStatic = true)
        {
            this.Name = Name;
            this.ToMetricDelegate = ToMetricDelegate;
            this.FromMetricDelegate = FromMetricDelegate;
            this.IsStatic = IsStatic;
        }
        #endregion
        #region 指定名称与转换常数
        /// <summary>
        /// 指定本单位的名称，
        /// 以及一个和公制单位进行转换的常数，
        /// 然后初始化本单位
        /// </summary>
        /// <param name="Name">本单位的名称</param>
        /// <param name="Size">一个常数，代表1单位的本单位，等于多少公制单位，
        /// 本对象使用它进行单位转换</param>
        public UT(string Name, Num Size)
            : this(Name, x => x * Size, x => x / Size)
        {

        }
        #endregion
        #endregion
    }
}
