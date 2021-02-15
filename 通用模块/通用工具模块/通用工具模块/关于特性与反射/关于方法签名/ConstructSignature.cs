using System.Linq;

namespace System.Reflection
{
    /// <summary>
    /// 这个类型表示构造函数的签名
    /// </summary>
    public sealed class ConstructSignature : MethodBaseSignature
    {
        #region 重写的方法
        #region 重写ToString
        public override string ToString()
            => base.ToString() + "   这个方法是构造函数，无返回值";
        #endregion
        #endregion
        #region 判断方法签名是否兼容
        public override bool IsSame(MethodBaseSignature Other, bool ExactlySame = true)
            => Other is ConstructSignature other && Aided(other, ExactlySame);
        #endregion
        #region 构造函数与创建对象
        #region 创建对象的静态方法
        /// <summary>
        /// 创建一个构造函数签名，该签名的参数和返回值与一个委托签名相同
        /// </summary>
        /// <typeparam name="DelegatesType">委托的类型，注意：如果它具有返回值，会引发异常</typeparam>
        /// <returns></returns>
        public static ConstructSignature Create<DelegatesType>()
            where DelegatesType : Delegate
        {
            var info = typeof(DelegatesType).GetMethod("Invoke")!;
            return info.ReturnType == typeof(void) ?
                new ConstructSignature(info.GetParameters().Select(x => x.ParameterType)) :
                throw new Exception($"委托类型{typeof(DelegatesType)}具有返回值，不能用它初始化构造函数签名");
        }
        #endregion
        #region 指定参数类型列表
        /// <summary>
        /// 使用指定的参数类型列表初始化对象
        /// </summary>
        /// <param name="ParametersType">构造函数的参数类型列表</param>
        public ConstructSignature(params Type[] ParametersType)
            : base(ParametersType)
        {

        }
        #endregion
        #region 指定参数列表
        /// <summary>
        /// 使用指定的参数列表初始化对象
        /// </summary>
        /// <param name="Parameters">构造函数的参数列表</param>
        public ConstructSignature(params object[] Parameters)
            : base(Parameters)
        {

        }
        #endregion
        #endregion
    }
}
