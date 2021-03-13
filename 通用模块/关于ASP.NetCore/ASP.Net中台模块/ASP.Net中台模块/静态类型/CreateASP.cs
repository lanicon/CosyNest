using System.Collections.Generic;
using System.NetFrancis.Http;

using Microsoft.Extensions.Configuration;

namespace Microsoft.AspNetCore
{
    /// <summary>
    /// 这个静态类可以用来创建通用的ASP.NET对象，
    /// 它们在前端或后端都有用处
    /// </summary>
    public static class CreateASP
    {
        #region 有关UriFrancis
        #region 基础Uri为本地主机
        /// <summary>
        /// 返回一个<see cref="UriFrancis"/>，
        /// 它的基础Uri已经填入本地主机的Uri
        /// </summary>
        /// <param name="configuration">用来获取本地主机URI的配置对象</param>
        /// <param name="key">用于从配置中提取本地主机Uri的键名，如果键不存在，会引发异常</param>
        /// <exception cref="KeyNotFoundException">配置中不存在<paramref name="key"/>所指示的键名，无法获取本机URI</exception>
        public static UriFrancis Uri(IConfiguration configuration, string key = "applicationUrl")
        {
            var uri = configuration[key] ?? throw new KeyNotFoundException($"在配置中找不到名为{key}的键，无法获取本机URI");
            return new() { UriBase = uri };
        }
        #endregion
        #endregion
        #region 获取提取身份验证信息的键名
        /// <summary>
        /// 获取从Cookies中提取身份验证信息的默认键名
        /// </summary>
        public const string AuthenticationKey = "Authentication";
        #endregion
    }
}
