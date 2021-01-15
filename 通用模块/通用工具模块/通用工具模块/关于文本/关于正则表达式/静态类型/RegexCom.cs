using System;
using System.Collections.Generic;
using System.Text;

namespace System.Text.RegularExpressions
{
    /// <summary>
    /// 这个静态类储存了一些常用的正则表达式
    /// </summary>
    public static class RegexCom
    {
        #region 十六进制数
        /// <summary>
        /// 返回用于匹配16进制数的正则表达式，
        /// 注意：按照规范，A-F须为大写
        /// </summary>
        public static string Sys16 { get; } = @"[0-9A-Fa-f]";
        #endregion
        #region 正整数
        /// <summary>
        /// 返回匹配正整数的正则表达式
        /// </summary>
        public static string Integ { get; } = @"[0-9]";
        #endregion
        #region 匹配字母
        /// <summary>
        /// 获取匹配字母的表达式文本
        /// </summary>
        /// <param name="MatchingCapital">如果这个值为真，代表匹配大写字母，
        /// 为假，代表匹配小写字母，为null，代表大小写都匹配</param>
        /// <returns></returns>
        public static string Letters(bool? MatchingCapital)
            => MatchingCapital switch
            {
                true => @"[A-Z]",
                false => @"[a-z]",
                null => @"[a-zA-Z]"
            };
        #endregion
        #region 数字或字母
        /// <summary>
        /// 返回匹配正整数或者字母的正则表达式
        /// </summary>
        public static string IntOrLetters { get; } = @"[0-9a-zA-Z]";
        #endregion
        #region 中文汉字
        /// <summary>
        /// 返回用于匹配中文汉字的正则表达式
        /// </summary>
        public static string Chinese { get; } = @"[\u4e00-\u9fa5]";
        #endregion
        #region 匹配非空白
        /// <summary>
        /// 返回用于非空验证的正则表达式
        /// </summary>
        public static string NotNull { get; } = @"\S";
        #endregion
        #region 匹配空白
        /// <summary>
        /// 返回用于匹配空白的正则表达式
        /// </summary>
        public static string Null { get; } = @"\s";
        #endregion
    }
}
