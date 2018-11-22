using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SD.Data.Context;
using SD.Data.Models.Identity;
using SD.Services.Data.Exceptions;
using SD.Services.Data.Services.Identity.Contracts;
using SD.Services.Data.Utils;
using X.PagedList;

namespace SD.Services.Data.Services.Identity
{
    public class UserService : IUserService
    {
        private readonly DataContext dataContext;

        public UserService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<IPagedList<ApplicationUser>> FilterUsersAsync(string filter = "", int pageNumber = 1, int pageSize = 10)
        {
            Validator.ValidateNull(filter, "Filter cannot be null!");

            Validator.ValidateMinRange(pageNumber, 1, "Page number cannot be less then 1!");
            Validator.ValidateMinRange(pageSize, 0, "Page size cannot be less then 0!");

            var query = this.dataContext.Users
                .Where(u => u.UserName.Contains(filter) || u.Email.Contains(filter));

            return await query.ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task SaveAvatarImageAsync(Stream stream, string userId)
        {
            Validator.ValidateNull(stream, "Image stream cannot be null!");
            Validator.ValidateNull(userId, "User Id cannot be null!");
            Validator.ValidateGuid(userId, "User id is not in the correct format.Unable to parse to Guid!");

            ApplicationUser user = await this.dataContext.Users.FindAsync(userId);

            if (user == null)
            {
                throw new EntityNotFoundException();
            }

            using (BinaryReader br = new BinaryReader(stream))
            {
                user.AvatarImage = br.ReadBytes((int)stream.Length);
            }

            await this.dataContext.SaveChangesAsync();
        }
    }
}
