using SD.Data.Models.DomainModels;
using SD.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace SD.Web.Areas.Administration.Models.SensorViewModels
{
	public class SensorIndexViewModel
	{
		public SensorIndexViewModel(IPagedList<UserSensor> userSensors, string searchTerm = "")
		{
			this.Table = new TableViewModel<SensorTableViewModel>()
			{
				Items = userSensors.Select(u => new SensorTableViewModel(u)),
				Pagination = new PaginationViewModel()
				{
					PageCount = userSensors.PageCount,
					PageNumber = userSensors.PageNumber,
					PageSize = userSensors.PageSize,
					HasNextPage = userSensors.HasNextPage,
					HasPreviousPage = userSensors.HasPreviousPage,
					SearchTerm = searchTerm,
					AreaRoute = "Administration",
					ControllerRoute = "Sensor",
					ActionRoute = "Filter"
				}
			};
		}

		public TableViewModel<SensorTableViewModel> Table { get; set; }
	}
}
