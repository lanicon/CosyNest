using System.Linq;
using System.NetFrancis.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.TreeObject;

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
                 var (_, Return) = await ToolException.IgnoreBusinessAsync(() => auth.Verify(context));
                 if (Return is { Identity: { IsAuthenticated: true } })
                     context.User = Return;
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
        public static IServiceCollection AddHttpRequestAuthentication(this IServiceCollection services,
            ProvideHttpRequestAsync? provideTemplate = null,
            string UserNameKey = DefaultKeyUserName,
            string PasswordKey = DefaultKeyPassword)
            => AddHttpRequest(services, provideTemplate ??= () => Task.FromResult(new HttpRequestRecording()),
               async (serviceProvider, provideHttpRequest) =>
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
                            Authentication = new("Cookies", $"{UserNameKey}={valueUserName};{PasswordKey}={valuePassword}")
                        }
                    } : template;
                });
        #endregion
        #region 使用模板来获取
        /// <summary>
        /// 注入一个<see cref="ProvideHttpRequestAsync"/>，
        /// 它先通过一个<see cref="ProvideHttpRequestAsync"/>来获取<see cref="HttpRequestRecording"/>的模板，
        /// 然后再通过一个委托对它进行改动，并返回最终的<see cref="HttpRequestRecording"/>
        /// </summary>
        /// <param name="services">待注入服务的容器</param>
        /// <param name="provideTemplate">这个委托用来获取<see cref="HttpRequestRecording"/>作为模板</param>
        /// <param name="provide">这个委托的第一个参数是用来请求服务的对象，
        /// 第二个参数是用来获取模板的委托，返回值是最终获取的<see cref="HttpRequestRecording"/></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpRequest(this IServiceCollection services,
            ProvideHttpRequestAsync provideTemplate,
            Func<IServiceProvider, ProvideHttpRequestAsync, Task<HttpRequestRecording>> provide)
            => services.AddSingleton(x =>
            {
                #region 本地函数
                Task<HttpRequestRecording> Fun()
                    => provide(x, provideTemplate);
                #endregion
                return (ProvideHttpRequestAsync)Fun;
            });

        /*问：这个服务的逻辑似乎有点复杂，请问它的用途是什么？
          答：举例说明，假设有以下需求：
          通过读取Cookies来改变Http请求头中的Authentication标头，
          如果Cookies中存储了身份信息，则在Http请求中带上身份，
          另外，无论在何种情况下，请求的基础Uri都是本机地址，
          这种需求可以通过本API按照以下方法实现：
          1.provideTemplate参数传入一个委托，调用它可以获得一个HttpRequestRecording，
          它的基础Uri已经填好，该UriFrancis对象可通过CreateWebApi.Uri方法获取
          2.provide参数传入一个委托，它通过IServiceProvider参数来请求一个可以读取Cookies的对象，
          然后调用ProvideHttpRequest参数来获取模板，并通过with表达式来更改模板的Authentication标头，至此功能实现完成*/
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
