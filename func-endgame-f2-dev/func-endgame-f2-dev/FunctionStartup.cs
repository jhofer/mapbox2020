
using Endgame.Domain;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

[assembly: FunctionsStartup(typeof(Endgame.Backend.FunctionStartup))]

namespace Endgame.Backend
{
    class FunctionStartup : FunctionsStartup
    {


        public FunctionStartup()
        {

        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var loggerFactory = new LoggerFactory();
            builder.Services.AddSingleton(loggerFactory.CreateLogger("F"));
            builder.Services.AddDbContext<CosmosContext>();
            builder.Services.AddSingleton<DBAccess>();

            /*   using (var scope = builder.Services.BuildServiceProvider().CreateScope())
               {

                   var dbContext = scope.ServiceProvider.GetRequiredService<CosmosContext>();

                   dbContext.Database.EnsureCreated();
               }*/

        }
    }
     
}
