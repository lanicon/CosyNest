using System.Linq;
using System.NetFrancis.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.TreeObject;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

using static Microsoft.AspNetCore.CreateWebApi;

namespace System
{
    public static partial class ExtenWebApi
    {
        //这个部分类专门用来声明有关依赖注入，中间件的扩展方法

        #region 添加中间件
        #region 添加审阅中间件
        /// <summary>
        /// 添加一个审阅中间件，
        /// 它可以用来查看<see cref="HttpContext"/>对象，
        /// 没有其他的功能
        /// </summary>
        /// <param name="app">待添加中间件的<see cref="IApplicationBuilder"/>对象</param>
        /// <param name="review">用来查看<see cref="HttpContext"/>的委托</param>
        public static void UseReview(this IApplicationBuilder app, Action<HttpContext> review)
            => app.Use(async (context, next) =>
           {
               review(context);
               await next();
           });
        #endregion
        #region 添加验证中间件
        /// <summary>
        /// 添加一个身份验证中间件，
        /// 它依赖于服务<see cref="IHttpAuthentication"/>
        /// </summary>
        /// <param name="application">待添加中间件的<see cref="IApplicationBuilder"/></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAuthenticationFrancis(this IApplicationBuilder application)
        {
            application.ApplicationServices.CheckService<IHttpAuthentication>();
            application.Use(static async (context, next) =>
             {
                 var auth = context.RequestServices.GetRequiredService<IHttpAuthentication>();
                 await ToolException.IgnoreBusinessAsync(() => auth.Verify(context));
                 context.Response.OnStarting(() => auth.SetVerify(context.User, context));      //在后面的中间件全部执行完毕后，将验证结果写回响应中
                 await next();
             });
            return application;
        }
        #endregion
        #endregion
        #region 依赖注入服务
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
            provideTemplate ??= () => Task.FromResult(new HttpRequestRecording());
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
        #endregion
        #region 配置MvcOptions
        #region 添加常用类型Json的支持
        /// <summary>
        /// 为<see cref="MvcOptions"/>添加常用类型Json的支持
        /// </summary>
        /// <param name="options">待添加支持的Mvc配置</param>
        public static void AddFormatterJson(this MvcOptions options)
        {
            #region 添加输入格式化器的本地函数
            void AddInput(params ISerialization<object>[] items)
                => items.ForEach(x => options.InputFormatters.Insert(0, InputFormatterJson(x)));
            #endregion
            AddInput(new JsonConverterIDirect().FitSerialization(), SerializationIIdentity);
        }
        #endregion
        #endregion
    }
}
