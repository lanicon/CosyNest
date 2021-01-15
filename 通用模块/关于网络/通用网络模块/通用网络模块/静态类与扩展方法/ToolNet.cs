using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace System.NetFrancis
{
    /// <summary>
    /// 关于网络的工具类
    /// </summary>
    public static class ToolNet
    {
        #region 关于Uri
        #region 提取Uri中的参数
        #region 辅助属性
        /// <summary>
        /// 获取用来匹配Uri参数的正则表达式
        /// </summary>
        private static IRegex MatchParameter { get; }
        = /*language=regex*/@"[\?&](?<name>[^=]+)=(?<value>[^&]+)".ToRegex();
        #endregion
        #region 正式方法
        /// <summary>
        /// 提取一个Uri中的参数（如果有）
        /// </summary>
        /// <param name="Uri">待提取参数的Uri</param>
        /// <returns>一个元组，它的项分别是Uri是否含有参数，
        /// Uri的非参数部分，以及Uri的参数部分，它枚举了参数的名称和值（如果没有参数，则为空数组）</returns>
        public static (bool HasParameters, string Uri, IEnumerable<(string Name, string Value)> Parameters) ExtractionParameters(string Uri)
        {
            var (IsMatch, Matches) = MatchParameter.Matches(Uri);
            if (!IsMatch)
                return (false, Uri, Array.Empty<(string, string)>());
            var par = Matches.Select(x => (x["name"].Match, x["value"].Match)).ToArray();
            return (true, Uri.Split("?")[0], par);
        }
        #endregion 
        #endregion
        #endregion
    }
}
