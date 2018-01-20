using MoMoFirebase.DAL;
using MoMoFirebase.Infrastructure;
using MoMoFirebase.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MoMoFirebase.Services
{
    public class FCMService
    {
        private FCMNotificationSender _fcmMessaging;
        private readonly string[] _badTokenRespMessange = { "NotRegistered", "InvalidRegistration", "MismatchSenderId" };

        public FCMService()
        {
            _fcmMessaging = new FCMNotificationSender();
        }

        public async Task AddTokenToDb(string tokenValue)
        {
            using (var _db = new MoMoFirebaseDbContext())
            {
                var existingToken = await _db.FCMTokens.FirstOrDefaultAsync(t => t.FcmTokenValue.Equals(tokenValue));

                if (existingToken == null)
                {
                    FCMToken newData = new FCMToken()
                    {
                        FcmTokenValue = tokenValue,
                        Active = true,
                        CreatedUtc = DateTime.UtcNow,
                        DeviceType = "WebBrowser"
                    };

                    _db.FCMTokens.Add(newData);
                }
                else
                {
                    existingToken.ModifiedUtc = DateTime.UtcNow;
                    existingToken.Active = true;
                }

                await _db.SaveChangesAsync();
            }
        }

        public async Task<SendNotificationResponse> SendNotificationToAllUsersAsync(MessageData messageData)
        {
            SendNotificationResponse response;

            using (var _db = new MoMoFirebaseDbContext())
            {
                var fcmTokens = await _db.FCMTokens.Where(t => t.Active == true)
                    .Select(t => t.FcmTokenValue)
                    .ToListAsync();

                if (fcmTokens != null && fcmTokens.Count > 0)
                {
                    var postData = new PostData()
                    {
                        MessageData = messageData,
                        FcmTokens = fcmTokens
                    };

                    response = await SendNotificationAsync(postData);
                }
                else
                {
                    response = new SendNotificationResponse()
                    {
                        SendNotificationStatus = SendNotificationEnum.MissingToken,
                        StatusMessage = "Don't have active tokens in database!"
                    };
                }

                return response;
            }
        }
        private async Task<SendNotificationResponse> SendNotificationAsync(PostData postData)
        {
            SendNotificationResponse response;

            if (postData.FcmTokens?.Any() == true)
            {
                var sendStatus = await _fcmMessaging.SendNotificationAsync(postData);

                if (sendStatus.IsAllSuccess())
                {
                    response = new SendNotificationResponse
                    {
                        SendNotificationStatus = SendNotificationEnum.Success,
                        StatusMessage = "Notification has been successfully sent!",
                    };
                }
                else if (sendStatus.IsAllFail())
                {
                    response = new SendNotificationResponse
                    {
                        SendNotificationStatus = SendNotificationEnum.NotificationPostFail,
                        StatusMessage = "Notification has not been sent!"
                    };
                }
                else
                {
                    response = new SendNotificationResponse
                    {
                        SendNotificationStatus = SendNotificationEnum.NotAllSuccess,
                        StatusMessage = "Some notification has not been sent!"
                    };
                }

                var tokensForMarkAsInactive = sendStatus.Results
                    .Select((r, index) => new
                    {
                        Result = r,
                        Index = index
                    })
                    .Where(r => _badTokenRespMessange.Contains(r.Result.Error))
                    .Select(resultWithIndex => postData.FcmTokens[resultWithIndex.Index])
                    .ToArray();

                await RemoveRegistrationAsync(tokensForMarkAsInactive);
            }
            else
            {
                response = new SendNotificationResponse()
                {
                    SendNotificationStatus = SendNotificationEnum.MissingToken,
                    StatusMessage = "FCM token is not found!"
                };
            }

            return response;
        }

        private async Task RemoveRegistrationAsync(string[] fcmTokens)
        {
            if (fcmTokens?.Any() == true)
            {
                using (var _db = new MoMoFirebaseDbContext())
                {
                    var fcmTokensDb = await _db.FCMTokens
                        .Where(b => fcmTokens.Contains(b.FcmTokenValue) && b.Active == true)
                        .ToListAsync();

                    if (fcmTokensDb.Count > 0)
                    {
                        foreach (var fcmToken in fcmTokensDb)
                        {
                            fcmToken.Active = false;
                            fcmToken.ModifiedUtc = DateTime.UtcNow;
                        }
                        await _db.SaveChangesAsync();
                    }
                }
            }
        }
    }
}