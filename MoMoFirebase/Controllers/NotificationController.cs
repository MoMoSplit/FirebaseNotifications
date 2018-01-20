using MoMoFirebase.Models;
using MoMoFirebase.Models.Dto;
using MoMoFirebase.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MoMoFirebase.Controllers
{
    public class NotificationController : Controller
    {
        private FCMService _fcmService;

        public NotificationController()
        {
            _fcmService = new FCMService();
        }
        // GET: Notification
        public ActionResult Notification()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Notification(NotificationDto data)
        {
            if(ModelState.IsValid)
            {
                MessageData messageData = new MessageData()
                {
                    Title = data.Title,
                    Message = data.Message,
                    Image = data.ImageUrl,
                    Link = data.Link
                };

                var response = await _fcmService.SendNotificationToAllUsersAsync(messageData);
                if (response.SendNotificationStatus == SendNotificationEnum.NotificationPostFail || response.SendNotificationStatus == SendNotificationEnum.MissingToken)
                    ModelState.AddModelError("", response.StatusMessage);

                return View(data);
            }

            return View(data);
        }
    }
}