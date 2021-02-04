using System;

namespace System.Design
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以显式释放非托管资源，
    /// 并且可以告诉调用者自己是否仍然可用
    /// </summary>
    public interface IDisposablePro : IDisposable
    {
        #region 指示是否被释放
        /// <summary>
        /// 如果这个值为<see langword="true"/>，代表该对象仍然可以使用，
        /// 否则代表对象不可用，试图访问它可能会引发异常
        /// </summary>
        bool IsAvailable { get; }
        #endregion
    }
}
