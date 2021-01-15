using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Maths
{
    /// <summary>
    /// 这个静态类可以用来帮助创建基本数学对象
    /// </summary>
    public static class CreateBaseMathObj
    {
        #region 创建Unit
        #region 复制另一个单位的模板
        /// <summary>
        /// 使用指定的公制单位数量，
        /// 并复制另一个单位的模板，以此来创建一个计量单位
        /// </summary>
        /// <typeparam name="Template">计量单位的模板类型</typeparam>
        /// <param name="MetricCount">公制单位的数量</param>
        /// <param name="Unit">另一个复合单位，本对象将复制它的模板</param>
        public static IUnit<Template> Unit<Template>(Num MetricCount, IUnit<Template> Unit)
            where Template : IUT
            => CreateBaseMathObj.Unit(MetricCount, Unit.Templates.ToArray());
        #endregion
        #region 创建指定数量的单位
        /// <summary>
        /// 创建指定数量的单位，并返回该计量单位
        /// </summary>
        /// <typeparam name="Template">计量单位的模板类型</typeparam>
        /// <param name="Count">指定单位的数量，不是公制单位的数量</param>
        /// <param name="UT">指定单位的模板</param>
        /// <returns></returns>
        public static IUnit<Template> Unit<Template>(Num Count, Template UT)
            where Template : IUT
            => Unit(UT.ToMetric(Count), new[] { UT });
        #endregion
        #region 创建指定公制单位数量的单位
        /// <summary>
        /// 创建具有指定公制单位数量的单位
        /// </summary>
        /// <typeparam name="Template">计量单位的模板类型</typeparam>
        /// <param name="MetricCount">公制单位的数量</param>
        /// <returns></returns>
        public static IUnit<Template> Unit<Template>(Num MetricCount)
            where Template : IUT
            => Unit(MetricCount, IUT.GetMetric<Template>());
        #endregion
        #region 创建具有指定模板和指定公制单位数量的单位
        /// <summary>
        /// 创建具有指定模板和指定公制单位数量的单位
        /// </summary>
        /// <typeparam name="Template">计量单位的模板类型</typeparam>
        /// <param name="MetricCount">公制单位的数量</param>
        /// <param name="Templates">指定单位的模板</param>
        /// <returns></returns>
        public static IUnit<Template> Unit<Template>(Num MetricCount, IEnumerable<Template> Templates)
            where Template : IUT
            => new Unit<Template>(MetricCount, Templates.ToArray());
        #endregion
        #endregion
        #region 创建IRandom
        #region 获取公用的随机数生成器
        /// <summary>
        /// 获取一个公用的随机数生成器
        /// </summary>
        public static IRandom RandomOnly { get; }
        = new Random();
        #endregion
        #region 使用指定的种子创建
        /// <summary>
        /// 使用指定的种子创建一个随机数生成器
        /// </summary>
        /// <param name="Seed">用来生成随机数的种子，
        /// 如果为<see langword="null"/>，则默认使用时间作为种子</param>
        /// <returns></returns>
        public static IRandom Random(int? Seed = null)
            => new Random(Seed);
        #endregion
        #endregion
        #region 创建INodeContent
        /// <summary>
        /// 创建一个封装指定类型内容的树形结构节点，
        /// 并以它作为树形结构的根节点
        /// </summary>
        /// <typeparam name="Obj">树形结构节点所封装的对象类型</typeparam>
        /// <param name="Root">指定根节点所封装的对象</param>
        /// <param name="GetSon">这个委托传入节点封装的对象，
        /// 返回它的直接子节点所封装的对象</param>
        /// <returns>新创建的树形结构根节点</returns>
        public static INodeContent<Obj> NodeContent<Obj>(Obj Root, Func<Obj, IEnumerable<Obj>> GetSon)
            => new NodeContent<Obj>(null, Root, GetSon);
        #endregion
    }
}
