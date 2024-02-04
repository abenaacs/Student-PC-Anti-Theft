using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Safe_Campus.Models;
using Serilog;
using System.Security.Claims;

namespace Safe_Campus.Services
{
    public class UserService : IUserService
    {
        //Database connectivity
        private readonly IMongoCollection<User> _userCollection;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IOptions<CampusDatabaseSettings> settings, IHttpContextAccessor httpContextAccessor)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _userCollection = database.GetCollection<User>(settings.Value.CollectionsName);
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task Create(User user)
        {
            
           await _userCollection.InsertOneAsync(user);
           
        }
        //Get all user based on their role
        public async Task<List<User>> GetAll(string role)
        {
            
                return await _userCollection.Find(_ => true && _.Role==role).ToListAsync();

        }

        public  bool CheckUser(string userName)
        {
            var existingUser =  _userCollection.Find(i => i.UserName == userName).FirstOrDefault();
             if(existingUser != null) {
                  return true;
            }
             else
            {
                return false;
            }
        }
        public User GetByName(string userName)
        {
            var existingUser = _userCollection.Find(i=>true && i.UserName== userName).FirstOrDefault();
            return existingUser;
        }
        public async Task<User> GetById(string Id)
        {
            return await _userCollection.FindAsync(i =>true && i.Id == Id).Result.FirstOrDefaultAsync();
        }
        public User GetByToken(string token)
        {
            return _userCollection.Find(i=>true && i.RefreshToken.Equals(token)).FirstOrDefault();
        }
        public async void Remove(string Id)
        {
            await _userCollection.DeleteOneAsync(i =>true && i.Id == Id);
        }
        
        public async void Update(string Id, User user)
        {

            await _userCollection.ReplaceOneAsync(i => true && i.Id ==Id, user);
        }

        public async void UpdateRefreshToken(User user)
        {
            var existingUser = _userCollection.Find(i => true && i.UserName == user.UserName).FirstOrDefault();

            existingUser.RefreshToken = user.RefreshToken;
            existingUser.RefreshTokenCreated = user.RefreshTokenCreated;
            existingUser.RefreshTokenExpires = user.RefreshTokenExpires;
            await _userCollection.ReplaceOneAsync(i => true && i.Id == existingUser.Id, existingUser);
        }
        public string GetMyName()
        {
            var result = string.Empty;
            if(_httpContextAccessor.HttpContext is not null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                
            }
            
            return result;
        }
    }   
}
