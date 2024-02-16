using Safe_Campus.Models;
using System.Threading.Tasks;

namespace Safe_Campus.Services
{
    public interface IUserService
    {
        
        Task Create(User user);
        Task<List<User>> GetStudent();
        Task<List<User>> GetGuard();
        bool CheckUser(string userName);
        User  GetByName(string userName);
        Task<User> Get(string Id);
        User GetByToken(string token);
        Task Update(string Id, User user);
        Task UpdateRefreshToken(User user);
        Task RemoveUser(string Id);
        Task UpdateImage(string Id, string imageUrl);



    }
}
