using System.Collections.Generic;
using System.Design;
using System.Linq;
using System.Safety.Authentication;
using System.Threading;
using System.Threading.Tasks;
using System.Time;

using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;

namespace System.NetFrancis.Mail
{
    /// <summary>
    /// 这个类型是使用MailKit实现的电子邮件客户端
    /// </summary>
    class MailClient : AutoRelease, IMailClient
    {
        #region 辅助成员
        #region 对邮件客户端执行方法，然后释放掉它
        /// <summary>
        /// 对邮件客户端执行委托，返回委托的返回值，然后释放掉它
        /// </summary>
        /// <typeparam name="Server">邮件客户端类型</typeparam>
        /// <typeparam name="Ret">返回值类型</typeparam>
        /// <param name="connection">用于连接邮件客户端的信息</param>
        /// <param name="Del">对邮件客户端执行的委托</param>
        /// <returns></returns>
        private async Task<Ret> Request<Server, Ret>(ConnectionInfo connection, Func<Server, Task<Ret>> Del)
            where Server : IMailService, new()
        {
            using var server = new Server();
            var (host, port, ssl) = connection;
            await server.ConnectAsync(host, port, ssl);
            var (id, pas) = Credentials;
            await server.AuthenticateAsync(id, pas);
            var Return = await Del(server);
            await server.DisconnectAsync(true);
            return Return;
        }
        #endregion
        #region 对IMAP客户端执行方法，然后释放掉它
        /// <summary>
        /// 对IMAP客户端执行委托，返回委托的返回值，然后释放掉它
        /// </summary>
        /// <typeparam name="Ret">返回值类型</typeparam>
        /// <param name="Del">待执行的委托，它的参数分别是IMAP客户端，以及收件箱文件夹</param>
        /// <returns></returns>
        private Task<Ret> Request<Ret>(Func<ImapClient, IMailFolder, Task<Ret>> Del)
            => Request<ImapClient, Ret>(ImapConnection, async server =>
             {
                 var inBox = server.Inbox;
                 await inBox.OpenAsync(FolderAccess.ReadWrite);
                 var Return = await Del(server, inBox);
                 await inBox.CloseAsync();
                 return Return;
             });
        #endregion
        #region 用于登录的凭据
        /// <summary>
        /// 获取用于登录服务器的凭据
        /// </summary>
        private UnsafeCredentials Credentials { get; }
        #endregion
        #region SMTP连接信息
        /// <summary>
        /// 获取用来连接SMTP服务器的信息
        /// </summary>
        private ConnectionInfo SmtpConnection { get; }
        #endregion
        #region IMAP连接信息
        /// <summary>
        /// 获取用来连接IMAP服务器的信息
        /// </summary>
        private ConnectionInfo ImapConnection { get; }
        #endregion
        #endregion
        #region 关于邮件
        #region 关于邮件集合
        #region 返回元素数量
        public Task<int> AsyncCount
            => Request((server, inbox) => Task.FromResult(inbox.Count));
        #endregion
        #region 添加元素
        public Task AsyncAdd(IMailServed item)
            => throw new NotImplementedException("不支持显式向邮箱中添加邮件，请使用发送邮件完成这个功能");
        #endregion
        #region 移除元素
        public Task<bool> AsyncRemove(IMailServed item)
            => Request(async (server, inbox) =>
            {
                if (item is MailServed m && m.Client == this)
                {
                    await inbox.AddFlagsAsync(m.ID, MessageFlags.Deleted, true);
                    await inbox.ExpungeAsync(new[] { m.ID });
                    return true;
                }
                return false;
            });
        #endregion
        #region 移除全部元素
        public Task AsyncClear()
            => Request(async (server, inbox) =>
            {
                foreach (var item in await inbox.FetchAsync(0, -1, MessageSummaryItems.UniqueId))
                {
                    await inbox.AddFlagsAsync(item.UniqueId, MessageFlags.Deleted, true);
                }
                await inbox.ExpungeAsync();
                return 0;
            });
        #endregion
        #region 枚举所有邮件
        public async IAsyncEnumerator<IMailServed> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var mails = Request(async (server, inbox) =>
            {
                var mails = await inbox.FetchAsync(0, -1, MessageSummaryItems.UniqueId);
                return mails.Select(m =>
                {
                    var id = m.UniqueId;
                    var mail = inbox.GetMessage(id);
                    return new MailServed(mail, this, id);
                }).Sort(x => x.Data, false);
            });
            foreach (var item in await mails)
            {
                yield return item;
            }
        }
        #endregion
        #region 检查指定信件是否在邮箱中
        public Task<bool> AsyncContains(IMailServed item)
            => Task.FromResult(item is MailServed m && m.Client == this);
        #endregion
        #endregion
        #region 发送邮件
        public Task MailsSend(params IMailDraft[] draft)
            => Request<SmtpClient, int>(SmtpConnection, async server =>
            {
                await Task.WhenAll(draft.Select
                    (mail => server.SendAsync
                   (mail.ToMail(Credentials.ID))));
                return 0;
            });
        #endregion
        #region 有关新邮件到达时的事件
        #region 用于检查新邮件的计时器
        /// <summary>
        /// 用来检查新邮件的计时器
        /// </summary>
        private ITimer? CheckTimer { get; set; }
        #endregion
        #region 检查新邮件的间隔
        /// <summary>
        /// 获取用来检查新邮件的间隔
        /// </summary>
        private TimeSpan CheckInterval { get; }
        #endregion
        #region 用于缓存旧邮件的集合
        /// <summary>
        /// 用于缓存旧邮件的集合
        /// </summary>
        private IMailServed[]? OldMail { get; set; }
        #endregion
        #region 事件本体
        private Action<IMailClient, IMailServed>? NewMailField;

