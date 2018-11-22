using System.IO;
using System.Threading.Tasks;
using SD.Data.Models.Identity;
using X.PagedList;

namespace SD.Services.Data.Services.Identity.Contracts
{
    public interface IUserService
    {
        Task SaveAvatarImageAsync(Stream stream, string userId);

        Task<IPagedList<ApplicationUser>> FilterUsersAsync(string filter = "", int pageNumber = 1, int pageSize = 10);
    }
}

