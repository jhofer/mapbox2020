using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Text;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Assets.Game.Domain;
using Endgame.Domain;
using Microsoft.Extensions.Logging;

namespace func_endgame_f2_dev
{
    public class DBAccess
    {

        private static readonly FeedOptions DefaultOptions = new FeedOptions { EnableCrossPartitionQuery = true };
        private static readonly Uri usersUri = UriFactory.CreateDocumentCollectionUri("endgame", "users");
        private static readonly Uri buildingsUri = UriFactory.CreateDocumentCollectionUri("endgame", "buildings");


        private static readonly Dictionary<Type, Uri> mapping = new Dictionary<Type, Uri>()
        {
            {typeof(UserDto),usersUri },
            {typeof(BuildingDto),buildingsUri }
        };


        private DocumentClient client;
 

        public DBAccess(DocumentClient client) 
        {
           
            this.client = client;
        }


        public UserDto GetUserByEmail(string email)
        {
            IOrderedQueryable<UserDto> query = UserQuery();
            var user = query.Where(f => f.email == email).AsEnumerable().FirstOrDefault();
            return user;
        }

        private  IOrderedQueryable<UserDto> UserQuery()
        {
            var query = client.CreateDocumentQuery<UserDto>(usersUri, DefaultOptions);
            return query;
        }

        private IOrderedQueryable<BuildingDto> BuildingQuery()
        {
            var query = client.CreateDocumentQuery<BuildingDto>(buildingsUri, DefaultOptions);
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
