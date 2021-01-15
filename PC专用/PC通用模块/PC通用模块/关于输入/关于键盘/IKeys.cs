using System;
using System.Collections.Generic;
using System.Text;

namespace System.Underlying.PC.Input
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为键盘上的一个按键
    /// </summary>
    public interface IKeys
    {
        #region 按名称索引所有支持的按键
        /// <summary>
        /// 这个字典的键是键盘按键的名称，
        /// 值是所有受当前硬件支持的键盘按键
        /// </summary>
        IReadOnlyDictionary<string, IKeys> AllKeys { get; }
        #endregion
        #region 按键的名称
        /// <summary>
        /// 获取键盘按键的名称
        /// </summary>
        string Name { get; }
        #endregion
        #region 关于按下按键
        #region 短按
        /// <summary>
        /// 按下该键盘按键，并立即松开
        /// </summary>
        void Press();
        #endregion
        #region 长按
        /// <summary>
        /// 长按该键盘按键
        /// </summary>
        void PressLong();
        #endregion
        #region 松开按键
        /// <summary>
        /// 松开被长按的键盘按键
        /// </summary>
        void Loosen();
        #endregion
        #region 组合键
        /// <summary>
        /// 按下组合键，然后立即松开
        /// </summary>
        /// <param name="Other">组合键的其他按键</param>
        void PressCombination(params IKeys[] Other);
        #endregion
        #endregion
    }
}
