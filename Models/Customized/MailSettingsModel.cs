using MimeKit;
using System.Collections.Generic;

namespace OnlineManager.Models.Customized
{
    public class MailSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class MailData
    {
        public List<MailboxAddress> To { get; set; } = new List<MailboxAddress>();
        public List<MailboxAddress> ToCC { get; set; } = new List<MailboxAddress>();
        public List<MailboxAddress> ToBCC { get; set; } = new List<MailboxAddress>();
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> Attachment { get; set; } = new List<string>();
        public List<(string,byte[])> Attachment_byte { get; set; } = null;
    }
}
