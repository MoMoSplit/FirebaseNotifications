using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MoMoFirebase.Models.Dto
{
    public class NotificationDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Message { get; set; }

        public string ImageUrl { get; set; }

        public string Link { get; set; }
    }
}