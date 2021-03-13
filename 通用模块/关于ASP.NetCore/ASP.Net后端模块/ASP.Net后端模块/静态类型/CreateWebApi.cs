using System;
using System.Linq;
using System.SafetyFrancis.Authentication;
using System.Security.Principal;
using System.TreeObject;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Json;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Microsoft.AspNetCore
{
    /// <summary>
    /// 这个类型可以用来创建有关WebApi的对象
    /// </summary>
    public static class CreateWebApi
    {
        #region 有关Json
        #region 获取序列化IIdentity的对象
        /// <summary>
        /// 获取一个可以序列化和反序列化<see cref="IIdentity"/>的对象
        /// </summary>
        public static ISerialization<IIdentity> SerializationIIdentity { get; }
        = new SerializationIdentity();
        #endregion
        #region 创建Json格式化器
        #region 输出Json
        /// <summary>
        /// 创建一个<see cref="TextOutputFormatter"/>，
        /// 它可以将受支持的类型序列化为Json并在WebApi中返回
        /// </summary>
        /// <param name="serialization">>指定的序列化器，
        /// 它提供了将对象序列化为Json的功能</param>
        /// <returns></returns>
        public static TextOutputFormatter OutputFormatterJson(ISerialization<object> serialization)
            => new JsonOutputFormatterGeneral(serialization);
        #endregion
        #region 输入Json
        /// <summary>
        /// 创建一个<see cref="TextInputFormatter"/>，
        /// 它可以在WeiApi中接受Json，并将其反序列化为受支持的类型
        /// </summary>
        /// <param name="serialization">指定的反序列化器，
        /// 它提供了将Json反序列化为对象的功能</param>
        /// <returns></returns>
        public static TextInputFormatter InputFormatterJson(ISerialization<object> serialization)
            => new JsonInputFormatter(serialization);
        #endregion
        #endregion
        #endregion
        #region 创建IHttpAuthentication
        #region 通过Cookies和Authentication标头进行验证
        #region 直接创建
        /// <summary>
        /// 创建一个<see cref="IHttpAuthentication"/>，
        /// 它从Cookies和Authentication标头中提取信息，并验证身份
        /// </summary>
        /// <param name="Extraction"> 这个委托被用于从<see cref="HttpContext"/>中提取身份验证信息，
        /// 如果不存在身份验证信息，则返回<see langword="null"/></param>
        /// <param name="Authentication">这个委托可以通过验证信息来获取身份验证结果</param>
        /// <returns></returns>
        public static IHttpAuthentication HttpAuthentication
            (Func<HttpContext, string?> Extraction,
            AuthenticationFunction<string> Authentication)
            => new HttpAuthentication(Extraction, Authentication);
        #endregion
        #region 简单版本
        /// <summary>
        /// 创建一个<see cref="IHttpAuthentication"/>，
        /// 它从Cookies和Authentication标头中提取信息，并验证身份
        /// </summary>
        /// <param name="Extraction"> 这个委托被用于从<see cref="HttpContext"/>中提取身份验证信息，
        /// 如果不存在身份验证信息，则返回<see langword="null"/></param>
        /// <param name="AuthenticationKey">用来从Cookies中提取身份验证信息的键名</param>
        /// <returns></returns>
        public static IHttpAuthentication HttpAuthentication
            (AuthenticationFunction<string> Authentication, string AuthenticationKey = CreateASP.AuthenticationKey)
            => HttpAuthentication(http =>
            http.Request.Headers.TryGetValue("Authentication", out var headers) ? headers.First().Split(" ")[1] :
            http.Request.Cookies[AuthenticationKey], Authentication);
        #endregion
        #endregion
        #endregion
    }
}
