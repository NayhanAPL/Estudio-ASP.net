using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace versión_5_asp
{
    internal class ApplicationEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new System.NotImplementedException();
        }
    }
}