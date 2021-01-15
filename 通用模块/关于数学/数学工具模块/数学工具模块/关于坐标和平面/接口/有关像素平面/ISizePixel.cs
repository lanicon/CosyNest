﻿using System;
using System.Collections.Generic;
using System.Text;

namespace System.Maths
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个像素平面
    /// </summary>
    public interface ISizePixel
    {
        #region 说明文档
        /*问：什么是像素平面？它和普通的ISize有什么不同？
          答：首先，ISize通过坐标计算平面的范围，坐标由两个实数组成，它是可分的，
          而像素平面由不可分的像素平铺组成，但像素本身是一个ISize对象，它可以拥有自己的范围，
          在指定了像素的大小以后，ISizePixel和ISize可以互相转换。
          其次，ISizePixel.PixelCount和ISize.Size具有不同的含义，
          前者指水平和垂直方向的像素数量，后者指平面最大和最小的XY坐标的差，
          如果ISizePixel.PixelCount返回(0,0)，代表该像素平面不存在，
          如果ISize.Size返回(0,0)，代表该平面仅有一个坐标点
        
          问：区分ISize和ISizePixel有什么意义？
          答：它们的适用领域不同，前者更适合进行数学运算，
          后者更适合应用领域，例如排版，Excel等需求使用ISizePixel进行表达更加恰当*/
        #endregion
        #region 返回水平和垂直方向的像素数量
        /// <summary>
        /// 返回一个元组，它的项分别是水平和垂直方向的像素数量
        /// </summary>
        (int Horizontal, int Vertical) PixelCount { get; }

        /*实现本API请遵循以下规范：
          #Horizontal和Vertical不允许负值，
          如果在构造时传入负值，请抛出异常*/
        #endregion
        #region 将像素平面转换为坐标平面
        /// <summary>
        /// 将像素平面转换为坐标平面
        /// </summary>
        /// <param name="PixelSize">每个像素的大小，
        /// 如果为<see langword="null"/>，则默认为(1,1)</param>
        /// <returns></returns>
        ISize ToSize(ISize? PixelSize = null)
        {
            var (H, V) = this;
            var (Width, Height) = PixelSize ?? CreateMathObj.Size(1, 1);
            return CreateMathObj.Size(H * Width, V * Height);
        }
        #endregion
        #region 扩展像素平面
        /// <summary>
        /// 扩展像素平面，并返回扩展后的新平面
        /// </summary>
        /// <param name="ExtendedHorizontal">水平方向扩展的像素数量，
        /// 在加上原有的像素数量后不能为负值</param>
        /// <param name="ExtendedVertical">垂直方向扩展的像素数量，
        /// 在加上原有的像素数量后不能为负值</param>
        /// <returns></returns>
        ISizePixel Transform(int ExtendedHorizontal, int ExtendedVertical)
        {
            var (H, V) = this;
            return CreateMathObj.SizePixel(H + ExtendedHorizontal, V + ExtendedVertical);
        }
        #endregion
        #region 解构像素平面
        /// <summary>
        /// 解构像素平面
        /// </summary>
        /// <param name="Horizontal">用来接收水平方向像素点数量的对象</param>
        /// <param name="Vertical">用来接收垂直方向像素点数量的对象</param>
        void Deconstruct(out int Horizontal, out int Vertical)
        {
            var (H, V) = PixelCount;
            Horizontal = H;
            Vertical = V;
        }
        #endregion
    }
}
