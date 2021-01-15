using System;

namespace System.Design
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以显式释放非托管资源，
    /// 并且可以告诉调用者自己是否已被释放
    /// </summary>
    public interface IDisposablePro : IDisposable
    {
        #region 指示是否被释放
        /// <summary>
        /// 如果这个值为<see langword="true"/>，代表该对象已被释放
        /// </summary>
        bool IsFree { get; }
        #endregion
    }
}
