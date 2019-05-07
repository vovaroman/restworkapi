using System;
using restworkapi.Models.Database;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;

namespace restworkapi.Connectors
{
    public class DatabaseConnector
    {
        private MongoClient Mongo;
        private IMongoDatabase Db;

        public IMongoCollection<Job> jobCollection;
        public IMongoCollection<CV> cvCollection;
        public IMongoCollection<Category> categoryCollection;
        public IMongoCollection<User> userCollection;

        public async Task<List<T>> GetAll<T>(IMongoCollection<T> collection)
        {
            var output = new List<T>();
            try
            {
                using (IAsyncCursor<T> cursor = await collection.FindAsync(FilterDefinition<T>.Empty))
                {
                    while (await cursor.MoveNextAsync())
                    {
                        IEnumerable<T> batch = cursor.Current;
                        foreach (T item in batch)
                        {
                            output.Add(item);
                        }
                    }
                }
            }
            catch { }

            return output;
        }

        public async Task<List<T>> GetItemsByFilter<T>(FilterDefinition<T> filter, IMongoCollection<T> collection)
        {
            var output = new List<T>();
            using (IAsyncCursor<T> cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<T> batch = cursor.Current;

                    foreach (T item in batch)
                    {
                        output.Add(item);
                    }
                }
            }
            return output;
        }

        public async Task<object> GetItemById<T>(int ID, IMongoCollection<T> collection)
        {
            try
            {
                return await collection.Find(x => (x as IDatabaseObject).Id == ID).FirstOrDefaultAsync();
            }
            catch{
                return null;
            }

        }

        public async Task<bool> ModifyValue<T>(T item, IMongoCollection<T> collection)
        {
            try{
                var filter = Builders<T>.Filter.Eq("_id", (item as IDatabaseObject).Id);
                await collection.ReplaceOneAsync(filter, item);
                return true;
            }catch{
                //ignore
            }

            return false;
        }


        public async Task<bool> InsertNewValue<T>(T item, IMongoCollection<T> collection)
        {
            try{

                var count = await collection.CountDocumentsAsync(FilterDefinition<T>.Empty);
                (item as IDatabaseObject).Id = (int)count++;
                await collection.InsertOneAsync(item);
                return true;
            }
            catch{
                return false;
            }
        }

        private DatabaseConnector(){
            string connectionString = "mongodb://localhost:27017";
            Mongo = new MongoClient(connectionString);
            Db = Mongo.GetDatabase("restworkapi");
            jobCollection = Db.GetCollection<Job>("Jobs");
            cvCollection = Db.GetCollection<CV>("CV");
            categoryCollection = Db.GetCollection<Category>("Categories");
            userCollection = Db.GetCollection<User>("Users");
        }

        private static DatabaseConnector databaseConnector;

        private static DatabaseConnector _createInstance()
        {
            databaseConnector = new DatabaseConnector();
            return databaseConnector;
        }

        public static DatabaseConnector GetDatabaseConnector(){
            return databaseConnector is null ? _createInstance() : databaseConnector;
        }


    }
}
