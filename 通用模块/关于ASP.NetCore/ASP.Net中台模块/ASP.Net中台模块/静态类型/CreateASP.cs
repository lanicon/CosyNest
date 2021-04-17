namespace Microsoft.AspNetCore
{
    /// <summary>
    /// 这个静态类可以用来创建通用的ASP.NET对象，
    /// 它们在前端或后端都有用处
    /// </summary>
    public static class CreateASP
    {
        #region 获取提取身份验证信息的键名
        /// <summary>
        /// 获取从Cookies中提取身份验证信息的默认键名
        /// </summary>
        public const string AuthenticationKey = "Authentication";
        #endregion
    }
}
