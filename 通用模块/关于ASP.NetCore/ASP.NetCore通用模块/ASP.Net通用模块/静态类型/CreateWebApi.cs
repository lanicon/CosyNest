using System.Collections.Generic;
using System.NetFrancis.Http;
using System.Security.Principal;
using System.TreeObject;

using Microsoft.AspNetCore.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;

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
        #region 有关ProvideHttpRequest
        #region 基础Uri为本地主机
        /// <summary>
        /// 返回一个<see cref="ProvideHttpRequest"/>，
        /// 它可以用来创建一个<see cref="HttpRequestRecording"/>，且它的基础Uri已经填入本地主机的Uri
        /// </summary>
        /// <param name="configuration">用来获取本地主机URI的配置对象</param>
        /// <param name="key">用于从配置中提取本地主机Uri的键名，如果键不存在，会引发异常</param>
        /// <exception cref="KeyNotFoundException">配置中不存在<paramref name="key"/>所指示的键名，无法获取本机URI</exception>
        public static ProvideHttpRequest ProvideHttpRequestLocal(IConfiguration configuration, string key = "applicationUrl")
        {
            var uri = configuration[key] ?? throw new KeyNotFoundException($"在配置中找不到名为{key}的键，无法获取本机URI");
            var request = new HttpRequestRecording(new() { UriBase = uri });
            return () => request;
        }
        #endregion
        #endregion
    }
}
