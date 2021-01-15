using System;
using System.Linq;

using static System.Maths.CreateBaseMathObj;
using static System.Maths.ToolArithmetic;

namespace System.Maths.Geometric
{
    /// <summary>
    /// 有关三角函数的工具类
    /// </summary>
    public static class ToolTrigonometric
    {
        #region 关于三角形
        #region 关于勾股定理
        #region 判断三个数是否为勾股数
        /// <summary>
        /// 判断三个数是否为勾股数
        /// </summary>
        /// <param name="SideA">第一条边的长度</param>
        /// <param name="SideB">第二条边的长度</param>
        /// <param name="SideC">第三条边的长度</param>
        /// <returns></returns>
        public static bool IsPythagoreanTriple(Num SideA, Num SideB, Num SideC)
        {
            var (H, LLong, LShort) = DistinguishSide(SideA, SideB, SideC);
            return Pow(H) == Pow(LLong) + Pow(LShort);
        }
        #endregion
        #region 计算勾股定理
        /// <summary>
        /// 已知三角形的两边长度，
        /// 利用勾股定理求第三边的长度
        /// </summary>
        /// <param name="SideA">第一条边的长度</param>
        /// <param name="SideB">第二条边的长度</param>
        /// <param name="IsHypotenuse">如果这个值为<see langword="true"/>，代表斜边在前两个参数里面，
        /// 否则代表斜边就是需要计算的第三条边</param>
        /// <returns></returns>
        public static Num CalPythagoreanTriple(Num SideA, Num SideB, bool IsHypotenuse)
        {
            Num pow;                        //这个变量表示需要求值的那一条边的平方
            if (IsHypotenuse)
            {
                var (H, L, _) = DistinguishSide(SideA, SideB, 0);
                pow = Pow(H) - Pow(L);
            }
            else pow = Pow(SideA) + Pow(SideB);
            return Math.Sqrt(pow);
        }
        #endregion
        #region 区分斜边和直角边
        /// <summary>
        /// 传入三角形三边的长度，
        /// 区分哪一边是斜边，哪两条边是直角边
        /// </summary>
        /// <param name="SideA">第一条边的长度</param>
        /// <param name="SideB">第二条边的长度</param>
        /// <param name="SideC">第三条边的长度</param>
        /// <returns>一个元组，它的项分别是斜边，较长的直角边和较短的直角边的长度</returns>
        public static (Num H, Num LLong, Num LShort) DistinguishSide(Num SideA, Num SideB, Num SideC)
        {
            var Array = new[] { SideA, SideB, SideC }.Sort(false);
            return (Array[0], Array[1], Array[2]);
        }
        #endregion
        #endregion
        #region 已知三角形两边长度，求锐角角度
        /// <summary>
        /// 已知一个直角三角形斜边和一条直角边的长度，
        /// 求两个锐角的角度
        /// </summary>
        /// <param name="SideA">已知的第一条边长度</param>
        /// <param name="SideB">已知的第二条边长度，
        /// 这两条边必须有任意一条是斜边</param>
        /// <returns></returns>
        public static (IUnit<IUTAngle> Angle, IUnit<IUTAngle> Diagonal) CalAngle(Num SideA, Num SideB)
        {
            var (H, L, _) = DistinguishSide(SideA, SideB, 0);
            var Angle = Unit(Math.Acos(L / H), IUTAngle.Radian);
            return (Angle, Unit(90, IUTAngle.AngleMetric) - Angle);
        }

        /*说明文档：
          返回的两个角按照以下顺序排列：
          Angle指的是SideA和SideB的夹角，
          而Diagonal指的是除此之外的另一个锐角*/
        #endregion
        #endregion
        #region 三角函数
        #region 返回指定角的余切
        /// <summary>
        /// 返回指定角度的余切
        /// </summary>
        /// <param name="Angle">指定的角度</param>
        /// <returns></returns>
        public static Num Cot(IUnit<IUTAngle> Angle)
        {
            var tan = 1.0 / Math.Tan(Angle.ConvertSingle(IUTAngle.Radian));
            return Math.Round(tan, 6);
        }
        #endregion
        #endregion
        #region 关于角
        #region 返回一个角的方向
        /// <summary>
        /// 如果一个角是直角，返回<see langword="true"/>，是水平角，返回<see langword="false"/>，都不是，返回<see langword="null"/>
        /// </summary>
        /// <param name="Angle">要进行判断的角度或弧度</param>
        /// <returns></returns>
        public static bool? IsRightAngle(IUnit<IUTAngle> Angle)
        {
            var ang = Split(Angle.ConvertSingle(IUTAngle.AngleMetric) / 90);
            return ang.IsInt ? ang.Integer % 2 != 0 : null;
        }
        #endregion
        #endregion
    }
}
