using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using restworkapi.AFactoryTask;
using restworkapi.Connectors;
using restworkapi.WebSpider;
using restworkapi.WebSpider.Models;
using System.Linq;

namespace restworkapi.AFactoryTask
{
	public class PopulateCategory : ITask
    {
        public PopulateCategory(){}

        public string Name { get => TaskType.PopulateCategory.ToString();}
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
                List<Category> categories = await spider.GetCategory();
                var dbconnector = DatabaseConnector.GetDatabaseConnector();
                var currentCategories = await dbconnector.GetAll(dbconnector.categoryCollection);
                var difference = categories.Where(x =>  currentCategories.FirstOrDefault(y => y.Name == x.Name) == null );
                if (currentCategories.Count == 0)
                    difference = categories;          
                foreach(var item in difference)
                {
                    Models.Database.Category category = new Models.Database.Category();
                    category.Name = item.Name;
                    category.Version = 0;
                    await dbconnector.InsertNewValue(category, dbconnector.categoryCollection);
                }

            }
            catch{
                Status = "false";
                return false;
            }
            Status = "true";
            return true;
        }
    }
}
