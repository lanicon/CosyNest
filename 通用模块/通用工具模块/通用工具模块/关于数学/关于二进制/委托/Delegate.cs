using System.IOFrancis.Bit;
using System.Threading.Tasks;

namespace System.Maths
{
    #region 说明文档
    /*问：BinaryConvert和BinaryVerify是两个非常简单的委托声明，
      它们具有什么作用？
      答：BinaryConvert能够将二进制内容翻译成另一种格式，
      BinaryVerify能够验证二进制内容的完整性，
      它们通常用于安全领域，但是也可以用在其他地方，
      因此作者决定将它们放在通用工具模块
      
      问：如何使用BinaryConvert表示对称加密，非对称加密和哈希函数？
      答：对于对称加密和非对称加密，
      使用两个BinaryConvert分别表示加密和解密转换，密钥则自包含在委托中，
      至于哈希函数，仅使用一个BinaryConvert即可*/
    #endregion
    #region 读取并转换管道
    /// <summary>
    /// 读取并转换一个管道的内容
    /// </summary>
    /// <param name="reader">待读取的管道</param>
    /// <returns>一个新的管道，通过它可以读取转换后的二进制数据</returns>
    public delegate IBitRead BinaryConvert(IBitRead reader);
    #endregion
    #region 读取并验证管道
    /// <summary>
    /// 读取并验证一个管道的内容
    /// </summary>
    /// <param name="reader">待读取的管道</param>
    /// <param name="comparison">用来和管道内容进行对比的数据</param>
    /// <returns>如果验证通过，则为<see langword="true"/>，否则为<see langword="false"/></returns>
    public delegate Task<bool> BinaryVerify(IBitRead reader, byte[] comparison);
    #endregion
}