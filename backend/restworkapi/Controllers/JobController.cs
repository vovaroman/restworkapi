using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using restworkapi.Connectors;
using restworkapi.Models.Database;
using Newtonsoft.Json;
namespace restworkapi.Controllers
{
    [Route("api/[controller]")]
    public class JobController : Controller
    {
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var output = new List<string>();
            var databaseConnector = DatabaseConnector.GetDatabaseConnector();
            (await databaseConnector.GetAll(databaseConnector.jobCollection)).ForEach((obj) => output.Add(JsonConvert.SerializeObject(obj)));
            return output;
        }

        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            var output = string.Empty;
            var databaseConnector = DatabaseConnector.GetDatabaseConnector();
            output = JsonConvert.SerializeObject(
                await databaseConnector.GetItemById(id, databaseConnector.jobCollection)
            );
            return output;
        }


        [HttpPost]
        public async void Post([FromBody]string value)
        {
            try
            {
                IDictionary<string, string> data = JsonConvert.DeserializeObject<IDictionary<string, string>>(value);
                var databaseConnector = DatabaseConnector.GetDatabaseConnector();
                await databaseConnector.InsertNewValue(new Job()
                {
                    Category = data["Category"],
                    Date = data["Date"],
                    Description = data["Description"],
                    Education = data["Education"],
                    Email = data["Email"],
                    Experience = data["Experience"],
                    Location = data["Location"],
                    Phone = data["Phone"],
                    Salary = data["Salary"],
                    Schedule = data["Schedule"],
                    Title = data["Title"],
                    Version = int.Parse(data["Version"])
                }, databaseConnector.jobCollection);
            }
            catch
            {
                //ignore
            }

        }

        [HttpPatch]
        public async void Patch([FromBody]string value)
        {
            try
            {
                IDictionary<string, string> data = JsonConvert.DeserializeObject<IDictionary<string, string>>(value);
                var databaseConnector = DatabaseConnector.GetDatabaseConnector();
                await databaseConnector.ModifyValue(new Job()
                {
                    Id = int.Parse(data["_id"]),
                    Category = data["Category"],
                    Date = data["Date"],
                    Description = data["Description"],
                    Education = data["Education"],
                    Email = data["Email"],
                    Experience = data["Experience"],
                    Location = data["Location"],
                    Phone = data["Phone"],
                    Salary = data["Salary"],
                    Schedule = data["Schedule"],
                    Title = data["Title"],
                    Version = int.Parse(data["Version"])
                }, databaseConnector.jobCollection);
            }
            catch
            {
                //ignore
            }

        }
    }
}

