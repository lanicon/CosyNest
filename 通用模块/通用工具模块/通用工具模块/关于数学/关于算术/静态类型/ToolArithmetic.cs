using System;
using System.Linq;

namespace System.Maths
{
    /// <summary>
    /// 关于算术和初等数学的工具类
    /// </summary>
    public static class ToolArithmetic
    {
        #region 关于运算
        #region 求一个数字的幂
        /// <summary>
        /// 求一个数字的幂
        /// </summary>
        /// <param name="Num">求幂的底数</param>
        /// <param name="Index">求幂的指数，暂时只支持整型，默认为求平方</param>
        /// <returns></returns>
        public static Num Pow(Num Num, int Index = 2)
        {
            if (Index == 0)
                return Num == 0 ? throw new Exception("不能计算0的0次方") : 1;
            Num Get(Num num, int index)
               => index == 1 ? num : Num * Get(num, index - 1);
            var Results = Get(Num, Abs(Index));
            return Index > 0 ? Results : 1 / Results;

            /*注释：求幂的约定：
               1.任何不等于零的数的零次幂都等于1
               2.任何不等于零的数的-p（p是正整数）次幂，等于这个数的p次幂的倒数。*/
        }
        #endregion
        #region 整除运算
        /// <summary>
        /// 将两个对象a和b相除，并返回一个元组，
        /// 元组的第一个项是相除得出的商（只保留整数），第二个项是余数，第三个项是是否除尽
        /// </summary>
        /// <param name="Numa">对象a，可以是任何能够同时进行减法，取余，除法运算的类型</param>
        /// <param name="Numb">对象b，可以是任何能够同时进行减法，取余，除法运算的类型</param>
        /// <returns></returns>
        public static (Num Divisor, Num Remainder, bool IsDivisor) Div(Num Numa, Num Numb)
        {
            var dic = Numa % Numb;
            var rem = (Numa - dic) / Numb;
            return (rem, dic, dic == 0);
        }
        #endregion
        #region 求最大公约数
        /// <summary>
        /// 求多个整数的最大公约数
        /// </summary>
        /// <param name="First">第一个整数</param>
        /// <param name="Second">第二个整数</param>
        /// <param name="Num">如果还有更多的整数，则在这里传入</param>
        /// <returns></returns>
        public static Num GCD(Num First, Num Second, params Num[] Num)
            => Num.Union(new[] { First, Second }).Aggregate((max, min) =>
                 {
                     //注意：本方法使用的算法是辗转相除法
                     if (max < min)
                         Tool.Exchange(ref max, ref min);
                     #region 本地函数
                     static Num Get(Num Left, Num Right)
                            => Right == 0 ? Left : Get(Right, Left % Right);
                     #endregion
                     return Get(max, min);
                 });

