using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MoMoFirebase.Models
{
    public class FCMToken
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string DeviceType { get; set; }

        [StringLength(255)]
        public string FcmTokenValue { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedUtc { get; set; }

        public DateTime? ModifiedUtc { get; set; }
    }
}