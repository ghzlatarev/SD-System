using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Web.Areas.UserRegular.Models
{
    public class DataSourceViewModel
    {
        public IEnumerable<SensorAPIViewModel> SensorApi { get; set; }
    }
}
