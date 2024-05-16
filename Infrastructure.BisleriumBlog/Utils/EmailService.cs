using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.BisleriumBlog.Utils
{
    public class EmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("18limboo@gmail.com", "zjbl wgmn sfko yuoa"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("18limboo@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }

        public string FormatPasswordResetEmail(string resetUrl)
        {
            return $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    border: 1px solid #ccc;
                    border-radius: 5px;
                }}
                .btn {{
                    display: inline-block;
                    padding: 10px 20px;
                    background-color: black;
                    color: white; 
                    text-decoration: none;
                    border-radius: 5px;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <p>Dear User,</p>
                <p>To change your password, please follow the link below:</p>
                <p><a class='btn' href='{resetUrl}'>Reset Password</a></p>
                <p>If you didn't request this change, please ignore this email.</p>
                <p>Best regards,<br/>Your Company Name</p>
            </div>
        </body>
        </html>";
        }
    }
}
