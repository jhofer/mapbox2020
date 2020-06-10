using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Text;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace func_endgame_f2_dev
{
    public class DBAccess
    {

        private static readonly FeedOptions DefaultOptions = new FeedOptions { EnableCrossPartitionQuery = true };
        private static readonly Uri usersUri = UriFactory.CreateDocumentCollectionUri("endgame", "users");

        private DocumentClient client;
        public DBAccess(DocumentClient client)
        {
            this.client = client;
        }

        public User GetUserByEmail(string email)
        {
            IOrderedQueryable<User> query = UserQuery();
            var user = query.Where(f => f.email == email).AsEnumerable().FirstOrDefault();
            return user;
        }

        private  IOrderedQueryable<User> UserQuery()
        {
            var query = client.CreateDocumentQuery<User>(usersUri, DefaultOptions);
            return query;
        }

        public async Task<User> Create(User user)
        {
            var doc = await client.UpsertDocumentAsync(usersUri, user);
            var newUser = JsonConvert.DeserializeObject<User>(doc.Resource.ToString());
            return newUser;
        }
    }
}
