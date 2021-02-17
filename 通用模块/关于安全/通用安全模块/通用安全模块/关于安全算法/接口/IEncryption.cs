using System.IOFrancis;
using System.IOFrancis.Bit;
using System.Text;

namespace System.SafetyFrancis.Algorithm
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以作为一个加密算法
    /// </summary>
    public interface IEncryption
    {
        #region 执行加密，传入IBitRead
        /// <summary>
        /// 对明文进行加密，并返回读取密文的管道
        /// </summary>
        /// <param name="plaintext">用来读取明文的管道</param>
        /// <returns></returns>
        IBitRead Encryption(IBitRead plaintext);
        #endregion
        #region 执行加密，传入String
        #region 复杂方法
        /// <summary>
        /// 对字符串格式的明文进行加密，
        /// 并以字符串的格式返回密文
        /// </summary>
        /// <param name="plaintext">待加密的明文</param>
        /// <param name="coding">用来将明文字符串转换为字节数组的委托</param>
        /// <param name="decoding">用来将密文字节数组转换为字符串的委托</param>
        /// <returns></returns>
        string Encryption(string plaintext, Func<string, byte[]> coding, Func<byte[], string> decoding)
        {
            using var Plaintext = CreateIO.BitReadEnumerable(coding(plaintext));
            using var ciphertext = Encryption(Plaintext);
            return decoding(ciphertext.ReadAll().Result);
        }
        #endregion
        #region 简单方法
        /// <summary>
        /// 对字符串格式的明文进行加密，
        /// 并以字符串的格式返回密文，
        /// 注意：编码字符串和解码字节数组的方法，
        /// 需要接口的调用者和开发者自行约定好
        /// </summary>
        /// <param name="plaintext">待加密的明文</param>
        /// <returns></returns>
        string Encryption(string plaintext)
           => Encryption(plaintext,
               x => Encoding.Unicode.GetBytes(x),
               x => Convert.ToHexString(x));

        /*说明：
          在默认实现下，将字符串编码为UTF16格式，
          将字节数组解码为十六进制字符串*/
        #endregion
        #endregion
    }
}
