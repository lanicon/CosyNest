﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Design;

namespace System
{
    /// <summary>
    /// 表示一个锁，
    /// 它可以利用using语句自动完成某些操作，以避免遗漏
    /// </summary>
    class Lock : AutoRelease
    {
        #region 在释放锁的时候执行的委托
        /// <summary>
        /// 在锁被释放的时候，执行这个委托
        /// </summary>
        private Action? DisposeDel { get; set; }
        #endregion
        #region 释放锁
        protected override void DisposeRealize()
        {
            DisposeDel?.Invoke();
            DisposeDel = null;
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的初始化委托和释放委托初始化对象
        /// </summary>
        /// <param name="Initialization">在构造函数中，这个委托会被立即执行，
        /// 如果为<see langword="null"/>，则会被忽略</param>
        /// <param name="Dispose">在对象被释放的时候，这个委托会被执行</param>
        public Lock(Action? Initialization, Action Dispose)
        {
            Initialization?.Invoke();
            DisposeDel = Dispose;
        }
        #endregion
    }
}
