using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using restworkapi.AFactoryTask;
using restworkapi.WebSpider;

namespace restworkapi.Controllers
{
    [Route("api/[controller]")]
    public class InvokeController : Controller
    {

        // GET api/values
        [HttpGet("{category}")]
        public async Task<bool> Get(string category)
        {
            TaskType taskType = (TaskType)Enum.Parse(typeof(TaskType), category);
            ITask taskInvoker;
            switch(taskType)
            {
                case TaskType.PopulateCategory: taskInvoker = new PopulateCategory();break;
                case TaskType.PopulateCVs: taskInvoker = new Dummy();break;
                case TaskType.PopulateJobs: taskInvoker = new Dummy(); break;
                default: taskInvoker = new Dummy();break;
            }
            await taskInvoker.Invoke();
            return true;
        }

      

    }
}
