using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp;
using System.Net;
using System.Linq;
using restworkapi.WebSpider.Models;

namespace restworkapi.WebSpider
{
    public abstract class WebSpiderBuilder
    {
        public abstract void BuildLink();

        public abstract void BuildFields();

        public abstract WSpider GetWSpider();
    }


    public class BuildWebSpiderRabotamd : WebSpiderBuilder
    {
        private WSpider wSpider = new WSpider();

        public override void BuildFields()
        {
            wSpider.fields = new Dictionary<string, string>();
            wSpider.fields.Add("BaseClass", Sol.VacancyHelper.BaseClass);
        }

        public override void BuildLink()
        {
            wSpider.linkToParse = Sol.RMDVacancyLink;
        }

        public override WSpider GetWSpider()
        {
            return wSpider;
        }
    }

    /* Here you can declare webspider for another source site
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     */

    public class WSpider
    {
        public string linkToParse;
        public Dictionary<string, string> fields;
        public string htmlCode;

        public async Task<List<Category>> GetCategory()
        {
            var categoryList = new List<Category>();
            try
            {
                var config = Configuration.Default.WithDefaultLoader();
                var document = await BrowsingContext.New(config).OpenAsync(Sol.RMDVacancyLink);
                var maindoc = document.QuerySelector("div.b_info12");
                var columns = maindoc.QuerySelectorAll("div.col");
                foreach(var col in columns)
                {
                    var ul = col.QuerySelector("ul");
                    var lis = ul.QuerySelectorAll("li");
                    foreach(var li in lis)
                    {
                        var a = li.QuerySelector("a");
                        categoryList.Add(
                            new Category()
                            {
                                Link = a.GetAttribute("href"),
                                Name = a.TextContent.Trim()
                             
                            });
                    }
                }

            }
            catch (Exception) { return categoryList; }
            return categoryList;
        }

    }
}
