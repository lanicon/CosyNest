﻿using System.Linq;

namespace System.Text.RegularExpressions
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个正则表达式
    /// </summary>
    public interface IRegex
    {
        #region 用于匹配的正则表达式
        /// <summary>
        /// 获取用于匹配的正则表达式
        /// </summary>
        string RegexText { get; }
        #endregion
        #region 关于匹配结果
        #region 返回是否匹配到了结果
        /// <summary>
        /// 返回在指定的文本中是否匹配到了结果
        /// </summary>
        /// <param name="Text">待匹配的文本</param>
        /// <returns></returns>
        bool IsMatch(string Text)
            => Matches(Text).IsMatch;
        #endregion
        #region 返回匹配结果
        /// <summary>
        /// 返回指定文本的匹配结果
        /// </summary>
        /// <param name="Text">待匹配的文本</param>
        /// <returns>一个元组，它的项分别是是否匹配到了结果，
        /// 以及匹配的结果（如果没有任何匹配，则返回空数组）</returns>
        (bool IsMatch, IMatch[] Matches) Matches(string Text);
        #endregion
        #region 返回第一个匹配到的结果
        /// <summary>
        /// 返回第一个匹配到的结果，
        /// 如果没有任何匹配，返回<see langword="null"/>
        /// </summary>
        /// <param name="Text">待匹配的文本</param>
        /// <returns></returns>
        IMatch? MatcheFirst(string Text)
            => Matches(Text).Matches.FirstOrDefault();
        #endregion
        #endregion
        #region 关于替换与删除
        #region 替换匹配到的字符
        /// <summary>
        /// 替换所有匹配到的字符，
        /// 并返回替换后的文本
        /// </summary>
        /// <param name="Text">待修改的原始文本</param>
        /// <param name="Replace">匹配到的文本会被这个文本所代替</param>
        /// <returns></returns>
        string Replace(string Text, string Replace);
        #endregion
        #region 删除匹配到的字符
        /// <summary>
        /// 删除匹配到的所有字符
        /// </summary>
        /// <param name="Text">待修改的原始文本</param>
        /// <returns></returns>
        string Remove(string Text)
            => Replace(Text, "");
        #endregion
        #endregion 
    }
}
