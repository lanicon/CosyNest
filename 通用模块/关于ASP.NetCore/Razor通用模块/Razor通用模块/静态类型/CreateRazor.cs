
using Microsoft.JSInterop;

namespace Microsoft.AspNetCore
{
    /// <summary>
    /// 这个静态类可以用来创建和Razor有关的对象
    /// </summary>
    public static class CreateRazor
    {
        #region 创建IJSWindow
        /// <summary>
        /// 创建一个<see cref="IJSWindow"/>，
        /// 它是JS的Window对象的Net封装
        /// </summary>
        /// <param name="JSRuntime">用于执行JS代码的运行时</param>
        /// <returns></returns>
        public static IJSWindow JSWindow(IJSRuntime JSRuntime)
            => new JSWindow(JSRuntime);
        #endregion
    }
}
