using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Safe_Campus.Models;
using SafeCampus.Models;
using System.Security.Claims;

namespace SafeCampus.Services
{
    public class ReportService
    {
        private readonly IMongoCollection<Report> _reportCollection;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportService(IOptions<CampusDatabaseSettings> campusDatabaseSettings, IHttpContextAccessor httpContextAccessor)
        {

            var mongoClient = new MongoClient(campusDatabaseSettings.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(campusDatabaseSettings.Value.DatabaseName);
            _reportCollection = mongoDb.GetCollection<Report>("Report");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Report>> GetAllReports()
        {
            return await _reportCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Report> GetReportById(string id)
        {
            return await _reportCollection.Find(report => report.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateReport(Report report)
        {
            await _reportCollection.InsertOneAsync(report);
        }

        public async Task UpdateReport(string id, Report report)
        {
            await _reportCollection.ReplaceOneAsync(r => r.Id == id, report);
        }

        public async Task DeleteReport(string id)
        {
            await _reportCollection.DeleteOneAsync(report => report.Id == id);
        }

       public string GetMyName()
    {
        var result = string.Empty;
        if (_httpContextAccessor.HttpContext is not null)
        {
            result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

        }

        return result;
  
    }
    }
}