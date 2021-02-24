using System.NetFrancis.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Components.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace System
{
    /// <summary>
    /// 有关Razor的扩展方法全部放在这里
    /// </summary>
    public static class ExtenRazor
    {
        #region 有关JS互操作
        #region 调用eval方法
        #region 说明文档
        /*问：JS函数很多，为什么需要为执行eval专门声明一个扩展方法？
          答：因为这个函数赋予了以字符串的方式执行任意JS代码的能力，
          这对于JS互操作来说非常重要且非常灵活，有必要予以特殊待遇*/
        #endregion
        #region 无返回值
        /// <summary>
        /// 通过JS互操作调用eval方法，
        /// 并执行JS代码
        /// </summary>
        /// <param name="js">执行JS代码的运行时</param>
        /// <param name="jsCode">要执行的JS代码</param>
        /// <returns></returns>
        public static ValueTask InvokeCodeVoidAsync(this IJSRuntime js, string jsCode)
            => js.InvokeVoidAsync("eval", $"{jsCode}");
        #endregion
        #region 有返回值
        /// <summary>
        /// 通过JS互操作调用eval方法，
        /// 并执行JS代码，然后返回返回值
        /// </summary>
        /// <typeparam name="Ret">返回值类型</typeparam>
        /// <param name="js">执行JS代码的运行时</param>
        /// <param name="jsCode">要执行的JS代码</param>
        /// <returns></returns>
        public static ValueTask<Ret> InvokeCodeAsync<Ret>(this IJSRuntime js, string jsCode)
            => js.InvokeAsync<Ret>("eval", $"{jsCode}");
        #endregion
        #endregion
        #region 访问JS属性
        #region 读取属性
        /// <summary>
        /// 读取一个JS属性
        /// </summary>
        /// <typeparam name="Property">属性的类型</typeparam>
        /// <param name="js">JS运行时对象</param>
        /// <param name="property">用来访问属性的表达式，它相对于Window对象</param>
        /// <returns></returns>
        public static ValueTask<Property> GetProperty<Property>(this IJSRuntime js, string property)
            => js.InvokeCodeAsync<Property>($"{property}");
        #endregion
        #region 写入属性
        /// <summary>
        /// 写入一个JS属性
        /// </summary>
        /// <param name="js">JS运行时对象</param>
        /// <param name="property">用来访问属性的表达式，它相对于Window对象</param>
        /// <param name="value">待写入的属性的值</param>
        /// <returns></returns>
        public static ValueTask SetProperty(this IJSRuntime js, string property, object value)
            => js.InvokeCodeVoidAsync($"{property}={(value is string ? $"\"{value}\"" : value)}");
        #endregion
        #endregion
        #endregion
        #region 关于依赖注入
        #region 注入ProvideHttpRequest
        #region 根据身份验证信息动态返回HttpRequestRecording
        /// <summary>
        /// 以Scoped模式注入一个<see cref="ProvideHttpRequestAsync"/>，
        /// 它会检查Cookies中的身份验证信息，如果存在，会在HTTP请求中自动加入Authentication标头
        /// </summary>
        /// <param name="services">待注入服务的容器</param>
        /// <param name="provideTemplate">用来提供模板的委托，
        /// 如果为<see langword="null"/>，则默认返回一个调用无参数构造函数的<see cref="HttpRequestRecording"/></param>
        /// <param name="UserNameKey">从Cookies中提取用户名的键名</param>
        /// <param name="PasswordKey">从Cookies中提取密码的键名</param>
        /// <returns></returns>
        public static IServiceCollection AddHttpRequestAuthentication(this IServiceCollection services,
            ProvideHttpRequestAsync? provideTemplate = null,
            string UserNameKey = CreateASP.DefaultKeyUserName,
            string PasswordKey = CreateASP.DefaultKeyPassword)
        {
            provideTemplate ??= () => new HttpRequestRecording().ToTask();
            return services.AddScoped<ProvideHttpRequestAsync>(serviceProvider => async () =>
            {
                var template = await provideTemplate();
                var cookies = serviceProvider.GetRequiredService<IJSWindow>().Document.Cookie;
                var (existUserName, valueUserName) = await cookies.AsyncTryGetValue(UserNameKey);
                var (existPassword, valuePassword) = await cookies.AsyncTryGetValue(PasswordKey);
                return existUserName && existPassword ?
                template with
                {
                    Header = template.Header with
                    {
                        Authentication = CreateASP.AuthenticationHeader(valueUserName!, valuePassword!, UserNameKey: UserNameKey, PasswordKey: PasswordKey)
                    }
                } : template;
            });
        }
        #endregion
        #endregion 
        #region 注入前端对象
        /// <summary>
        /// 向服务容器注入常用前端对象
        /// </summary>
        /// <param name="services">待注入的服务容器</param>
        /// <returns></returns>
        public static IServiceCollection AddFront(this IServiceCollection services)
        {
            services.AddJSWindow();
            return services.AddSingleton<IProvidedDefaultTemplate>(ProvidedTemplate.Only);
        }
        #endregion
        #region 注入IJSWindow
        /// <summary>
        /// 向服务容器注入一个<see cref="IJSWindow"/>
        /// </summary>
        /// <param name="services">待注入的服务容器</param>
        /// <returns></returns>
        public static IServiceCollection AddJSWindow(this IServiceCollection services)
            => services.AddScoped(x => CreateRazor.JSWindow(x.GetRequiredService<IJSRuntime>()));
        #endregion
        #endregion 
    }
}
