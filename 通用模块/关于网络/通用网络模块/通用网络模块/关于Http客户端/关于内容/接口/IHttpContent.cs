using System;

namespace System.NetFrancis.Http
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个Http消息的内容
    /// </summary>
    public interface IHttpContent
    {
        #region 说明文档
        /*问：本接口只提供了一个直接读取HttpContent二进制内容的的API，
          这样会不会很不方便？
          答：不会，你可以为本接口编写扩展方法，
          自行将二进制内容解释为其他类型，如文本，Json等等
          
          问：BCL原生的HttpContent采用了多态化的设计思路，
          使用不同的派生类，例如ByteArrayContent，JsonContent来表示不同类型的Http正文，
          请问为什么本接口不采用这种思路？
          答：因为Http正文本身就不是强类型的，它的内容可以被同时解释为多种格式，
          举例说明，JsonContent既是StreamContent，又是StringContent，
          因此在这种情况下，使用"如何解读Http正文"来代替"Http正文是什么类型"是一个更合理的设计*/
        #endregion
        #region 返回内容标头
        /// <summary>
        /// 返回Http内容的标头
        /// </summary>
        IHttpHeaderContent Header { get; }
        #endregion
        #region 返回内容
        /// <summary>
        /// 返回Http消息的内容，
        /// 它以二进制的格式呈现
        /// </summary>
        ReadOnlySpan<byte> Content { get; }

        /*注意：
          本API直接返回消息的内容，
          而不是通过Stream流式读取消息，
          因此本接口不支持传输大文件，
          如需要此功能，请使用以下方法：
          1.分块传输
          2.使用更加专业的FTP*/
        #endregion
    }
}
