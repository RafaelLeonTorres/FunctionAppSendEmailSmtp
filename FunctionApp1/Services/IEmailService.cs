using FunctionApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp1.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage emailMessage);
    }
}
