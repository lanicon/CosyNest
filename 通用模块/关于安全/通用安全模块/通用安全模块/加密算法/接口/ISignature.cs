using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Safety.Algorithm
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以签发和验证数字签名
    /// </summary>
    public interface ISignature
    {
        #region 是否可签发签名
        /// <summary>
        /// 如果这个值为<see langword="true"/>，
        /// 代表本对象持有私钥，可以签发签名
        /// </summary>
        bool CanSignature { get; }
        #endregion
    }
}
