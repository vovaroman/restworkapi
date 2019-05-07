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
    public class CVController : Controller
    {
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var output = new List<string>();
            var databaseConnector = DatabaseConnector.GetDatabaseConnector();
            var cvs = await databaseConnector.GetAll(databaseConnector.cvCollection);
            cvs.ForEach((obj) => output.Add(JsonConvert.SerializeObject(obj)));
            return output;
        }

        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            var output = string.Empty;
            var databaseConnector = DatabaseConnector.GetDatabaseConnector();
            output = JsonConvert.SerializeObject(
                await databaseConnector.GetItemById(id, databaseConnector.cvCollection)
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
                await databaseConnector.InsertNewValue(new CV()
                {
                    Attachment = data["Attachment"],
                    Category = data["Category"],
                    Courses = data["Courses"],
                    Description = data["Description"],
                    Education = data["Education"],
                    Expirience = data["Expirience"],
                    Languages = data["Languages"],
                    Photo = data["Photo"],
                    Skills = data["Skills"],
                    Version = int.Parse(data["Version"]),
                    Email = data["Email"],
                    Phone = data["Phone"],
                    Position = data["Position"],
                    Salary = int.Parse(data["Salary"])
                }, databaseConnector.cvCollection);
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
                await databaseConnector.ModifyValue(new CV()
                {
                    Id = int.Parse(data["_id"]),
                    Attachment = data["Attachment"],
                    Category = data["Category"],
                    Courses = data["Courses"],
                    Description = data["Description"],
                    Education = data["Education"],
                    Expirience = data["Expirience"],
                    Languages = data["Languages"],
                    Photo = data["Photo"],
                    Skills = data["Skills"],
                    Version = int.Parse(data["Version"]),
                    Email = data["Email"],
                    Phone = data["Phone"],
                    Position = data["Position"],
                    Salary = int.Parse(data["Salary"])
                }, databaseConnector.cvCollection);
            }
            catch
            {
                //ignore
            }

        }
    }
}

