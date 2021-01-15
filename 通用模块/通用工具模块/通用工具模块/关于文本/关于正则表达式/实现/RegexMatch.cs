using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Text.RegularExpressions
{
    /// <summary>
    /// 这个结构是<see cref="IMatch"/>的实现，
    /// 可以用来表示一个正则表达式匹配结果
    /// </summary>
    class RegexMatch : IMatch
    {
        #region 关于匹配
        #region 匹配到的字符
        public string Match { get; }
        #endregion
        #region 匹配到的组
        public IReadOnlyList<IMatch> Groups { get; }
        #endregion
        #region 获取匹配组的名称
        public string? Name { get; }
        #endregion
        #region 获取命名匹配组
        public IReadOnlyDictionary<string, IMatch> GroupsNamed { get; }
        #endregion
        #endregion
        #region 重写的ToString方法
        public override string ToString()
            => Match;
        #endregion
        #region 构造函数
        #region 指定匹配字符和组
        /// <summary>
        /// 使用指定的匹配字符和组初始化对象
        /// </summary>
        /// <param name="Match">匹配到的字符</param>
        /// <param name="Groups">匹配结果的子结果，
        /// 如果为<see langword="null"/>，则默认为空数组</param>
        /// <param name="GroupsNamed">索引命名匹配组的字典，
        /// 如果为<see langword="null"/>，则默认为空字典</param>
        private RegexMatch(string Match, IReadOnlyList<IMatch>? Groups = null, IReadOnlyDictionary<string, IMatch>? GroupsNamed = null)
        {
            this.Match = Match;
            this.Groups = Groups ?? Array.Empty<IMatch>();
            this.GroupsNamed = GroupsNamed ?? new Dictionary<string, IMatch>();
        }
        #endregion
        #region 指定匹配结果
        /// <summary>
        /// 使用指定的匹配结果初始化对象
        /// </summary>
        /// <param name="Match">指定的匹配结果</param>
        /// <param name="Regular">用来匹配的正则表达式</param>
        public RegexMatch(Capture Match, string Regular)
        {
            this.Match = Match.Value;
            Name = (Match as Group)?.Name;
            Groups = (Match switch
            {
                Match m => m.Groups.OfType<Group>().Skip(1).Where(x => x.Success),
                Group m => (IEnumerable<Capture>)m.Captures,
                _ => Array.Empty<Capture>()
            }).
            Select(x => x == Match ? new RegexMatch(x.Value) : (IMatch)new RegexMatch(x, Regular)).ToArray();
            var names = Regex.Matches(Regular, @"\(\?\<\S+?\>").Select(x => x.Value[3..^1]).ToHashSet();
            GroupsNamed = Groups.Where(x => x.Name != null && names.Contains(x.Name)).ToDictionary(x => (x.Name!, x), false);
        }
        #endregion
        #endregion
    }
}
