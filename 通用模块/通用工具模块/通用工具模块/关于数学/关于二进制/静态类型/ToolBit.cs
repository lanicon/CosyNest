using System;
using System.Collections.Generic;
using System.Linq;

using static System.ExceptionIntervalOut;
using static System.Maths.ToolArithmetic;

namespace System.Maths
{
    /// <summary>
    /// 关于位运算与非十进制的工具类
    /// </summary>
    public static class ToolBit
    {
        #region 关于进制转换
        #region 从十进制转换为其他进制
        /// <summary>
        /// 将一个十进制数转换为其他进制，并返回一个元组，
        /// 它的项分别是新数字的整数部分和小数部分，以及每一位数字的值
        /// </summary>
        /// <param name="num">待转换的十进制数</param>
        /// <param name="Bit">目标数字的位权，也就是几位数</param>
        /// <param name="Progress">如果转换后的新数字不是整数，则这个参数指示其最大精度</param>
        /// <returns></returns>
        public static (List<int> Integer, List<int> Decimal) FromDecimal(Num num, int Bit = 2, int Progress = 6)
        {
            #region 本地函数
            int Get(int Index)
                => num < Pow(Bit, Index) ? Index : Get(++Index);
            #endregion
            var MaxBit = Get(1) - 1;                                        //首先求出转换后整数的位数
            var (I, D) = (new List<int>(), new List<int>());
            while (D.Count <= Progress)
            {
                var (div, rem, _) = Div(num, Pow(Bit, MaxBit));
                (MaxBit >= 0 ? I : D).Add(div);
                num = rem;
                if (num == 0)                           //如果待转换的数被除尽，则停止循环
                {
                    if (MaxBit > 0)                     //如果数字被除尽，但是还有多余的位数
                        I.AddRange(Enumerable.Repeat(0, MaxBit));               //则将这些位数补0
                    break;
                }
                MaxBit--;
            }
            return (I, D);
        }
        #endregion
        #region 从其他进制转换为十进制
        /// <summary>
        /// 将其他进制的数字转换为十进制
        /// </summary>
        /// <param name="Bit">待转换的数字的位权，也就是几进制数</param>
        /// <param name="NumInteger">枚举待转换数字整数部分每一位的值，如果为<see langword="null"/>，代表整数部分为0</param>
        /// <param name="NumDecimal">枚举待转换数字小数部分每一位的值，如果为<see langword="null"/>，代表没有小数部分</param>
        /// <returns></returns>
        public static Num ToDecimal(int Bit, IEnumerable<int>? NumInteger, IEnumerable<int>? NumDecimal)
        {
            NumInteger ??= new[] { 0 };
            NumDecimal ??= Array.Empty<int>();
            var MaxBit = NumInteger.Count();
            return NumInteger.Union(false, NumDecimal).Aggregate((Num)0, (seed, source) =>
            {
                MaxBit--;
                return Pow(Bit, MaxBit) * source + seed;
            });
        }
        #endregion
        #region 将任意进制的数字互相转换
        /// <summary>
        /// 将任意进制的数字互相转换
        /// </summary>
        /// <param name="FromBit">待转换的数字的位权，也就是几进制数</param>
        /// <param name="FromInteger">枚举待转换数字整数部分每一位的值，如果为<see langword="null"/>，代表整数部分为0</param>
        /// <param name="FromDecimal">枚举待转换数字小数部分每一位的值，如果为<see langword="null"/>，代表没有小数部分</param>
        /// <param name="ToBit">转换目标数字的位权</param>
        /// <param name="Progress">如果转换后的新数字不是整数，则这个参数指示其最大精度</param>
        /// <returns></returns>
        public static (List<int> Integer, List<int> Decimal) ToOther(int FromBit, IEnumerable<int>? FromInteger, IEnumerable<int>? FromDecimal,
            int ToBit, int Progress = 6)
        {
            var dec = ToDecimal(FromBit, FromInteger, FromDecimal);
            return ToolBit.FromDecimal(dec, ToBit, Progress);
        }
        #endregion
        #endregion
        #region 关于位域
        #region 枚举整数的所有位域
        /// <summary>
        /// 枚举一个整数的所有位域，类似设置了<see cref="FlagsAttribute"/>的<see cref="Enum"/>，
        /// 集合元素是一个元组，它的项分别是位域的幂和指数
        /// </summary>
        /// <param name="Num">要枚举位域的整数</param>
        /// <param name="MaxIndex">指定要检查的最大数字，单位是2的N次幂</param>
        /// <returns></returns>
        public static IEnumerable<(int Power, int Index)> AllFlag(int Num, int MaxIndex = 30)
        {
            Check(1, null, Num);
            Check(0, null, MaxIndex = Math.Min(MaxIndex, 30));      //最大指数为30是因为int.MaxValue=2^31-1
            for (int Power = 1, Index = 0; Index <= MaxIndex; Index++, Power <<= 1)
            {
                if ((Power & Num) == Power)
                    yield return (Power, Index);
            }
        }
        #endregion
        #region 创建位域
        /// <summary>
        /// 创建一个位域，并返回
        /// </summary>
        /// <param name="Indexs">该位域所兼容的以2为底的指数</param>
        /// <returns></returns>
        public static int CreateFlag(params int[] Indexs)
        {
            Indexs = Indexs.Distinct().ToArray();
            Indexs.ForEach(x => Check(0, 30, x));
            return Indexs.Select(x => (int)Math.Pow(2, x)).Aggregate((x, y) => x | y);
        }
        #endregion
        #endregion
    }
}
