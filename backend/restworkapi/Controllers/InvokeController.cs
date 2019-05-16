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
                case TaskType.PopulateResume: taskInvoker = new PopulateResume();break;
                case TaskType.PopulateVacancy: taskInvoker = new PopulateVacancy(); break;
                default: taskInvoker = new Dummy();break;
            }
            await taskInvoker.Invoke();
            return true;
        }

        [HttpGet("{category}/{param}")]
        public async Task<bool> Get(string category, string param)
        {
            TaskType taskType = (TaskType)Enum.Parse(typeof(TaskType), category);
            ITask taskInvoker;
            switch (taskType)
            {
                case TaskType.PopulateCategory: taskInvoker = new PopulateCategory(); break;
                case TaskType.PopulateResume: taskInvoker = new PopulateResume(); break;
                case TaskType.PopulateVacancy: taskInvoker = new PopulateVacancy(); break;
                default: taskInvoker = new Dummy(); break;
            }
            if(taskInvoker is PopulateVacancy)
                await (taskInvoker as PopulateVacancy).Invoke(param);
            if (taskInvoker is PopulateResume)
                await (taskInvoker as PopulateResume).Invoke(param);
            return true;
        }



      

    }
}
