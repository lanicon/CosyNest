﻿using System.Collections.Generic;
using System.Linq;

namespace System
{
    /// <summary>
    /// 表示由于类型非法所引发的异常
    /// </summary>
    public class ExceptionTypeUnlawful : InvalidCastException
    {
        #region 检查类型的合法性
        /// <summary>
        /// 检查一个对象，如果它不是合法类型，则抛出一个异常
        /// </summary>
        /// <param name="Check">待检查的类型，如果它不是<see cref="Type"/>，则会获取它的类型</param>
        /// <param name="Legal">如果<paramref name="Check"/>不能赋值给这个集合中的任何一个类型，则会引发异常</param>
        public static void Check(object Check, params object[] Legal)
        {
            var type = Check.GetTypeObj();
            if (Legal.Select(x => x.GetTypeObj()).
                All(x => !x.IsAssignableFrom(type)))
            {
                throw new ExceptionTypeUnlawful(Check, Legal);
            }
        }
        #endregion
        #region 非法类型
        /// <summary>
        /// 获取引发异常的非法类型
        /// </summary>
        public Type UnlawfulType { get; }
        #endregion
        #region 合法类型
        /// <summary>
        /// 获取受支持的合法类型
        /// </summary>
        public IEnumerable<Type> LegalType { get; }

        /*注释：
          合法类型可以为多个的原因在于：
          有时候程序会支持多种合法类型，
          这个问题在解析表达式树的时候表现得非常明显，
          而这样设计会为这种情况提供方便*/
        #endregion
        #region 构造方法
        /// <summary>
        /// 用指定的非法和合法类型初始化异常
        /// </summary>
        /// <param name="UnlawfulType">引发异常的非法类型，
        /// 如果它是<see cref="Type"/>，则返回它本身，否则调用<see cref="object.GetType"/>方法获取它的类型</param>
        /// <param name="LegalType">受支持的合法类型，可以为多个，
        /// 它的元素会通过与上一个参数相同的方法转换为<see cref="Type"/></param>
        public ExceptionTypeUnlawful(object? UnlawfulType, params object[] LegalType)
            : base(new[] { "错误原因：类型非法，且无法转换为合法类型" ,
            $"引发异常的非法类型：{(object?)UnlawfulType?.GetTypeObj() ?? "null"}",
            $"受支持的合法类型：{LegalType.Join(x => x.GetTypeObj().ToString(), "，")}"}.
                  Join(Environment.NewLine))
        {

            this.UnlawfulType = UnlawfulType?.GetTypeObj() ?? typeof(object);
            this.LegalType = LegalType.Select(x => x.GetTypeObj());
        }
        #endregion
    }
}
