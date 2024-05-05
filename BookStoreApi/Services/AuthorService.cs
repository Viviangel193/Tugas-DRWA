using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;


namespace BookStoreApi.Services
{
    public class AuthorsService
    {
        private readonly IMongoCollection<Author> _authorsCollection;

        public AuthorsService(
            IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _authorsCollection = mongoDatabase.GetCollection<Author>(
                bookStoreDatabaseSettings.Value.AuthorsCollectionName);
        }

        // Retrieve all authors
        public async Task<List<Author>> GetAllAsync() =>
            await _authorsCollection.Find(_ => true).ToListAsync();

        // Retrieve a single author by ID
        public async Task<Author?> GetAsync(string id)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(id, out objectId))
            {
                // Handle invalid ObjectId format here
                return null;
            }

            return await _authorsCollection.Find(author => author.Id == objectId).FirstOrDefaultAsync();
        }


        // Remove an author
        public async Task RemoveAsync(string id)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(id, out objectId))
            {
                // Handle invalid ObjectId format here
                return;
            }

            await _authorsCollection.DeleteOneAsync(author => author.Id == objectId);
        }


        // Create a new author
        public async Task CreateAsync(Author author) =>
            await _authorsCollection.InsertOneAsync(author);

        // Update an existing author
        public async Task UpdateAsync(string id, Author authorIn)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(id, out objectId))
            {
                // Handle invalid ObjectId format here
                return;
            }

            await _authorsCollection.ReplaceOneAsync(author => author.Id == objectId, authorIn);
        }


    }
}
