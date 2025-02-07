using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp1.Models
{
    public class EmailMessage
    {
        public List<string> To { get; set; } = new();
        public List<string> Cc { get; set; } = new();
        public List<string> Bcc { get; set; } = new();
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public List<Attachment> Attachments { get; set; } = new();
    }
}
