using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Web.Areas.UserRegular.Models
{
    public class UserSensorsViewModel
    {
        public IEnumerable<UserSensorViewModel> userSensorViewModels { get; set; }

    }
}
