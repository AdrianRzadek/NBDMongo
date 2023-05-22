using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoNBD.Models;

namespace MongoNBD.Data
{
    public class ComputerContext
    {
        IMongoDatabase database;
        IGridFSBucket gridFS;
        public ComputerContext(IOptions<> mongoDbSet)
 
                    
	        {
	
	            var client = new MongoClient(mongoDbSet.Value.ConnectionString);
	            database = client.GetDatabase(mongoDbSet.Value.DatabaseName);
            gridFS = new GridFSBucket(database);
	        }
	
	        public IMongoCollection<Computers> Computers
	        {
	            get => database.GetCollection<Computers>("Computers");
	        }


         public async Task<IEnumerable<Computers>> GetComputers(int? year, string name)
         {
var builder = new FilterDefinitionBuilder<Computers>();
var filter = builder.Empty;

             if (!string.IsNullOrWhiteSpace(name))
filter = filter & builder.Regex("Name", new BsonRegularExpression(name));

             if (year.HasValue)
filter = filter & builder.Eq("Year", year.Value);

             return await Computers.Find(filter).ToListAsync();
         }

         public async Task<Computers> GetComputer(string id) => await Computers.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();

         public async Task Create(Computers c) => await Computers.InsertOneAsync(c);

         public async Task Update(Computers c) => await Computers.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(c.Id)), c);

         public async Task Remove(string id) => await Computers.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
         public async Task<Byte[]> GetImage(string id) => await gridFS.DownloadAsBytesAsync(new ObjectId(id));
         public async Task StoreImage(string id, Stream imageStream, string imageName)
        {
var computers = await GetComputer(id);
             if (computers.HasImage())
await gridFS.DeleteAsync(new ObjectId(computers.ImageId));
var imageId = await gridFS.UploadFromStreamAsync(imageName, imageStream);
computers.ImageId = imageId.ToString();

var filter = Builders<Computers>.Filter.Eq("_id", new ObjectId(computers.Id));
var update = Builders<Computers>.Update.Set("ImageId", computers.ImageId);

await Computers.UpdateOneAsync(filter, update);
         }
     }
 }
           
            

