using System.Threading.Tasks;

namespace Microsoft.JSInterop
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个JS中的Location对象
    /// </summary>
    public interface IJSLocation
    {
        #region 刷新页面
        /// <summary>
        /// 刷新当前页面
        /// </summary>
        /// <param name="forceGet">如果该参数为<see langword="true"/>，
        /// 则绕过缓存，直接从服务器下载页面</param>
        /// <returns></returns>
        ValueTask Reload(bool forceGet = false);
        #endregion
    }
}
