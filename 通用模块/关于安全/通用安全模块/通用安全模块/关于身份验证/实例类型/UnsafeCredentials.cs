using System;

namespace System.SafetyFrancis.Authentication
{
    /// <summary>
    /// 代表一个不安全的凭据，
    /// 它直接封装了用户名和密码
    /// </summary>
    public record UnsafeCredentials
    {
        #region 重要说明
        /*注意：根据net规范，不应该明文存储密码，
          因此本类型的使用应该被严格控制在以下范围：
          1.完全受信任的环境
          2.学习和测试
          3.确实需要明文储存密码的场合，
          但作者仍然认为需要有更好的办法来解决这个问题，
          在找到了新办法以后，请不要在这个场合继续使用本类型*/
        #endregion
        #region 用户名
        /// <summary>
        /// 获取凭据的用户名
        /// </summary>
        public string ID { get; init; }
        #endregion
        #region 密码
        /// <summary>
        /// 获取凭据的密码
        /// </summary>
        public string Password { get; init; }
        #endregion
        #region 解构对象
        /// <summary>
        /// 将对象解构为用户名和密码
        /// </summary>
        /// <param name="ID">凭据的用户名</param>
        /// <param name="Password">凭据的密码</param>
        public void Deconstruct(out string ID, out string Password)
        {
            ID = this.ID;
            Password = this.Password;
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的用户名和密码初始化对象
        /// </summary>
        /// <param name="ID">凭据的用户名</param>
        /// <param name="Password">凭据的密码</param>
        public UnsafeCredentials(string ID, string Password)
        {
            this.ID = ID;
            this.Password = Password;
        }
        #endregion
    }
}
