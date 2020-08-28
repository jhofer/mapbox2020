using Endgame.Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Endgame.Domain
{
    /**
     *  Only for non location passed entites
     * 
     */
    public class CosmosContext: DbContext
    {
        public DbSet<User> Users { get; set; }


        private IConfiguration config;

        public CosmosContext(DbContextOptions<CosmosContext> options, IConfiguration configuration)
        : base(options)
        {
            this.config = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(config["CosmosDBAccountEndpoint"], config["CosmosDBAccountKey"], "endgame");
        }

    }

      
    
}
