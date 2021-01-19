using System;
using System.DataFrancis;
using System.Design.Direct;
using System.Safety;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore
{
    /// <summary>
    /// 有关WebApi的工具类
    /// </summary>
    public static class ToolWebApi
    {
        #region 验证并序列化验证结果
        #region 传入委托
        /// <summary>
        /// 执行身份验证，并忽略其中的业务异常，然后返回验证结果
        /// </summary>
        /// <param name="verify">用于执行身份验证的委托</param>
        /// <returns>一个元组，它的项分别是身份验证的结果，以及一个可用于WebApi的<see cref="IDirect"/>，
        /// 它可以被序列化然后发送到前端，并告知前端是否验证通过，以及验证不通过的原因</returns>
        public static async Task<(ClaimsPrincipal Results, IDirect Serialization)> VerifySerialization(Func<Task<ClaimsPrincipal>> verify)
        {
            var (Exception, Return) = await ToolException.IgnoreBusinessAsync(verify);
            return Return.IsAuthenticated() ?
                (Return, CreateDataObj.Data(("IsAuthenticated", true))) :
                (CreateSafety.PrincipalDefault, CreateDataObj.Data(("IsAuthenticated", false), ("Message", Exception!.Message)));
        }
        #endregion
        #endregion
    }
}
