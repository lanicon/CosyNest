using System.NetFrancis.Http;

using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace System
{
    /// <summary>
    /// 有关ASP.Net前后端通用的扩展方法全部放在这个类型中
    /// </summary>
    public static class ExtenASP
    {
        #region 有关依赖注入
        #region 注入UriFrancis
        #region 通过配置获取URI
        /// <summary>
        /// 以单例模式注入一个<see cref="UriFrancis"/>，
        /// 它通过配置文件获取本地主机的Uri，并将其作为完整Uri的基础部分
        /// </summary>
        /// <param name="services">要注入依赖的容器</param>
        /// <param name="configuration">用来获取Uri的配置对象</param>
        /// <param name="key">指定用于获取Uri的键名</param>
        /// <returns></returns>
        public static IServiceCollection AddLocalUri(this IServiceCollection services, IConfiguration configuration, string key = "applicationUrl")
            => services.AddSingleton(CreateASP.Uri(configuration, key));
        #endregion
        #endregion
        #endregion 
    }
}
