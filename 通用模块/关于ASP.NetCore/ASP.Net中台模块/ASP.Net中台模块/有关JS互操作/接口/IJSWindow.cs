using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.JSInterop
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以作为JS中的Window对象的Net封装
    /// </summary>
    public interface IJSWindow
    {
        #region 返回Document对象
        /// <summary>
        /// 返回JS中的Document对象
        /// </summary>
        IJSDocument Document { get; }
        #endregion
        #region 返回本地存储对象
        /// <summary>
        /// 返回一个字典，它可以用来索引浏览器本地存储
        /// </summary>
        IAsyncDictionary<string, string> LocalStorage { get; }
        #endregion
        #region 弹出消息窗
        /// <summary>
        /// 弹出一个消息窗
        /// </summary>
        /// <param name="message">消息窗的文本</param>
        /// <returns></returns>
        ValueTask Alert(string message);
        #endregion
        #region 打印窗口
        /// <summary>
        /// 打印这个窗口
        /// </summary>
        /// <returns></returns>
        ValueTask Print();
        #endregion
    }
}
