
using System.Collections.Generic;

namespace Microsoft.JSInterop
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以作为JS中的Document对象的Net封装
    /// </summary>
    public interface IJSDocument
    {
        #region 返回Cookie对象
        /// <summary>
        /// 返回一个字典，它可以用来索引Cookie
        /// </summary>
        IAsyncDictionary<string, string> Cookie { get; }
        #endregion
    }
}
