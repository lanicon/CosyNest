using System.Linq;
using System.Threading.Tasks;

using MimeKit;
using MimeKit.Text;

namespace System.NetFrancis.Mail
{
    /// <summary>
    /// 这个静态类是帮助实现电子邮件的辅助类型
    /// </summary>
    static class MailRealize
    {
        #region 将电子邮件草稿转换为MimeMessage
        /// <summary>
        /// 将电子邮件草稿转换为<see cref="MimeMessage"/>
        /// </summary>
        /// <param name="mail">待转换的电子邮件草稿</param>
        /// <param name="From">邮件的发件人</param>
        /// <returns></returns>
        public static MimeMessage ToMail(this IMailDraft mail, string From)
        {
            var (IsText, Body) = mail.Body;
            var body = new Multipart()
            {
                new TextPart(IsText ? TextFormat.Text : TextFormat.Html)
                {
                    Text=Body
                }
            };
            body.Add(mail.Attachment.Select(x =>
            new MimePart()
            {
                FileName = x.Describe ?? "附件" + (x.Format.IsVoid() ? null : $".{x.Format}"),
                Content = new MimeContent(x.ToStream())
            }));
            var m = new MimeMessage()
            {
                Subject = mail.Title,
                Body = body
            };
            m.To.Add(mail.To.Select(x => MailboxAddress.Parse(x)));
            m.From.Add(MailboxAddress.Parse(From));
            return m;
        }
        #endregion
    }
}
