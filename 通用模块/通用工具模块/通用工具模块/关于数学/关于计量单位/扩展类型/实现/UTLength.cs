namespace System.Maths
{
    /// <summary>
    /// 这个类型是<see cref="IUTLength"/>的实现，
    /// 可以视为一个长度单位
    /// </summary>
    class UTLength : UT, IUTLength
    {
        #region 返回单位的类型
        protected override Type UTType
            => typeof(IUTLength);
        #endregion
        #region 构造函数
        #region 使用常数
        /// <summary>
        /// 用指定的名称和换算标准（常数）初始化长度单位
        /// </summary>
        /// <param name="Name">长度单位的名称</param>
        /// <param name="Conver">长度单位和公制单位的换算标准，这是一个常数</param>
        public UTLength(string Name, Num Conver)
            : base(Name, Conver)
        {

        }
        #endregion
        #region 使用委托
        /// <summary>
        /// 指定本单位的名称和转换方法，然后初始化单位
        /// </summary>
        /// <param name="Name">本单位的名称</param>
        /// <param name="ToMetric">从本单位转换为公制单位的委托</param>
        /// <param name="FromMetric">从公制单位转换为本单位的委托</param>
        public UTLength(string Name, Func<Num, Num> ToMetric, Func<Num, Num> FromMetric)
            : base(Name, ToMetric, FromMetric)
        {

        }
        #endregion
        #endregion
    }
}
