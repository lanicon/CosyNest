using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Design;

namespace System.Reflection
{
    /// <summary>
    /// 这个类型表示方法和构造函数的签名
    /// </summary>
    public abstract class MethodBaseSignature
    {
        #region 参数列表
        /// <summary>
        /// 获取方法的参数列表
        /// </summary>
        public IReadOnlyList<Type> Parameters { get; }
        #endregion
        #region 重写的方法
        #region 重写GetHashCode
        public override int GetHashCode()
            => ToolEqual.CreateHash(Parameters.ToArray());
        #endregion
        #region 重写Equals
        public override bool Equals(object? obj)
            => obj is MethodBaseSignature m &&
            IsSame(m);
        #endregion
        #region 重写ToString
        public override string ToString()
            => Parameters.Any() ?
                $"参数:{Parameters.Join(x => x.Name, "，")}" : "无参数";
        #endregion
        #endregion
        #region 判断方法签名是否兼容
        #region 正式方法
        /// <summary>
        /// 判断这个签名和另一个方法签名是否兼容
        /// </summary>
        /// <param name="Other">要比较的另一个签名</param>
        /// <param name="ExactlySame">如果这个值为<see langword="true"/>，则要求两个签名完全相同，
        /// 如果为<see langword="false"/>，则可以支持协变与逆变</param>
        /// <returns></returns>
        public abstract bool IsSame(MethodBaseSignature Other, bool ExactlySame = true);

        /*实现本API请遵循以下规范：
           如果ExactlySame为假，那么签名兼容指的是：
           本对象的返回值和参数可以用在Other身上，而不是相反*/
        #endregion
        #region 辅助方法
        /// <summary>
        /// 用来比较类型的辅助方法，支持协变
        /// </summary>
        /// <param name="Other">要比较的另一个方法签名</param>
        /// <param name="ExactlySame">如果这个值为<see langword="true"/>，则要求两个签名完全相同，
        /// 如果为<see langword="false"/>，则可以支持协变与逆变</param>
        /// <returns></returns>
        private protected bool Aided(MethodBaseSignature Other, bool ExactlySame)
        {
            var otherPar = Other.Parameters;
            if (otherPar.Count != Parameters.Count)
                return false;
            return Parameters.Zip(otherPar, false).All(x =>
                  {
                      var (s, o) = x;
                      return ExactlySame ? s == o : o?.IsAssignableFrom(s) ?? false;
                  });
        }
        #endregion
        #endregion
        #region 构造函数
        #region 指定参数类型列表
        /// <summary>
        /// 使用指定的参数类型列表初始化对象
        /// </summary>
        /// <param name="ParametersType">指定的参数类型列表</param>
        public MethodBaseSignature(params Type[] ParametersType)
        {
            Parameters = ParametersType;
        }
        #endregion
        #region 指定参数列表
        /// <summary>
        /// 用指定的参数列表初始化对象
        /// </summary>
        /// <param name="Parameters">方法的参数列表</param>
        public MethodBaseSignature(params object[] Parameters)
            : this(Parameters.Select(x => x.GetTypeObj()).ToArray())
        {

        }
        #endregion
        #endregion
    }
}
