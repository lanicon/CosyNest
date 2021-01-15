using System;
using System.Collections.Generic;
using System.Text;

namespace System.Design.Direct
{
    /// <summary>
    /// 表示由于架构中的属性未找到所引发的异常
    /// </summary>
    public class ExceptionSchemaNotFound : ExceptionSchema
    {
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="PropertyName">发生异常的属性名称</param>
        /// <param name="Message">对错误的说明，
        /// 如果为<see langword="null"/>，则使用默认说明</param>
        public ExceptionSchemaNotFound(string PropertyName, string? Message = null)
            : base(PropertyName, Message ?? $"由于属性{PropertyName}未找到，所以架构无法兼容")
        {

        }
        #endregion
    }
}
