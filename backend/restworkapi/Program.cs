using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading;
using restworkapi.Models.Database;
using restworkapi.WebSpider;

namespace restworkapi
{
    public class Program
    {
        public async static Task<bool> tests(){
            restworkapi.Connectors.DatabaseConnector connector = restworkapi.Connectors.DatabaseConnector.GetDatabaseConnector();
            var job = new Job()
            {
                Title = "test",
                Category = "temp",
                Phone = "temp"
            };
            var cv = new CV()
            {
                Attachment = "test",
                Category = "temp",
                Description = "temp"
            };

            var category = new Category()
            {
                Name = "IT",
                Version = 1
            };
            var category2 = new Category()
            {
                Name = "Not IT",
                Version = 1
            };
            //await connector.InsertNewValue<Job>(job);
            //await connector.InsertNewValue<Job>(job);
            //await connector.InsertNewValue<CV>(cv);
            //List<Job> jobs = await connector.GetAll<Job>();
            //List<CV> cvs = await connector.GetAll<CV>();
            //var tempjob = await connector.GetItemById<Job>(0) as Job;
            //var tempjob1 = await connector.GetItemById<Job>(1) as Job;
            //await connector.InsertNewValue(category, connector.categoryCollection);
            //await connector.InsertNewValue(ategory2, connector.categoryCollection);
            WebSpiderBuilder webSpiderBuilder = new BuildWebSpiderRabotamd();
            webSpiderBuilder.BuildFields();
            webSpiderBuilder.BuildLink();
            var spider = webSpiderBuilder.GetWSpider();
            await spider.Invoke();

            return true;
        }

        public static void Main(string[] args)
        {
            Thread thread = new Thread(async () => await tests());
            thread.Start();

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
