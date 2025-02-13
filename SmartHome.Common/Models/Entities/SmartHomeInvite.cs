using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Common.Models.Entities
{
    public class SmartHomeInvite : Entity
    {
        [Required]
        public Guid DeviceId { get; set; }

        [Required]
        public Guid SmartUserId { get; set; }

        public SmartHome Device { get; set; }
        public Account SmartUser { get; set; }
    }
}