        #endregion
        #region 求最小公倍数
        /// <summary>
        /// 求多个整数的最小公倍数
        /// </summary>
        /// <param name="First">第一个整数</param>
        /// <param name="Second">第二个整数</param>
        /// <param name="Num">如果还有更多的整数，则在这里传入</param>
        /// <returns></returns>
        public static Num LCM(Num First, Num Second, params Num[] Num)
            => Num.Union(new[] { First, Second }).Aggregate(
                (x, y) => x * y / GCD(x, y));
        #endregion
        #region 返回两个数字的比
        /// <summary>
        /// 以整数形式返回两个非零数字的比
        /// </summary>
        /// <param name="NumA">数字A</param>
        /// <param name="NumB">数字B</param>
        /// <returns></returns>
        public static (int NumA, int NumB) Proportion(Num NumA, Num NumB)
        {
            #region 本地函数
            static int Get(Num num, int pre)
                => num * Pow(10, pre);
            #endregion
            Num pow = Limit(true, Pre(NumA).Decimal, Pre(NumB).Decimal);
            var (A, B) = (Get(NumA, pow), Get(NumB, pow));                  //首先将两个数字全部化成比例相等的整数
            var GCD = ToolArithmetic.GCD(A, B);                                         //然后求出它们的最大公约数
            return (A / GCD, B / GCD);                                                      //再除以最大公约数，得出比值
        }
        #endregion
        #endregion
        #region 关于转化数字
        #region 求数字的绝对值
        /// <summary>
        /// 求一个数字的绝对值
        /// </summary>
        /// <param name="num">要求绝对值的数字</param>
        /// <returns></returns>
        public static Num Abs(Num num)
            => num >= 0 ? num : -num;
        #endregion
        #region 将一个数转化为正数或负数
        /// <summary>
        /// 将一个数字转化为正数或负数
        /// </summary>
        /// <param name="Num">要转化的数字</param>
        /// <param name="ToPositive">指示转化的行为，如果为<see langword="true"/>，转化为正数，
        /// 为<see langword="false"/>，转化为负数，为<see langword="null"/>，则将正负数取反</param>
        /// <returns></returns>
        public static Num Reve(Num Num, bool? ToPositive)
        {
            var IsPositive = Num > 0;                              //判断数字是否是正数
            var toPositive = ToPositive ?? !IsPositive;        //判断应该转化为正数还是负数
            return IsPositive == toPositive ? Num : -Num;
        }
        #endregion
        #region 分割数字
        /// <summary>
        /// 将一个数字按照权重分割
        /// </summary>
        /// <param name="Num">待分割的数字</param>
        /// <param name="Weight">分割数字的权重</param>
        /// <returns></returns>
        public static Num[] Segmentation(Num Num, params Num[] Weight)
        {
            var Atomic = Num / Weight.Sum();
            return Weight.Select(x => Atomic * x).ToArray();
        }
        #endregion
        #region 将数字的整数部分和小数部分拆分
        /// <summary>
        /// 将一个数字的整数部分和小数部分拆分，并返回一个元组，
        /// 第一个项是整数部分，第二个项是小数部分，
        /// 第三个项是小数部分是否等于0，也就是这个数字是不是整数
        /// </summary>
        /// <param name="Num">要拆分的数字</param>
        /// <returns></returns>
        public static (Num Integer, Num Decimal, bool IsInt) Split(Num Num)
        {
            var Integer = decimal.Truncate(Num);
            var Decimal = Num - Integer;
            return (Integer, Decimal, Decimal == 0);
        }
        #endregion
        #endregion
        #region 关于近似值和精度
        #region 返回数字的整数位数和小数位数
        /// <summary>
        /// 返回一个元组，指示一个数字的整数位数和小数位数
        /// </summary>
        /// <param name="Num">要检查的数字</param>
        /// <returns></returns>
        public static (int Integer, int Decimal) Pre(Num Num)
        {
            var (i, d, IsInt) = Split(Reve(Num, true));
            var Integer = i == 0 ? 1 : Math.Log10(i) + 1;
            #region 求小数位数的本地函数
            int Get()
            {
                for (int Decimal = 1; true; Decimal++)
                    if (Split(d *= 10).IsInt)
                        return Decimal;
            }
            #endregion
            return ((int)Integer, IsInt ? 0 : Get());
        }
        #endregion
        #region 将数字抹平
        /// <summary>
        /// 将数字在指定位数抹平，
        /// 并将后面的位数用0填补，然后返回一个元组，
        /// 分别是抹平后的数字和被抹掉的零头
        /// </summary>
        /// <param name="Num">要抹平的数字</param>
        /// <param name="digits">如果这个值为正数，代表抹平N位整数，
        /// 为负数，代表抹平N位小数精度</param>
        /// <returns></returns>
        public static (Num Num, Num Remainder) Fla(Num Num, int digits)
        {
            var Remainder = Num % Pow(10, digits > 0 ? digits - 1 : digits);        //需要这个三元表达式的原因在于：10的0次方才是1，而不是1次方
            return (Num - Remainder, Remainder);

            /*注意：
               如果抹平的整数位数大于数字的整数位数，
               则会返回0，例如：将100抹平4位整数，就会返回0，
               但抹平小数位数时不会发生这种情况*/
        }
        #endregion
        #region 对数字取近似值
        #region 辅助方法
        /// <summary>
        /// 求近似值的辅助方法，输入数字的零头，
        /// 返回一个布尔值，如果为<see langword="true"/>代表应该进1，为<see langword="false"/>代表应该去尾
        /// </summary>
        /// <param name="rem">数字的零头，例如：如果对3.14取1位小数精度，那么零头就是0.04</param>
        /// <param name="Mod">如果这个值为<see langword="true"/>，代表用进一法取近似值，为<see langword="null"/>，代表用四舍五入</param>
        /// <returns></returns>
        private static bool SimAided(Num rem, bool? Mod)
        {
            var text = rem.ToString().TrimStart('0', '.').First();
            var num = Convert.ToInt32(text.ToString());                //获取第一位非0非小数点字符
            return Mod == true ?                                    //在这里不需要判断是否为去尾法，因为在Sim中已经判断过
                num != 0 : num >= 5;
        }
        #endregion
        #region 正式方法
        /// <summary>
        /// 求一个数字的近似值
        /// </summary>
        /// <param name="Num">需要求近似值的数字</param>
        /// <param name="Pre">如果这个值为正数或0，代表取N位整数精度，
        /// 为负数，代表取N位小数精度</param>
        /// <param name="IsProgressive">如果这个值为<see langword="true"/>，用进一法取近似值，
        /// 为<see langword="false"/>，用去尾法，为<see langword="null"/>，用四舍五入法</param>
        /// <returns></returns>
        public static Num Sim(Num Num, int Pre = 0, bool? IsProgressive = true)
        {
            var (fla, rem) = Fla(Num, Pre);            //将数字抹平
            if (fla == 0 || rem == 0)                           //如果数字的位数比要求的精度要低，则直接返回，不需要取近似值
                return Num;
            return IsProgressive == false || !SimAided(rem, IsProgressive) ?        //如果用去尾法取近似值，或经过判断后应该去尾
                fla : fla + Pow(10, Pre > 0 ? Pre - 1 : Pre);                                                     //则返回去尾后的近似值，否则返回进1后的近似值
        }
        #endregion
        #endregion
        #endregion
        #region 关于比较
        #region 返回极限
        /// <summary>
        /// 返回若干个对象的极限，也就是它们当中最大或最小的值
        /// </summary>
        /// <typeparam name="Obj">要返回极限的对象</typeparam>
        /// <param name="RetMax">如果这个值为<see langword="true"/>，返回集合的最大值，为<see langword="false"/>，返回最小值</param>
        /// <param name="Objs">要返回极限的对象</param>
        /// <returns></returns>
        public static Obj Limit<Obj>(bool RetMax, params Obj[] Objs)
            => Objs.Limit(x => x, RetMax);
        #endregion
        #endregion
        #region 关于相对值
        /// <summary>
        /// 设两个数字A和B，
        /// 如果要求返回绝对值，则直接返回B，
        /// 如果要求返回相对值，则返回A+B
        /// </summary>
        /// <param name="NumA">数字A</param>
        /// <param name="NumB">数字B，如果为0，在相对值下代表和数字A相等</param>
        /// <param name="IsAbs">如果这个值为<see langword="true"/>，则返回绝对值，为<see langword="false"/>，则返回相对值</param>
        /// <returns></returns>
        public static Num Rel(Num NumA, Num NumB, bool IsAbs)
            => IsAbs ? NumB : NumA + NumB;
        #endregion
    }
}
