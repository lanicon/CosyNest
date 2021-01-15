using System.Collections.Generic;
using System.IO;

namespace System.NetFrancis.Mail
{
    /// <summary>
    /// 这个类型是<see cref="IMailDraft"/>的实现，
    /// 可以作为一个电子邮件草稿
    /// </summary>
    public sealed record MailDraft : IMailDraft
    {
        #region 关于邮件的信息
        #region 标题
        public string Title { get; init; } = "";
        #endregion
        #region 正文
        public (bool IsText, string Body) Body { get; init; } = (true, "");
        #endregion
        #region 附件
        IEnumerable<IStrongTypeStream> IMail.Attachment
            => Attachment;

        public IList<IStrongTypeStream> Attachment { get; }
        = new List<IStrongTypeStream>();
        #endregion
        #region 收件人
        public IList<string> To { get; }
        = new List<string>();
        #endregion
        #endregion 
        #region 释放所有附件
        public void Dispose()
            => Attachment.DisposableAll();
        #endregion
    }
}
