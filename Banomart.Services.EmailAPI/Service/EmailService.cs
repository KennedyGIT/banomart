using Banomart.Services.EmailAPI.Data;
using Banomart.Services.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Banomart.Services.EmailAPI.Service
{
    public class EmailService : IEmailService
    {
        private readonly DbContextOptions<DatabaseContext> dboptions;

        public EmailService(DbContextOptions<DatabaseContext> dboptions)
        {
            this.dboptions = dboptions;
        }

        public async Task EmailCartLog(CartDto cartDto)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br> Cart Email Requested");
            message.AppendLine("<br> Total " + cartDto.CartHeader.CartTotal);
            message.AppendLine("<br>");
            message.AppendLine("<ul>");

            foreach (var item in cartDto.CartDetails) 
            {
                message.AppendLine("<li>");
                message.AppendLine(item.Product.Name + " x " + item.Quantity);
                message.AppendLine("</li>");
            }

            message.AppendLine("</ul>");

            await LogEmail(message.ToString(), cartDto.CartHeader.Email);
        }

        public async Task EmailNewUser(UserDto userDto)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br> New User Created");
            message.AppendLine("<br> User Email " + userDto.Email);
            message.AppendLine("<br>");
            message.AppendLine("<ul>");

            

            await LogEmail(message.ToString(), userDto.Email);
        }

        private async Task<bool> LogEmail(string message, string email) 
        {
            try 
            {
                EmailLog emailLog = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message,
                };

                await using var db = new DatabaseContext(this.dboptions);
                await db.EmailLogs.AddAsync(emailLog);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }
    }
}
