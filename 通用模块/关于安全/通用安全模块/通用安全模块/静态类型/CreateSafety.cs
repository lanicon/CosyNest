using System.Collections.Generic;
using System.IOFrancis;
using System.IOFrancis.Bit;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace System.Safety
{
    /// <summary>
    /// 这个静态类可以用来创建有关安全的对象
    /// </summary>
    public static class CreateSafety
    {
        #region 创建IIdentity
        /// <summary>
        /// 使用指定的验证类型，用户名和声明创建<see cref="IIdentity"/>对象
        /// </summary>
        /// <param name="AuthenticationType">身份验证的类型，如果为<see langword="null"/>，代表未通过验证</param>
        /// <param name="Name">用户的名称，如果为<see langword="null"/>，代表未通过验证</param>
        /// <param name="Claims">枚举该用户所有声明的键和值</param>
        /// <returns></returns>
        public static ClaimsIdentity Identity(string? AuthenticationType, string? Name, params (string Type, string Value)[] Claims)
        {
            var c = Name is null ? Claims : Claims.Append((ClaimsIdentity.DefaultNameClaimType, Name));
            return new ClaimsIdentity(c.Select(x => new Claim(x.Item1, x.Item2)), AuthenticationType);
        }
        #endregion
        #region 创建ClaimsPrincipal 
        #region 返回未通过验证的ClaimsPrincipal 
        /// <summary>
        /// 返回一个未通过验证的<see cref="ClaimsPrincipal"/>
        /// </summary>
        public static ClaimsPrincipal PrincipalDefault { get; } = new(new ClaimsIdentity());
        #endregion
        #region 使用主标识创建ClaimsPrincipal
        /// <summary>
        /// 使用指定的验证类型，用户名和声明创建一个<see cref="ClaimsIdentity"/>，
        /// 然后用它创建一个<see cref="ClaimsPrincipal"/>对象
        /// </summary>
        /// <param name="AuthenticationType">身份验证的类型，如果为<see langword="null"/>，代表未通过验证</param>
        /// <param name="Name">用户的名称，如果为<see langword="null"/>，代表未通过验证</param>
        /// <param name="Claims">枚举该用户所有声明的键和值</param>
        /// <returns></returns>
        public static ClaimsPrincipal Principal(string? AuthenticationType, string? Name, params (string Type, string Value)[] Claims)
            => new(Identity(AuthenticationType, Name, Claims));
        #endregion
        #endregion
        #region 有关哈希值
        #region 有关计算和验证哈希值
        #region 输入且输出IBitRead（使用SHA512Managed）
        /// <summary>
        /// 返回一个元组，它的项分别是用来计算哈希值的委托，
        /// 以及用来验证哈希值的委托，它通过<see cref="SHA512Managed"/>对象来计算哈希值
        /// </summary>
        /// <returns></returns>
        public static (BitTranslation Calculation, BitVerify<byte[]> Verify) Hash()
            => Hash<SHA512Managed>();
        #endregion
        #region 输入且输出IBitRead
        /// <summary>
        /// 返回一个元组，它的项分别是用来计算哈希值的委托，
        /// 以及用来验证哈希值的委托
        /// </summary>
        /// <typeparam name="Hash">用来计算哈希值的类型</typeparam>
        /// <returns></returns>
        public static (BitTranslation Calculation, BitVerify<byte[]> Verify) Hash<Hash>()
            where Hash : HashAlgorithm, new()
        {
            #region 用于计算哈希值的本地函数
            static IBitRead Calculation(IBitRead read)
            {
                #region 以异步流返回哈希值的本地函数
                async IAsyncEnumerable<byte[]> Fun()
                {
                    using var hash = new Hash();
                    using var stream = read.ToStream();
                    yield return await hash.ComputeHashAsync(stream);
                }
                #endregion
                return CreateIO.BitReadEnumerable(Fun());
            }
            #endregion
            #region 用于验证哈希值的本地函数
            static async Task<bool> Verify(IBitRead read, byte[] comparison)
            {
                using var hash = new Hash();
                using var stream = read.ToStream();
                var hashValue = await hash.ComputeHashAsync(stream);
                return comparison.SequenceEqual(hashValue);
            }
            #endregion
            return (Calculation, Verify);
        }
        #endregion
        #endregion
        #region 有关计算哈希值
        #region 输出String
        /// <summary>
        /// 返回一个函数，它接受<typeparamref name="Obj"/>类型的参数，
        /// 并计算它的哈希值，然后以字符串的形式返回
        /// </summary>
        /// <typeparam name="Obj">要计算哈希值的对象的类型</typeparam>
        /// <param name="hash">用来计算哈希值的函数</param>
        /// <param name="toBitRead">将<typeparamref name="Obj"/>转换为二进制管道的函数</param>
        /// <param name="toString">读取计算好的哈希值，并将其编码为字符串的函数</param>
        /// <returns></returns>
        public static Func<Obj, Task<string>> HashCalculationString<Obj>(BitTranslation hash,
            Func<Obj, IBitRead> toBitRead,
            Func<IBitRead, Task<string>> toString)
            => async obj =>
             {
                 using var read = toBitRead(obj);
                 return await toString(hash(read));
             };
        #endregion
        #region 输入且输出String
        /// <summary>
        /// 返回一个函数，它接受字符串参数，并计算它的哈希值，
        /// 然后将哈希值编码为十六进制字符串，并返回
        /// </summary>
        /// <param name="hash">用来计算哈希值的函数</param>
        /// <returns></returns>
        public static Func<string, Task<string>> HashCalculationString(BitTranslation hash)
            => HashCalculationString<string>(hash,
                x =>
                {
                    var bytes = Encoding.Unicode.GetBytes(x);
                    return CreateIO.BitReadMemory(bytes);
                },
               async x =>
                {
                    var bytes = await x.ReadAll();
                    return Convert.ToHexString(bytes);
                });
        #endregion
        #endregion
        #endregion
    }
}
