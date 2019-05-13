using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using restworkapi.Connectors;
using restworkapi.Models.Database;
using Newtonsoft.Json;
using MongoDB.Driver;

namespace restworkapi.Controllers
{
    [Route("api/[controller]")]
    public class CvByCategoryController : Controller
    {

        [HttpGet("{category}")]
        public async Task<IEnumerable<string>> Get(string category)
        {
            var output = new List<string>();
            var databaseConnector = DatabaseConnector.GetDatabaseConnector();
            var filter = Builders<CV>.Filter.Eq("Category", category);
            (await databaseConnector.GetItemsByFilter(filter, databaseConnector.cvCollection)).ForEach((obj) => output.Add(JsonConvert.SerializeObject(obj)));
            return output;
        }

    }
}

