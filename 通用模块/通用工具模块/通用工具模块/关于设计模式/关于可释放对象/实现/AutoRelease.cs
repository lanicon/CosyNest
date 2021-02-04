using System;

namespace System.Design
{
    /// <summary>
    /// 这个类型可以在被回收时自动释放非托管资源
    /// </summary>
    public abstract class AutoRelease : IDisposablePro
    {
        #region 说明文档
        /*问：为什么本类型要在析构函数中释放非托管资源？
          C#规范不是不推荐这种做法吗？
          答：作者经过斟酌后认为这样做是值得的，
          因为内存泄露是一个熵增过程，一旦出现就只会加重不会缓解，必须尽一切努力避免，
          而在析构函数中释放对象虽然会产生老化问题，但它可以保证对象最后仍然会被回收*/
        #endregion
        #region 关于释放对象
        #region 正式方法
        public void Dispose()
        {
            if (IsAvailable)
            {
                IsAvailable = false;
                GC.SuppressFinalize(this);
                DisposeRealize();
            }
        }
        #endregion
        #region 模板方法
        /// <summary>
        /// 释放资源的实际操作在这个函数中
        /// </summary>
        protected abstract void DisposeRealize();
        #endregion
        #endregion
        #region 指示对象是否可用
        public bool IsAvailable { get; private set; } = true;
        #endregion
        #region 析构函数
        /// <summary>
        /// 在本对象被回收时，自动释放非托管资源
        /// </summary>
        ~AutoRelease()
        {
            Dispose();
        }
        #endregion
    }
}
