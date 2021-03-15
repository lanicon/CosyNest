﻿namespace System.Design.Direct
{
    /// <summary>
    /// 表示由于架构中的属性类型不正确所引发的异常
    /// </summary>
    public class ExceptionSchemaType : ExceptionSchema
    {
        #region 属性类型
        /// <summary>
        /// 获取该属性目前的类型，
        /// 如果属性为<see langword="null"/>，
        /// 则取<see cref="object"/>的类型
        /// </summary>
        public Type PropertyType { get; }
        #endregion
        #region 合法类型
        /// <summary>
        /// 获取该属性的合法类型
        /// </summary>
        public Type LegalType { get; }
        #endregion
        #region 构造函数
        #region 指定非法类型
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="PropertyName">发生异常的属性名称</param>
        /// <param name="PropertyType">属性目前的类型，
        /// 如果属性为<see langword="null"/>，则取<see cref="object"/>的类型</param>
        /// <param name="LegalType">属性的合法类型</param>
        /// <param name="Message">对错误的说明，
        /// 如果为<see langword="null"/>，则使用默认说明</param>
        public ExceptionSchemaType(string PropertyName, Type PropertyType, Type LegalType, string? Message = null)
            : base(PropertyName, Message ?? $"属性{PropertyName}的合法类型为{LegalType}，但实际类型为{PropertyType}，类型非法")
        {
            this.PropertyType = PropertyType;
            this.LegalType = LegalType;
        }
        #endregion
        #region 指定非法值
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="PropertyName">发生异常的属性名称</param>
        /// <param name="PropertyValue">属性目前的非法值</param>
        /// <param name="LegalType">属性的合法类型</param>
        /// <param name="Message">对错误的说明，
        /// 如果为<see langword="null"/>，则使用默认说明</param>
        public ExceptionSchemaType(string PropertyName, object? PropertyValue, Type LegalType, string? Message = null)
            : this(PropertyName, PropertyValue?.GetType() ?? typeof(object), LegalType, Message)
        {

        }
        #endregion
        #endregion
    }
}
