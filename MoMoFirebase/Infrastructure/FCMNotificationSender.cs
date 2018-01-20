using MoMoFirebase.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MoMoFirebase.Infrastructure
{
    public class FCMNotificationSender
    {
        private const string _fcmEndpoint = "https://fcm.googleapis.com/fcm/send";
        private static readonly HttpClient HttpClient = new HttpClient();
        private string _authorizeKey = "AAAAOaR4GSU:APA91bF9EsqUDnMoNvvxZly6dDFrdJMZMQKEOvIYGrL11FaGTVkUHq8lwYBxgWHEAElbLYc0Xh_R7vADsBJd6JHduM9vFq2bipUbeuEdRWFOtPeIjKTEXEoHZ8frVWkqJ83jjQX47vMl";

        public FCMSendResponse SendNotification(PostData postData)
        {
            throw new NotImplementedException();
        }

        public async Task<FCMSendResponse> SendNotificationAsync(PostData postData)
        {
            FCMSendResponse sendResponse;
            try
            {
                string content = JsonConvert.SerializeObject(new
                {
                    registration_ids = postData.FcmTokens.ToArray(),
                    priority = "high",
                    data = postData.MessageData,
                    notification = new
                    {
                        body = postData.MessageData.Message,
                        title = postData.MessageData.Title,
                        icon = postData.MessageData.Image,
                        click_action = postData.MessageData.Link,
                        sound = "default",
                        content_available = true
                    }
                });
                var body = new StringContent(content, Encoding.UTF8, "application/json");
                sendResponse = await ExecuteFcmPostAsync(body).ConfigureAwait(false);
            }
            catch (Exception)
            {
                sendResponse = new FCMSendResponse()
                {
                    Success = 0
                };
            }

            return sendResponse;
        }

        private async Task<FCMSendResponse> ExecuteFcmPostAsync(StringContent body)
        {
            HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=" + _authorizeKey);
            var response = await HttpClient.PostAsync(_fcmEndpoint, body).ConfigureAwait(false);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var sendResponse = JsonConvert.DeserializeObject<FCMSendResponse>(responseContent);
            return sendResponse;
        }
    }
}