using System;
using System.SafetyFrancis;
using System.SafetyFrancis.Algorithm;
using System.SafetyFrancis.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.TreeObject;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Primitives;

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
        #region 通过Authentication标头以及Cookie验证身份
        #region 辅助方法
        /// <summary>
        /// 辅助方法，它将原始的提取凭据的委托，
        /// 转换为支持加密的版本
        /// </summary>
        /// <typeparam name="Obj">委托的输入类型</typeparam>
        /// <param name="fun">原始的提取凭据的委托</param>
        /// <param name="cryptology">指定用于解密的对象，
        /// 如果为<see langword="null"/>，代表不需要解密，无需执行任何操作</param>
        /// <returns></returns>
        private static Func<Obj, Task<UnsafeCredentials?>> Convert<Obj>(Func<Obj, Task<UnsafeCredentials?>> fun, ICryptology? cryptology)
            => cryptology is null ? fun : async x =>
            {
                var redentials = await fun(x);
                if (redentials is null)
                    return redentials;
                var plaintext = cryptology.Decrypt(Encoding.Unicode.GetBytes(redentials.Password));
                return redentials with { Password = plaintext };
            };

        #endregion
        #region 直接创建
        /// <summary>
        /// 返回一个<see cref="IHttpAuthentication"/>，
        /// 它通过Http请求的Authentication标头以及Cookie来确认身份，
        /// 但不会将验证结果记住，这个操作由前端负责
        /// </summary>
        /// <param name="VerifyUser">这个委托的参数是待验证的用户名和密码，返回值是验证结果</param>
        /// <param name="ExtractionHeader">这个委托的参数是HTTP请求的Authentication标头，返回值是提取到的用户名和密码，如果提取失败，则返回<see langword="null"/></param>
        /// <param name="ExtractionCookie">这个委托的参数是附加在请求中的Cookie，返回值是提取到的用户名和密码，如果提取失败，则返回<see langword="null"/></param>
        /// <param name="Cryptology">如果这个参数不为<see langword="null"/>，则代表密码部分是加密的，
        /// 需要通过这个参数解密还原原始密码</param>
        public static IHttpAuthentication HttpAuthentication(Func<UnsafeCredentials, Task<ClaimsPrincipal>> VerifyUser,
            Func<StringValues, Task<UnsafeCredentials?>> ExtractionHeader,
            Func<IRequestCookieCollection, Task<UnsafeCredentials?>> ExtractionCookie,
            ICryptology? Cryptology = null)
            => new HttpAuthentication(VerifyUser,
                Convert(ExtractionHeader, Cryptology),
                Convert(ExtractionCookie, Cryptology));
        #endregion
        #region 指定用户名和密码的键值对
        /// <summary>
        /// 返回一个<see cref="IHttpAuthentication"/>，
        /// 它通过指定的键名在Http请求的Authentication标头和Cookie中提取用户名和密码，
        /// 并执行身份验证
        /// </summary>
        /// <param name="VerifyUser">这个委托的参数是待验证的用户名和密码，返回值是验证结果</param>
        /// <param name="Cryptology">如果这个参数不为<see langword="null"/>，则代表密码部分是加密的，
        /// 需要通过这个参数解密还原原始密码</param>
        /// <param name="UserNameKey">用于提取用户名的键</param>
        /// <param name="PasswordKey">用于提取密码的键</param>
        /// <returns></returns>
        public static IHttpAuthentication HttpAuthentication(Func<UnsafeCredentials, Task<ClaimsPrincipal>> VerifyUser,
            ICryptology? Cryptology = null, string UserNameKey = CreateASP.DefaultKeyUserName, string PasswordKey = CreateASP.DefaultKeyPassword)
            => HttpAuthentication(VerifyUser, x =>
            {
                var d = ToolRegex.KeyValuePairExtraction(x.ToString(), "; ,");
                return (d.TryGetValue(UserNameKey, out var UserName) && d.TryGetValue(PasswordKey, out var Password) ?
                 new UnsafeCredentials(UserName, Password) : null).ToTask();
            },
            x => (x.TryGetValue(UserNameKey, out var UserName) && x.TryGetValue(PasswordKey, out var Password) ?
                 new UnsafeCredentials(UserName!, Password!) : null).ToTask(), Cryptology);
        #endregion
        #endregion
        #endregion
    }
}
