using System.Safety.Encryption;

namespace System.Safety.Hash
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以用来计算哈希值
    /// </summary>
    public interface IHash : IEncryption
    {

    }
}
