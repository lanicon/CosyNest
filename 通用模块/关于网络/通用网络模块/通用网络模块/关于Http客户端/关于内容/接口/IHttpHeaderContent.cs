using System.Collections.Generic;

namespace System.NetFrancis.Http
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个Http内容的标头
    /// </summary>
    public interface IHttpHeaderContent : IHttpHeader
    {
        #region 获取编码标头
        /// <summary>
        /// 获取Content-Encoding标头值，
        /// 它指定了Http内容的编码
        /// </summary>
        IEnumerable<string> ContentEncoding { get; }
        #endregion
    }
}
