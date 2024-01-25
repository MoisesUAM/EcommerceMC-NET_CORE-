using Microsoft.AspNetCore.Identity.UI.Services;
using ElasticEmail.Api;
using ElasticEmail.Client;
using ElasticEmail.Model;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;


namespace Ecommerce.BLL.Utilities
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration configLocal)
        {
            _config = configLocal;
            //Nombre y clave de Api Key
            var _ApiName = _config.GetValue<string>("ElasticAPI:ElasticAPI_Key");
            var _ApiKey = _config.GetValue<string>("ElasticAPI:Secret_Key");
        }


        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            
            try
            {
                var apiKey = _config.GetValue<string>("ElasticAPI_Key");
                var apiSecret = _config.GetValue<string>("Secret_Key");

                Configuration config = new Configuration();
                config.BasePath = "https://api.elasticemail.com/v4";
                config.AddApiKey("X-ElasticEmail-ApiKey", apiSecret);
                config.ApiKeyPrefix.Add(apiSecret, "Bearer");

                var apiInstance = new EmailsApi(config);

                var from = "moises65907@gmail.com";
                var _body = new BodyPart { 
                    ContentType = BodyContentType.HTML,
                    Content = htmlMessage,
                    Charset = "utf-8"
                };
                List<BodyPart> _bodyParts = [_body];

                var recipients = new List<EmailRecipient> { new EmailRecipient(email) };
                var content = new EmailContent {
                    From = from,
                    Subject = subject,
                    Body = _bodyParts
                };
                var emailMessageData = new EmailMessageData(recipients, content);

               return apiInstance.EmailsPostAsync(emailMessageData);
               
            }
            catch (ApiException ex)
            {
                Debug.Print("Exception when calling EmailsApi.EmailsPost: " + ex.Message);
                Debug.Print("Status Code: " + ex.ErrorCode);
                Debug.Print(ex.StackTrace);
                throw new Exception("Error al enviar el correo electrónico", ex);

            }

            
        }
    }
}
