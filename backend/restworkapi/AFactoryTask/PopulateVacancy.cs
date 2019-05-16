using System.Collections.Generic;
using System.Threading.Tasks;
using restworkapi.Connectors;
using restworkapi.WebSpider;
using restworkapi.WebSpider.Models;
using System.Linq;

namespace restworkapi.AFactoryTask
{
    public class PopulateVacancy : ITask
    {
        public PopulateVacancy() { }

        public string Name => TaskType.PopulateVacancy.ToString();
        public string Status { get => _status; set => _status = value; }
        private string _status { get; set; } = "false";

        public async Task<bool> Invoke()
        {
            try
            {
                WebSpiderBuilder webSpiderBuilder = new BuildWebSpiderRabotamd();
                webSpiderBuilder.BuildFields();
                webSpiderBuilder.BuildLink();
                var spider = webSpiderBuilder.GetWSpider();
                var categories = await spider.GetCategory();
                foreach(var category in categories)
                {
                    await Invoke(category.Name);
                }

            }
            catch
            {
                Status = "false";
                return false;
            }
            Status = "true";
            return true;
        }

        public async Task<bool> Invoke(string category)
        {
            try
            {
                WebSpiderBuilder webSpiderBuilder = new BuildWebSpiderRabotamd();
                webSpiderBuilder.BuildFields();
                webSpiderBuilder.BuildLink();
                var spider = webSpiderBuilder.GetWSpider();
                List<Job> dbjobs = await spider.GetJobs(category);
                var dbconnector = DatabaseConnector.GetDatabaseConnector();
                var currentJobs = await dbconnector.GetAll(dbconnector.jobCollection);
                var difference = dbjobs.Where(x => currentJobs.FirstOrDefault(y => y.Position == x.Position) == null);
                if (currentJobs.Count == 0)
                {
                    difference = dbjobs;
                }

                foreach (var item in difference)
                {
                    var job = new Models.Database.Job
                    {
                        Category = item.Category,
                        Company = item.Company,
                        Date = item.Date,
                        Description = item.Description,
                        Education = item.Education,
                        Email = item.Email,
                        Experience = item.Experience,
                        Location = item.Location,
                        Phone = item.Phone,
                        Photo = item.Photo,
                        Position = item.Position,
                        Salary = item.Salary,
                        Schedule = item.Schedule,
                        Skills = item.Skills,
                        Version = 0
                    };
                    await dbconnector.InsertNewValue(job, dbconnector.jobCollection);
                }

            }
            catch
            {
                Status = "false";
                return false;
            }
            Status = "true";
            return true;
        }
    }
}
