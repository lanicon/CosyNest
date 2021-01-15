using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Maths;

namespace System.DrawingFrancis
{
    /// <summary>
    /// 关于颜色的工具类
    /// </summary>
    public static class ToolColor
    {
        #region 根据16进制字符串，返回Color
        /// <summary>
        /// 根据一个用16进制表示的字符串，创建一个<see cref="IColor"/>并返回
        /// </summary>
        /// <param name="Sys16">指示ARGB值的十六进制字符串，格式为FF001122</param>
        /// <returns></returns>
        public static IColor GetColorFrom16(string Sys16)
        {
            var match = @$"(?<color>{RegexCom.Sys16}{{2}}){{4}}".ToRegex().MatcheFirst(Sys16);
            if (match == null)
                throw new Exception($"{Sys16}不是合法的16进制字符串");
            var ARGB = match["color"].Groups
                .Select(x => Convert.ToByte(x.Match, 16)).ToArray();
            return CreateDrawingObj.Color(ARGB[1], ARGB[2], ARGB[3], ARGB[0]);
        }
        #endregion
        #region 生成随机颜色
        /// <summary>
        /// 生成透明度指定，但RGB随机的颜色
        /// </summary>
        /// <param name="alpha">颜色的透明度</param>
        /// <param name="Rand">用来生成随机数的对象，如果为<see langword="null"/>，则使用一个默认对象</param>
        /// <returns></returns>
        public static IColor RandomColor(byte alpha = 255, IRandom? Rand = null)
        {
            Rand ??= CreateBaseMathObj.RandomOnly;
            byte Get()
                => (byte)Rand!.RandRange(0, 255);
            return CreateDrawingObj.Color(Get(), Get(), Get(), alpha);
        }
        #endregion
    }
}
