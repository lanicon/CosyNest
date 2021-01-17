using System.Collections.Generic;
using System.Linq;

namespace System.Text.RegularExpressions
{
    /// <summary>
    /// 这个静态类是有关正则表达式的工具类
    /// </summary>
    public static class ToolRegex
    {
        #region 匹配十六进制数
        /// <summary>
        /// 返回用于匹配16进制数的正则表达式，
        /// 注意：按照规范，A-F须为大写
        /// </summary>
        public static string Sys16 { get; } = @"[0-9A-Fa-f]";
        #endregion
        #region 匹配数字或字母
        /// <summary>
        /// 返回匹配正整数或者字母的正则表达式
        /// </summary>
        public static string IntOrLetters { get; } = @"[0-9a-zA-Z]";
        #endregion
        #region 匹配中文汉字
        /// <summary>
        /// 返回用于匹配中文汉字的正则表达式
        /// </summary>
        public static string Chinese { get; } = @"[\u4e00-\u9fa5]";
        #endregion
        #region 匹配键值对
        #region 返回表达式
        /// <summary>
        /// 返回用于匹配键值对的正则表达式
        /// </summary>
        /// <param name="Separator">键值对之间的分隔符，
        /// 如果存在多个分隔符，则将它们全部塞在这个字符串中，中间不要有空格</param>
        /// <returns></returns>
        public static string KeyValuePair(string Separator)
            => @$"[^{Separator}](?<key>\S+?)=(?<value>\S+?)[^{Separator}]";
        #endregion
        #region 提取键值对
        /// <summary>
        /// 通过正则表达式，从字符串中提取键值对
        /// </summary>
        /// <param name="Text">要提取键值对的字符串</param>
        /// <param name="Separator">键值对之间的分隔符，
        /// 如果存在多个分隔符，则将它们全部塞在这个字符串中，中间不要有空格</param>
        /// <returns></returns>
        public static Dictionary<string, string> KeyValuePairExtraction(string Text, string Separator)
            => KeyValuePair(Separator).ToRegex().Matches(Text).Matches.ToDictionary(x => (x["key"].Match, x["value"].Match), true);
        #endregion
        #endregion
    }
}
