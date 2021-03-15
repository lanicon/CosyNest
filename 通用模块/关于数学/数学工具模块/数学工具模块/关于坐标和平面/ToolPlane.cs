using System.Linq;

using static System.Maths.CreateMathObj;

using static System.Maths.ToolArithmetic;

namespace System.Maths
{
    /// <summary>
    /// 关于坐标和平面的工具类
    /// </summary>
    public static class ToolPlane
    {
        #region 关于IPoint
        #region 返回相对位置
        /// <summary>
        /// 以一个点为原点，
        /// 返回另一个点相对于它的位置
        /// </summary>
        /// <param name="Original">原点</param>
        /// <param name="Relative">另一个点，注意：它是绝对坐标</param>
        /// <returns></returns>
        public static PointPos RelativePos(IPoint Original, IPoint Relative)
        {
            var (OR, OT) = Original;
            var (RR, RT) = Relative;
            var EqualsR = OR == RR;
            var EqualsT = OT == RT;
            if (EqualsR && EqualsT)
                return PointPos.Coincidence;
            var ComT = RT > OT;
            if (EqualsR)
                return ComT ? PointPos.Above : PointPos.Below;
            var ComR = RR > OR;
            if (EqualsT)
                return ComR ? PointPos.Right : PointPos.Left;
            if (ComR)
                return ComT ? PointPos.Q1 : PointPos.Q4;
            return ComT ? PointPos.Q2 : PointPos.Q3;
        }
        #endregion
        #endregion
        #region 关于ISize及其衍生类
        #region 排列一个矩形
        /// <summary>
        /// 传入一个矩形大小，将它在一个空间中按照水平或垂直方向排列，
        /// 如有超出则会换行，并返回第N个矩形的坐标，
        /// 注意：这个方法假定排列空间的下端或右端是无限的
        /// </summary>
        /// <param name="Size">要排列的<see cref="ISize"/></param>
        /// <param name="N">指示排列到第N个矩形，从1开始</param>
        /// <param name="IsVertical">如果这个值为<see langword="true"/>，则垂直排列，否则水平排列</param>
        /// <param name="Starting">指定开始排列的原点，如果为<see langword="null"/>，则默认为(0,0)</param>
        /// <param name="Wrapping">在排列过程中，如果坐标超出这个上限，
        /// 就会根据参数<paramref name="IsVertical"/>决定应该换行还是换列，如果是默认值，则代表永不换行或换列</param>
        /// <returns></returns>
        public static IPoint Arranged(ISize Size, int N, bool IsVertical = true, IPoint? Starting = null, Num Wrapping = default)
        {
            var (W, H) = Size;
            var IsDef = Equals(Wrapping, 0);
            #region 本地函数
            int RC(Num num)                                    //辅助方法， 输入宽或高，计算出所处的行列数
                => IsDef ? N : (int)Sim(num * N / Wrapping);
            /*注释：
              1.这个本地函数要将num转换为分数的原因在于：
              如果num是Int，在相除的时候会错误地用去尾法取近似值，
              但这个情况下要求使用进一法*/
            #endregion
            var Row = IsVertical ? RC(H) : 0;
            var Col = IsVertical ? 0 : RC(W);
            return Point(Col * W, Row * H).ToRel(Starting ?? IPoint.Original);
        }
        #endregion
        #region 返回平面的界限
        #region 传入IPoint
        /// <summary>
        /// 传入若干个坐标，
        /// 返回容纳它们所需要的最小平面
        /// </summary>
        /// <param name="Points">传入的坐标</param>
        /// <returns></returns>
        public static ISizePos Boundaries(params IPoint[] Points)
        {
            switch (Points.Length)
            {
                case 0:
                    return SizePos(0, 0, 0, 0);
                case 1:
                    return SizePos(Points[0], 0, 0);
                default:
                    var (lx, ly) = Points[0];
                    var (rx, ry) = (lx, ly);
                    foreach (var p in Points[1..])
                    {
                        var (x, y) = p;
                        if (x < lx)
                            lx = x;
                        else if (x > rx)
                            rx = x;
                        if (y > ly)
                            ly = y;
                        else if (y < ry)
                            ry = y;
                    }
                    return SizePos(Point(lx, ly), Point(rx, ry));
            }
        }
        #endregion
        #region 传入ISizePos
        /// <summary>
        /// 返回一个平面数组的界限，
        /// 也就是容纳数组中所有平面，所需要的最小矩形
        /// </summary>
        /// <typeparam name="Num">描述矩形大小的数组类型</typeparam>
        /// <param name="Plane">要返回界限的平面数组</param>
        /// <returns></returns>
        public static ISizePos Boundaries(params ISizePos[] Plane)
        {
            var Point = Plane.Select(x =>
            {
                var (left, right) = x.Boundaries;
                return new[] { left, right };
            }).UnionNesting(false);
            return Boundaries(Point.ToArray());
        }
        #endregion 
        #endregion
        #endregion
    }
    #region 关于象限的枚举
    /// <summary>
    /// 以一个点O为原点，
    /// 这个枚举指示另一个点P相对于它的位置
    /// </summary>
    public enum PointPos
    {
        /// <summary>
        /// 点P处于第一象限
        /// </summary>
        Q1,
        /// <summary>
        /// 点P处于第二象限
        /// </summary>
        Q2,
        /// <summary>
        /// 点P处于第三象限
        /// </summary>
        Q3,
        /// <summary>
        /// 点P处于第四象限
        /// </summary>
        Q4,
        /// <summary>
        /// 两个点都位于Y轴，而且P在O的上面
        /// </summary>
        Above,
        /// <summary>
        /// 两个点都位于Y轴，而且P在O的下面
        /// </summary>
        Below,
        /// <summary>
        /// 两个点都位于X轴，而且P在O的左边
        /// </summary>
        Left,
        /// <summary>
        /// 两个点都位于X轴，而且P在O的右边
        /// </summary>
        Right,
        /// <summary>
        /// 两个点完全重合
        /// </summary>
        Coincidence
    }
    #endregion
}
