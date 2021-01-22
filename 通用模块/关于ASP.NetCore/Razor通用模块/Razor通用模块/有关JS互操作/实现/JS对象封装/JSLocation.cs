using System.Threading.Tasks;

namespace Microsoft.JSInterop
{
    /// <summary>
    /// 这个类型是<see cref="IJSLocation"/>的实现，
    /// 可以视为一个JS中的Location对象
    /// </summary>
    class JSLocation : JSRuntimeBase, IJSLocation
    {
        #region 刷新页面
        public ValueTask Reload(bool forceGet = false)
            => JSRuntime.InvokeVoidAsync("location.reload", forceGet);
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的JS运行时初始化对象
        /// </summary>
        /// <param name="JSRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
        public JSLocation(IJSRuntime JSRuntime)
            : base(JSRuntime)
        {

        }
        #endregion
    }
}
