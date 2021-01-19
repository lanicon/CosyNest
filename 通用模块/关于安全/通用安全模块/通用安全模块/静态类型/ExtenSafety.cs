using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;

namespace System
{
    /// <summary>
    /// 有关安全的扩展方法全部放在这里
    /// </summary>
    public static class ExtenSafety
    {
        #region 关于IPrincipal
        #region 返回主体是否被验证
        /// <summary>
        /// 返回主体是否被验证
        /// </summary>
        /// <param name="principal">待检查验证状态的主体，
        /// 如果为<see langword="null"/>，则直接返回<see langword="false"/></param>
        /// <returns></returns>
        public static bool IsAuthenticated([NotNullWhen(true)] this IPrincipal? principal)
            => principal is { Identity: { IsAuthenticated: true } };
        #endregion
        #endregion
    }
}
