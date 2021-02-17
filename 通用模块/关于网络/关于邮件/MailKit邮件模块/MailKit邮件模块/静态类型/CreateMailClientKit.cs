using System.SafetyFrancis.Authentication;

namespace System.NetFrancis.Mail
{
    /// <summary>
    /// 这个静态类可以用来帮助创建邮件客户端
    /// </summary>
    public static class CreateMailClientKit
    {
        #region 创建任意邮件客户端
        #region 需指定完整用户名
        /// <summary>
        /// 创建任意电子邮件客户端
        /// </summary>
        /// <param name="Smtp">指定连接Smtp服务器的方式</param>
        /// <param name="IMAP">指定连接IMAP服务器的方式</param>
        /// <param name="credentials">用来登录邮件服务器的凭据，需要指定完整的用户名，包含@和邮箱域名</param>
        /// <param name="CheckInterval">指定用来检查新邮件的间隔，
        /// 如果为<see langword="null"/>，默认为1分钟</param>
        /// <returns></returns>
        public static IMailClient Client(ConnectionInfo Smtp, ConnectionInfo IMAP, UnsafeCredentials credentials, TimeSpan? CheckInterval = null)
            => new MailClient(Smtp, IMAP, credentials, CheckInterval);
        #endregion
        #region 仅指定短用户名
        /// <summary>
        /// 创建任意电子邮件客户端
        /// </summary>
        /// <param name="Smtp">指定连接Smtp服务器的方式</param>
        /// <param name="IMAP">指定连接IMAP服务器的方式</param>
        /// <param name="credentials">用来登录邮件服务器的凭据，用户名末尾不需要加上@和域名</param>
        /// <param name="DomainName">用户名的域名，不需要加上@符号</param>
        /// <param name="CheckInterval">指定用来检查新邮件的间隔，
        /// 如果为<see langword="null"/>，默认为1分钟</param>
        /// <returns></returns>
        public static IMailClient Client(ConnectionInfo Smtp, ConnectionInfo IMAP, UnsafeCredentials credentials, string DomainName, TimeSpan? CheckInterval = null)
            => Client(Smtp, IMAP, new($"{credentials.ID}@{DomainName}", credentials.Password), CheckInterval);
        #endregion
        #endregion 
        #region 创建QQ邮箱客户端
        /// <summary>
        /// 创建一个QQ邮箱客户端
        /// </summary>
        /// <param name="credentials">用来登录邮件服务器的凭据，用户名不需要加上@qq.com</param>
        /// <param name="CheckInterval">指定用来检查新邮件的间隔，
        /// 如果为<see langword="null"/>，默认为1分钟</param>
        /// <returns></returns>
        public static IMailClient ClientQQ(UnsafeCredentials credentials, TimeSpan? CheckInterval = null)
            => Client
            (new("smtp.qq.com", 465, true),
                new("imap.qq.com", 993, true),
                credentials, "qq.com", CheckInterval);
        #endregion
        #region 创建网易邮箱客户端
        /// <summary>
        /// 创建一个网易邮箱客户端
        /// </summary>
        /// <param name="credentials">用来登录邮件服务器的凭据，用户名不需要加上@163.com</param>
        /// <param name="CheckInterval">指定用来检查新邮件的间隔，
        /// 如果为<see langword="null"/>，默认为1分钟</param>
        /// <returns></returns>
        public static IMailClient Client163(UnsafeCredentials credentials, TimeSpan? CheckInterval = null)
            => Client
            (new("smtp.163.com", 465, true),
                new("imap.163.com", 993, true),
                credentials, "163.com", CheckInterval);
        #endregion
        #region 创建微软邮箱客户端
        /// <summary>
        /// 创建一个微软邮箱客户端
        /// </summary>
        /// <param name="credentials">用来登录邮件服务器的凭据，用户名不需要加上@outlook.com</param>
        /// <param name="CheckInterval">指定用来检查新邮件的间隔，
        /// 如果为<see langword="null"/>，默认为1分钟</param>
        /// <returns></returns>
        public static IMailClient ClientOutlook(UnsafeCredentials credentials, TimeSpan? CheckInterval = null)
            => Client
            (new("smtp.office365.com", 587, true),
                new("outlook.office365.com", 993, true),
                credentials, "outlook.com", CheckInterval);
        #endregion
    }
}
