using System.Collections.Generic;
using System.Threading.Tasks;
using restworkapi.Connectors;
using restworkapi.WebSpider;
using restworkapi.WebSpider.Models;
using System.Linq;

namespace restworkapi.AFactoryTask
{
    public class PopulateResume : ITask
    {
        public PopulateResume() { }

        public string Name => TaskType.PopulateResume.ToString();
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
                foreach (var category in categories)
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
                List<CV> dbresumes = await spider.GetCVs(category);
                var dbconnector = DatabaseConnector.GetDatabaseConnector();
                var currentResume = await dbconnector.GetAll(dbconnector.resumeCollection);
                var difference = dbresumes.Where(x => currentResume.FirstOrDefault(y => y.Position == x.Position) == null);
                if (currentResume.Count == 0)
                {
                    difference = dbresumes;
                }
                int i = 0;
                foreach (var item in difference)
                {
                    var resume = new Models.Database.CV
                    {
                        Attachment = item.Attachment,
                        Category = item.Category,
                        Courses = item.Courses,
                        Description = item.Description,
                        Email = item.Email,
                        Education = item.Education,
                        Expirience = item.Expirience,
                        Languages = item.Languages,
                        Phone = item.Phone,
                        Photo = item.Photo,
                        Position = item.Position,
                        Salary = item.Salary,
                        Skills = item.Skills,
                        Version = 0
                    };
                    await dbconnector.InsertNewValue(resume, dbconnector.resumeCollection);

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
