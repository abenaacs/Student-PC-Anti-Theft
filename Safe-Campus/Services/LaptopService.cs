using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SafeCampus.Models;
using Safe_Campus.Models;
using System.Security.Claims;

namespace SafeCampus.Services;


public class LaptopService
{

    private readonly IMongoCollection<Laptop> _laptopCollection;

    public LaptopService(IOptions<CampusDatabaseSettings> campusDatabaseSettings)
    {

        var mongoClient = new MongoClient(campusDatabaseSettings.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(campusDatabaseSettings.Value.DatabaseName);
        _laptopCollection = mongoDb.GetCollection<Laptop>("Laptop");

    }


    public async Task<List<Laptop>> GetAsync()
    {
        var laptops = await _laptopCollection.Find(_ => true).ToListAsync();

        // for debugging
        Console.WriteLine($"Number of laptops retrieved: {laptops.Count}");
        return laptops;
    }

    public async Task<Laptop?> GetAsync(string id)
    {
        return await _laptopCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Laptop newLaptop) =>
       await _laptopCollection.InsertOneAsync(newLaptop);

    public async Task UpdateAsync(string id, Laptop updatedLaptop) =>
        await _laptopCollection.ReplaceOneAsync(x => x.Id == id, updatedLaptop);

    public async Task RemoveAsync(string id) =>
        await _laptopCollection.DeleteOneAsync(x => x.Id == id);

    public Laptop GetByOwner(string ownerId)
    {
        return   _laptopCollection.Find(x => x.OwnerId == ownerId).FirstOrDefault();
        
    }
    public Laptop GetBySerial(string serial)
    {
        return _laptopCollection.Find(x => x.SerialNumber == serial).FirstOrDefault();

    }

}