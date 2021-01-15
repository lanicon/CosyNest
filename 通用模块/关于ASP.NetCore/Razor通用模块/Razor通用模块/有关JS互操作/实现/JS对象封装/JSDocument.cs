
using System.Collections.Generic;

namespace Microsoft.JSInterop
{
    /// <summary>
    /// 这个类型是<see cref="IJSDocument"/>的实现，
    /// 可以视为一个JS中的Document对象
    /// </summary>
    class JSDocument : JSRuntimeBase, IJSDocument
    {
        #region 返回索引Cookie的字典
        private JSCookie? CookieField;

        public IAsyncDictionary<string, string> Cookie
            => CookieField ??= new(JSRuntime);
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的JS运行时初始化对象
        /// </summary>
        /// <param name="JSRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
        public JSDocument(IJSRuntime JSRuntime)
            : base(JSRuntime)
        {

        }
        #endregion
    }
}
