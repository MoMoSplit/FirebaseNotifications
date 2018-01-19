using MoMoFirebase.DAL;
using MoMoFirebase.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MoMoFirebase.Services
{
    public class FCMService
    {
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
    }
}