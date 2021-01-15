using System;
using System.Collections.Generic;
using System.Linq;

using static System.Maths.CreateBaseMathObj;

namespace System.Maths
{
    /// <summary>
    /// 这个类型是<see cref="IUnit{Template}"/>的实现，
    /// 可以视为一个不可变的复合计量单位
    /// </summary>
    /// <typeparam name="Template">计量单位的类型</typeparam>
    class Unit<Template> : IUnit<Template>
        where Template : IUT
    {
        #region 返回接口形式
        /// <summary>
        /// 返回本对象的接口形式，
        /// 通过它可以访问显式实现的成员
        /// </summary>
        public IUnit<Template> Interface => this;
        #endregion
        #region 接口实现
        #region 关于单位的值
        #region 公制单位形式
        public Num ValueMetric { get; }
        #endregion
        #region 复合单位形式
        public IEnumerable<(Template Template, Num Value)> Value { get; }
        #endregion
        #endregion
        #region 创建计量单位
        IUnit<Template> IUnit<Template>.Create(Num Num)
            => Unit(Num, this);
        #endregion
        #region 转换为复合单位
        public IUnit<Template> Convert(params Template[] templates)
            => new Unit<Template>(ValueMetric, templates);
        #endregion
        #endregion
        #region 重写的方法
        #region 重写GetHashCode
        public override int GetHashCode()
            => ValueMetric;
        #endregion
        #region 重写Equals
        public override bool Equals(object? obj)
            => obj is IUnit<Template> u &&
            u.ValueMetric == ValueMetric;
        #endregion
        #region 重写ToString
        public override string ToString()
            => Value.Join(x => x.Value.ToString() + x.Template);
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的公制单位数量和模板初始化单位
        /// </summary>
        /// <param name="MetricCount">公制单位的数量</param>
        /// <param name="templates">枚举单位的模板</param>
        public Unit(Num MetricCount, params Template[] templates)
        {
            templates.AnyCheck("公制单位模板");
            ValueMetric = MetricCount;
            Value = templates.Distinct().Sort(false).ToArray().PackIndex(true).
                AggregateSelect(MetricCount, (source, seed) =>
              {
                  var (tem, index, count) = source;
                  var temCount = tem.FromMetric(seed);
                  if (index == count - 1)
                      return ((tem, temCount), default);
                  var (Divisor, Remainder, _) = ToolArithmetic.Split(temCount);
                  return ((tem, Divisor), tem.ToMetric(Remainder));
              }).ToArray();
        }
        #endregion
    }
}
