using Endgame.Backend.Domain;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User = Endgame.Backend.Domain.User;

namespace Endgame.Domain
{
    public class DBAccess
    {

        private static readonly FeedOptions DefaultOptions = new FeedOptions { EnableCrossPartitionQuery = true };
        private static readonly Uri usersUri = UriFactory.CreateDocumentCollectionUri("endgame", "users");
        private static readonly Uri buildingsUri = UriFactory.CreateDocumentCollectionUri("endgame", "buildings");


        private static readonly Dictionary<Type, Uri> mapping = new Dictionary<Type, Uri>()
        {
            {typeof(User),usersUri },
            {typeof(Building),buildingsUri }
        };


        private IDocumentClient client;


        public DBAccess(IConfiguration config)
        {
            string endpoint = config["CosmosDBAccountEndpoint"];
            string key = config["CosmosDBAccountKey"];
            var uri = new Uri(endpoint);
            client = new DocumentClient(uri, key);
        }


        public User GetUserByEmail(string email)
        {
            IOrderedQueryable<User> query = UserQuery();
            var user = query.Where(f => f.Email == email).AsEnumerable().FirstOrDefault();
            return user;
        }

        private IOrderedQueryable<User> UserQuery()
        {
            var query = client.CreateDocumentQuery<User>(usersUri, DefaultOptions);
            return query;
        }

        private IOrderedQueryable<Building> BuildingQuery()
        {

            var query = client.CreateDocumentQuery<Building>(buildingsUri, DefaultOptions);
            return query;
        }

        public async Task<T> Create<T>(T model)
        {
            var uri = mapping[typeof(T)];

            var doc = await client.UpsertDocumentAsync(uri, model);
            var savedModel = JsonConvert.DeserializeObject<T>(doc.Resource.ToString());
            return savedModel;
        }


    }
}
