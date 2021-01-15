using System.Collections.Generic;

namespace Microsoft.JSInterop
{
    /// <summary>
    /// 这个类型是<see cref="IJSWindow"/>的实现，
    /// 可以视为一个Window对象
    /// </summary>
    class JSWindow : JSRuntimeBase, IJSWindow
    {
        #region 返回Document对象
        private IJSDocument? DocumentField;

        public IJSDocument Document
            => DocumentField ??= new JSDocument(JSRuntime);
        #endregion
        #region 返回本地存储对象
        private JSLocalStorage? LocalStorageField;

        public IAsyncDictionary<string, string> LocalStorage
             => LocalStorageField ??= new(JSRuntime);
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的JS运行时初始化对象
        /// </summary>
        /// <param name="JSRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
        public JSWindow(IJSRuntime JSRuntime)
            : base(JSRuntime)
        {

        }
        #endregion
    }
}
