using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.JSInterop
{
    /// <summary>
    /// 这个抽象类是所有JS对象封装的基类
    /// </summary>
    abstract class JSRuntimeBase : IJSRuntime
    {
        #region 封装的JS对象
        /// <summary>
        /// 获取封装的JS运行时对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        protected IJSRuntime JSRuntime { get; }
        #endregion
        #region 执行JS函数
        #region 带取消令牌
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
            => JSRuntime.InvokeAsync<TValue>(identifier, cancellationToken, args);
        #endregion
        #region 不带取消令牌
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
            => JSRuntime.InvokeAsync<TValue>(identifier, args);
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的JS运行时初始化对象
        /// </summary>
        /// <param name="JSRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
        public JSRuntimeBase(IJSRuntime JSRuntime)
        {
            this.JSRuntime = JSRuntime;
        }
        #endregion
    }
}
