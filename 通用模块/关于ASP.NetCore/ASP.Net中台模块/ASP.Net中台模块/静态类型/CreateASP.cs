using System.Collections.Generic;
using System.Net.Http.Headers;
using System.NetFrancis.Http;
using System.Safety.Authentication;

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
            return new UriFrancis() { UriBase = uri };
        }
        #endregion
        #endregion
        #region 有关标头
        #region 根据身份验证信息创建AuthenticationHeaderValue
        #region 辅助成员
        #region 用户名的默认键名
        /// <summary>
        /// 获取用来提取用户名的默认键名
        /// </summary>
        public const string DefaultKeyUserName = "UserName";
        #endregion
        #region 密码的默认键名
        /// <summary>
        /// 获取用来提取密码的默认键名
        /// </summary>
        public const string DefaultKeyPassword = "Password";
        #endregion
        #endregion 
        #region 根据用户名和密码
        /// <summary>
        /// 根据用户名和密码，创建一个<see cref="AuthenticationHeaderValue"/>对象
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Password">密码</param>
        /// <param name="Scheme">身份验证架构</param>
        /// <param name="UserNameKey">用来提取用户名的键名</param>
        /// <param name="PasswordKey">用来提取密码的键名</param>
        /// <returns></returns>
        public static AuthenticationHeaderValue AuthenticationHeader(string UserName, string Password,
            string Scheme = "Cookies", string UserNameKey = DefaultKeyUserName, string PasswordKey = DefaultKeyPassword)
            => new(Scheme, $"{UserNameKey}={UserName};{PasswordKey}={Password}");
        #endregion
        #region 根据UnsafeCredentials
        /// <summary>
        /// 根据用户名和密码，创建一个<see cref="AuthenticationHeaderValue"/>对象
        /// </summary>
        /// <param name="Credentials">用来封装用户名和密码的对象</param>
        /// <param name="Scheme">身份验证架构</param>
        /// <param name="UserNameKey">用来提取用户名的键名</param>
        /// <param name="PasswordKey">用来提取密码的键名</param>
        /// <returns></returns>
        public static AuthenticationHeaderValue AuthenticationHeader(UnsafeCredentials Credentials,
            string Scheme = "Cookies", string UserNameKey = DefaultKeyUserName, string PasswordKey = DefaultKeyPassword)
            => AuthenticationHeader(Credentials.ID, Credentials.Password, Scheme, UserNameKey, PasswordKey);
        #endregion
        #endregion
        #endregion
    }
}