        public event Action<IMailClient, IMailServed>? NewMail
        {
            add
            {
                NewMailField += value;          //仅当事件被注册后，才会执行检查新邮件的操作
                if (CheckTimer == null && NewMailField != null)
                {
                    #region 用来检查新邮件的本地函数
                    void CheckNewMail()
                    {
                        var NewMails = this.ToArrayAsync().Result();
                        foreach (var item in NewMails.Except(OldMail, FastRealize.EqualityComparer<IMailServed>(x => x.Data)))
                        {
                            NewMailField(this, item);
                        }
                        OldMail = NewMails;
                    }
                    #endregion
                    OldMail = this.ToArrayAsync().Result();
                    CheckTimer = CreateTimer.Timer(CheckInterval, null);
                    CheckTimer.Due += CheckNewMail;
                    CheckTimer.Start();
                }
            }
            remove
            {
                NewMailField -= value;          //如果事件中注册的委托被全部删除，则会停止检查新邮件
                if (NewMailField == null)
                    DisposeRealize();
            }
        }
        #endregion
        #endregion
        #endregion
        #region 释放资源
        protected override void DisposeRealize()
        {
            CheckTimer?.Dispose();
            CheckTimer = null;
            OldMail = null;
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Smtp">用来发送邮件的SMTP服务器连接方式</param>
        /// <param name="Imap">接收邮件的IMAP服务器连接方式</param>
        /// <param name="Credentials">用来登录邮件服务器的凭据</param>
        /// <param name="CheckInterval">指定用来检查新邮件的间隔，
        /// 如果为<see langword="null"/>，默认为1分钟</param>
        public MailClient(ConnectionInfo Smtp, ConnectionInfo Imap, UnsafeCredentials Credentials, TimeSpan? CheckInterval)
        {
            SmtpConnection = Smtp;
            ImapConnection = Imap;
            this.Credentials = Credentials;
            this.CheckInterval = CheckInterval ?? TimeSpan.FromMinutes(1);
        }
        #endregion
    }
}
