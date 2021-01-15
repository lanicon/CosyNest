using System.Text;

using Microsoft.AspNetCore.Http;

namespace System
{
    public static partial class ExtenWebApi
    {
        //这个部分类专门用来声明有关请求和响应的扩展方法

        #region 阅读Http响应
        /// <summary>
        /// 阅读一个Http响应的头和正文，
        /// 并将其封装为元组返回
        /// </summary>
        /// <param name="request">待阅读的Http响应</param>
        /// <param name="encoding">指定Http请求的编码，
        /// 如果为<see langword="null"/>，则默认为UTF8</param>
        /// <returns></returns>
        public static (string Headers, string Body) Read(this HttpRequest request, Encoding? encoding = null)
        {
            using var stream = request.BodyReader.AsStream();
            var headers = request.Headers.Join(x => $"{x.Key}:{x.Value}", Environment.NewLine);
            return (headers, stream.ReadTextAll(encoding));
        }
        #endregion
    }
}
