using System.Linq;
using SD.Data.Models.Identity;
using SD.Web.Models;
using X.PagedList;

namespace SD.Web.Areas.Administration.Models
{
    public class IndexViewModel
    {
        public IndexViewModel(IPagedList<ApplicationUser> users, string searchTerm = "")
        {
            this.Table = new TableViewModel<UserTableViewModel>()
            {
                Items = users.Select(u => new UserTableViewModel(u)),
                Pagination = new PaginationViewModel()
                {
                    PageCount = users.PageCount,
                    PageNumber = users.PageNumber,
                    PageSize = users.PageSize,
                    HasNextPage = users.HasNextPage,
                    HasPreviousPage = users.HasPreviousPage,
                    SearchTerm = searchTerm,
                    AreaRoute = "Admin",
                    ControllerRoute = "User",
                    ActionRoute = "Filter"
                }
            };
        }

        public TableViewModel<UserTableViewModel> Table { get; set; }
    }
}
