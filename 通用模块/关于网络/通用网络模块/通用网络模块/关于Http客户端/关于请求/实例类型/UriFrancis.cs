using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.NetFrancis.Http
{
    /// <summary>
    /// 这个记录封装了Uri的每个部分
    /// </summary>
    public record UriFrancis
    {
        #region 隐式类型转换
        public static implicit operator UriFrancis(string UriComplete)
            => new(UriComplete);
        #endregion
        #region 关于Uri
        #region 获取协议部分
        /// <summary>
        /// 获取Uri的协议部分
        /// </summary>
        public string Agreement
            => UriBase[0..UriBase.IndexOf(":")];
        #endregion
        #region 获取基础URI
        private readonly string UriBaseField;
        /// <summary>
        /// 获取请求的目标的基础Uri部分，
        /// 通过它可以找到服务器主机
        /// </summary>
        public string UriBase
        {
            get => UriBaseField;
            init => UriBaseField = value.TrimEnd('/');
        }
        #endregion
        #region 获取扩展Uri
        private readonly string? UriExtendedField;

        /// <summary>
        /// 获取请求的目标的扩展Uri部分，
        /// 通过它可以找到主机上的资源，
        /// 如果不存在，则返回<see cref="string.Empty"/>
        /// </summary>
        public string UriExtended
        {
            get => UriExtendedField ?? "";
            init => UriExtendedField = value.Trim('/');
        }
        #endregion
        #region 获取Uri参数
        /// <summary>
        /// 获取Uri参数（如果有）
        /// </summary>
        public IEnumerable<(string Name, string Value)> UriParameters { get; init; } = Array.Empty<(string, string)>();
        #endregion
        #region 获取完整Uri
        /// <summary>
        /// 获取完整的绝对Uri
        /// </summary>
        public string UriComplete
        {
            get
            {
                var uri = new StringBuilder(UriBase);
                if (UriExtended is not "")
                    uri.Append("/" + UriExtended);
                if (UriParameters.Any())
                    uri.Append("?" + UriParameters.Join(x => $"{x.Name}={x.Value}", "&"));
                return uri.ToString();
            }
        }
        #endregion
        #endregion 
        #region 重写ToString
        public override string ToString()
            => UriComplete;
        #endregion
        #region 构造函数
#pragma warning disable CS8618
        #region 指定完整绝对Uri
        /// <summary>
        /// 使用指定的完整绝对Uri初始化对象
        /// </summary>
        /// <param name="UriComplete">完整的绝对Uri，可能包含参数</param>
        public UriFrancis(string UriComplete)
        {
            var (_, Uri, Parameters) = ToolNet.ExtractionParameters(UriComplete);
            this.UriBase = Uri;
            UriParameters = Parameters;
        }
        #endregion
        #region 无参数构造函数
        public UriFrancis()
        {

        }
        #endregion
#pragma warning restore
        #endregion
    }
}
