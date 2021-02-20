using System.Design;
using System.IOFrancis.Bit;

namespace System.SafetyFrancis.Algorithm
{
    /// <summary>
    /// 这个类型是<see cref="IEncryption"/>的实现，
    /// 它会先取明文的哈希值，再对哈希值进行加密
    /// </summary>
    class CryptologyHash : AutoRelease, IEncryption
    {
        #region 封装的对象
        #region 用来计算哈希值的对象
        /// <summary>
        /// 获取用来计算哈希值的对象
        /// </summary>
        private IHash PackHash { get; }
        #endregion
        #region 用来加密的对象
        /// <summary>
        /// 获取用来加密哈希值的对象
        /// </summary>
        private IEncryption PackEncryption { get; }
        #endregion
        #endregion
        #region 释放对象
        protected override void DisposeRealize()
        {
            PackHash.Dispose();
            PackEncryption.Dispose();
        }
        #endregion
        #region 执行加密
        public IBitRead Encryption(IBitRead plaintext)
        {
            var hash = PackHash.Encryption(plaintext);
            return PackEncryption.Encryption(hash);
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Hash">用来计算哈希值的对象</param>
        /// <param name="Encryption">用来加密哈希值的对象</param>
        public CryptologyHash(IHash Hash, IEncryption Encryption)
        {
            PackHash = Hash;
            PackEncryption = Encryption;
        }
        #endregion
    }
}
