using System.Linq;
using System.SafetyFrancis.Algorithm.Hash;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace System.SafetyFrancis
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
        #region 创建IHash
        #region 可指定哈希算法
        /// <summary>
        /// 创建一个<see cref="IHash"/>，
        /// 它在内部使用<see cref="HashAlgorithm"/>来计算哈希值
        /// </summary>
        /// <typeparam name="Hash">用于计算哈希值的算法</typeparam>
        /// <returns></returns>
        public static IHash Hash<Hash>()
            where Hash : HashAlgorithm, new()
            => new HashBCL<Hash>();
        #endregion
        #region 使用SHA512Managed
        /// <summary>
        /// 创建一个<see cref="IHash"/>，
        /// 它使用<see cref="SHA512Managed"/>作为算法
        /// </summary>
        /// <returns></returns>
        public static IHash Hash()
            => Hash<SHA512Managed>();
        #endregion
        #endregion
    }
}
