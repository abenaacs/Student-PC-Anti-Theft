using Safe_Campus.Models;
using System.Threading.Tasks;

namespace Safe_Campus.Services
{
    public interface IUserService
    {
        
        Task Create(User user);
        Task<List<User>> GetAll(string role);
        bool CheckUser(string userName);
        User GetByName(string userName);
        Task<User> GetById(string Id);
        User GetByToken(string token);
        void Update(string Id, User user);
        void UpdateRefreshToken(User user);
        void Remove(string Id);
        string GetMyName();



    }
}
