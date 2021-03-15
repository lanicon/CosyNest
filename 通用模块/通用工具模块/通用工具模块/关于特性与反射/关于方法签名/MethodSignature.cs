namespace System.Reflection
{
    /// <summary>
    /// 这个类型表示一个方法的签名
    /// </summary>
    public sealed class MethodSignature : MethodBaseSignature
    {
        #region 返回值类型
        /// <summary>
        /// 获取方法的返回值类型
        /// </summary>
        public Type Return { get; }
        #endregion
        #region 重写的方法
        #region 重写GetHash
        public override int GetHashCode()
            => HashCode.Combine(base.GetHashCode(), Return);
        #endregion
        #region 重写ToString
        public override string ToString()
            => base.ToString() + $"   返回值：{Return.Name}";
        #endregion
        #endregion
        #region 判断方法签名是否兼容
        public override bool IsSame(MethodBaseSignature Other, bool ExactlySame = true)
        {
            if (Other is MethodSignature other)
            {
                var retSame = ExactlySame ?
                    Return == other.Return : Return.IsAssignableFrom(other.Return);
                return retSame && Aided(other, ExactlySame);
            }
            return false;
        }
        #endregion
        #region 构造函数与创建对象
        #region 用于创建对象的静态方法
        /// <summary>
        /// 创建一个方法签名，该签名的参数和返回值与一个委托签名相同
        /// </summary>
        /// <typeparam name="DelegatesType">委托类型</typeparam>
        /// <returns></returns>
        public static MethodSignature Create<DelegatesType>()
            where DelegatesType : Delegate
            => typeof(DelegatesType).GetMethod("Invoke")!.GetSignature();
        #endregion
        #region 指定返回值和参数类型列表
        /// <summary>
        /// 用指定的返回值类型和参数类型列表初始化方法签名
        /// </summary>
        /// <param name="Return">返回值类型，如果为<see langword="null"/>，则为<see cref="void"/></param>
        /// <param name="ParametersType">方法的参数列表</param>
        public MethodSignature(Type? Return, params Type[] ParametersType)
            : base(ParametersType)
        {
            this.Return = Return ?? typeof(void);
        }
        #endregion
        #region 指定返回值和参数类型
        /// <summary>
        /// 用指定的返回值类型和参数列表初始化方法签名
        /// </summary>
        /// <param name="Return">返回值类型，如果为<see langword="null"/>，则为<see cref="void"/></param>
        /// <param name="Parameters">方法的参数列表</param>
        public MethodSignature(Type? Return, params object[] Parameters)
            : base(Parameters)
        {
            this.Return = Return ?? typeof(void);
        }
        #endregion
        #endregion
    }
}
