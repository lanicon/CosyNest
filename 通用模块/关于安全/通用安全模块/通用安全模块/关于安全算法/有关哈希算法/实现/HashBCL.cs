using System.Collections.Generic;
using System.IOFrancis;
using System.IOFrancis.Bit;
using System.Security.Cryptography;

namespace System.SafetyFrancis.Algorithm.Hash
{
    /// <summary>
    /// 这个类型是<see cref="IHash"/>的实现，
    /// 它通过BCL内置的<see cref="HashAlgorithm"/>来计算哈希值
    /// </summary>
    /// <typeparam name="Hash">用来计算哈希值的BCL对象类型</typeparam>
    class HashBCL<Hash> : IHash
        where Hash : HashAlgorithm, new()
    {
        #region 计算哈希值
        public IBitRead Encryption(IBitRead plaintext)
        {
            #region 以异步流返回哈希值的本地函数
            async IAsyncEnumerable<byte[]> Fun()
            {
                using var hash = new Hash();
                using var stream = plaintext.ToStream();
                yield return await hash.ComputeHashAsync(stream);
            }
            #endregion
            return CreateIO.BitReadEnumerable(Fun());
        }
        #endregion 
    }
}
