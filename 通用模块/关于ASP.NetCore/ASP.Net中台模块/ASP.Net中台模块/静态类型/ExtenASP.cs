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
        #region 注入NamingService
        /// <summary>
        /// 注入一个<see cref="NamingService{Service}"/>，
        /// 它是获取其他服务的跳板，能够进一步根据名称获取服务
        /// </summary>
        /// <typeparam name="Service">服务的类型</typeparam>
        /// <param name="serviceCollection">用来获取服务的容器</param>
        /// <param name="serviceLifetime">服务的生存期</param>
        /// <param name="services">这个元组指定服务的名称，以及用来获取服务的委托</param>
        /// <returns></returns>
        public static IServiceCollection AddNamingService<Service>(this IServiceCollection serviceCollection,
            ServiceLifetime serviceLifetime,
            params (string Name, Func<IServiceProvider, Service> Service)[] services)
        {
            serviceCollection.Add(new(typeof(NamingService<Service>),
                x => (NamingService<Service>)new NamingServiceCollection<Service>(x, services).NamingService, serviceLifetime));
            return serviceCollection;
        }
        #endregion
        #endregion
    }
}
