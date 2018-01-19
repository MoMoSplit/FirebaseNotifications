using MoMoFirebase.Models;
using MoMoFirebase.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MoMoFirebase.Controllers
{
    public class HomeController : Controller
    {
        private FCMService _fcmService;

        public HomeController()
        {
            _fcmService = new FCMService();
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SaveFCMToken(string tokenValue)
        {  
            if(!string.IsNullOrEmpty(tokenValue))
            {
                await _fcmService.AddTokenToDb(tokenValue);
                return Json("OK");
            }
            else
            {
                return Json("Bad Request");
            }
        }

        //[HttpPost]
        //public ActionResult SendNotification(string data)
        //{

        //}
    }
}
