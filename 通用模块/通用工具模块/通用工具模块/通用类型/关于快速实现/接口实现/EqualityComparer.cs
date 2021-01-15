using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// 这个类型可以使用委托来进行对象的相等比较
    /// </summary>
    /// <typeparam name="Obj">进行相等比较的对象类型</typeparam>
    class EqualityComparer<Obj> : IEqualityComparer<Obj>
    {
        #region 封装的委托
        #region 进行相等比较的委托
        /// <summary>
        /// 在进行相等比较时，实际执行这个委托
        /// </summary>
        private Func<Obj?, Obj?, bool> EqualsDel { get; }
        #endregion
        #region 计算哈希值的委托
        /// <summary>
        /// 在计算哈希值的时候，实际执行这个委托
        /// </summary>
        private Func<Obj, int> GetHashCodeDel { get; }
        #endregion
        #endregion
        #region 接口实现
        public bool Equals(Obj? x, Obj? y)
            => EqualsDel(x, y);
        public int GetHashCode(Obj obj)
            => GetHashCodeDel(obj);
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="EqualsDel">这个委托用于执行相等比较</param>
        /// <param name="GetHashCodeDel">这个委托用于计算哈希值</param>
        public EqualityComparer(Func<Obj, Obj, bool> EqualsDel, Func<Obj, int> GetHashCodeDel)
        {
            this.EqualsDel = (x, y) => ToolEqual.JudgeNull(x, y) ?? EqualsDel(x!, y!);
            this.GetHashCodeDel = x => x is null ? 0 : GetHashCodeDel(x);
        }
        #endregion
    }
}
