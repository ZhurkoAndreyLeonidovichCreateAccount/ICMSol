using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Security.Authentication;

namespace ICM
{
    public class EmailService
    {
        private readonly ILogger<EmailService> logger;

        public EmailService(ILogger<EmailService> logger)
        {
            this.logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
           
            //формируем сообщение 
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "-----"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = message
            };
            try
            {
                using (var client = new SmtpClient())
                {
                  //Соединяемся с сервером gmail
                  //Если захотите ввести свои данные и проверить то в гугл аккаунт нужно сделать двухэтапную аутентификацию и затем сгеренировать пароль 
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.Auto);
                    await client.AuthenticateAsync("Email", "password");
                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }
            } catch (Exception e)
            {
                logger.LogError(e.GetBaseException().Message);
            }
           
        }



    }
}

