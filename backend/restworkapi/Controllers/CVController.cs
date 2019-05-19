using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using restworkapi.Connectors;
using restworkapi.Models.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace restworkapi.Controllers
{
    [Route("api/[controller]")]
    public class CVController : Controller
    {
        [HttpGet]
        public async Task<string> Get()
        {
            var output = new List<CV>();
            var databaseConnector = DatabaseConnector.GetDatabaseConnector();
            output = await databaseConnector.GetAll(databaseConnector.resumeCollection);
            //output.ForEach(x => x.Photo = "empty");
            var jsonOutput = JsonConvert.SerializeObject(output);
            return jsonOutput;
        }

        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            //var output = string.Empty;
            var databaseConnector = DatabaseConnector.GetDatabaseConnector();
            //output = JsonConvert.SerializeObject(
            //    await databaseConnector.GetItemById(id, databaseConnector.resumeCollection)
            //);
            var output = await databaseConnector.GetItemById(id, databaseConnector.resumeCollection) as CV;
            var jsonOutput = JsonConvert.SerializeObject(output);

            return jsonOutput;
        }
        

        [HttpPost]
        public async void Post([FromBody] JObject value)
        {
            try
            {
                IDictionary<string, string> data = JsonConvert.DeserializeObject<IDictionary<string, string>>(value.ToString());
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
                    Salary = data["Salary"],
                    UserId = int.Parse(data["UserId"])
                }, databaseConnector.resumeCollection);
            }
            catch
            {
                //ignore
            }

        }

        [HttpPatch]
        public async void Patch([FromBody] JObject value)
        {
            try
            {
                IDictionary<string, string> data = JsonConvert.DeserializeObject<IDictionary<string, string>>(value.ToString());
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
                    Salary = data["Salary"]
                }, databaseConnector.resumeCollection);
            }
            catch
            {
                //ignore
            }

        }
    }
}

