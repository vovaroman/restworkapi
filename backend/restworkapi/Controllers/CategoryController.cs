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
    public class CategoryController : Controller
    {
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var output = new List<string>();
            var databaseConnector = DatabaseConnector.GetDatabaseConnector();
            var categories = await databaseConnector.GetAll(databaseConnector.categoryCollection);
            categories.ForEach((obj) => output.Add(JsonConvert.SerializeObject(obj)));
            return output;
        }
    }
}
