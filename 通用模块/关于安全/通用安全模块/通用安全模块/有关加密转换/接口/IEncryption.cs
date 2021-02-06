using System.IOFrancis;
using System.IOFrancis.Bit;
using System.Text;

namespace System.Safety.Encryption
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以进行加密运算
    /// </summary>
    public interface IEncryption
    {
        #region 通过明文得到密文
        #region 传入IBitRead
        /// <summary>
        /// 将明文转换为密文
        /// </summary>
        /// <param name="Plaintext">待加密的明文</param>
        /// <returns></returns>
        IBitRead Encryption(IBitRead Plaintext);
        #endregion
        #region 传入字符串
        /// <summary>
        /// 将明文转换为密文
        /// </summary>
        /// <param name="Plaintext">待加密的明文</param>
        /// <returns>加密后的字符串，它通过十六进制字符串来进行编码</returns>
        string Encryption(string Plaintext)
        {
            var text = Encoding.Unicode.GetBytes(Plaintext);
            var read = Encryption(CreateIO.BitReadEnumerable(new[] { text })).ReadAll().Result;
            return Convert.ToHexString(read);
        }
        #endregion
        #endregion
    }
}
