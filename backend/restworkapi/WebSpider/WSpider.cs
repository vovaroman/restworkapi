using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp;
using System.Net;
using System.Linq;
using restworkapi.WebSpider.Models;
using restworkapi.Connectors;
using System.Text.RegularExpressions;
using System.Text;

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

        private string ParseEmail(string description)
        {
            Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            RegexOptions.IgnoreCase);
            MatchCollection emailMatches = emailRegex.Matches(description);

            StringBuilder sb = new StringBuilder();
            if(emailMatches.FirstOrDefault() != null)
                sb.AppendLine(emailMatches.FirstOrDefault().Value);
            return sb.ToString();
        }

        private string ParsePhone(string description)
        {
            var phones =  new string[]{ "Телефон", "Phone", "Numarul","Mob" };
            foreach(var phone in phones)
            {
                try
                {
                    if (description.Contains(phone))
                    {
                        return description.Substring(description.IndexOf(phone, StringComparison.CurrentCulture), description.IndexOf(" ", description.IndexOf(phone, StringComparison.CurrentCulture), StringComparison.CurrentCulture));
                    }
                }
                catch(Exception) {}
            }

            return string.Empty;
        }

        private string ConvertImageToBASE64FromPath(string path)
        {
            try
            {
                var webClient = new WebClient();
                byte[] imageBytes = webClient.DownloadData(path);
                return Convert.ToBase64String(imageBytes);
            }
            catch{
                return string.Empty;
            }
        }


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
                                LinkVacancy = a.GetAttribute("href"),
                                Name = a.TextContent.Trim()
                             
                            });
                    }
                }

            }
            catch (Exception) { return categoryList; }
            return categoryList;
        }

        public string GetInnerOfBlockAsString(AngleSharp.Dom.IElement block )
        {
            var stringToReturn = string.Empty;
            try
            {
                var courseBlock = block.ParentElement;
                var rsmBlockItem = courseBlock.QuerySelectorAll("div.rsm_block_item");
                foreach (var blockItem in rsmBlockItem)
                {

                    var years = string.Empty;
                    try{
                        years = courseBlock.QuerySelectorAll("span")[0].TextContent.Trim();
                    }catch{}
                    var rightChild = blockItem.QuerySelector("div.rsm_block_right");
                    string innerInfo = string.Empty;
                    foreach (var rightChildBlock in rightChild.ChildNodes)
                    {
                        var innerData = innerInfo.Contains(rightChild.FirstElementChild.TextContent.Trim()) == true ? string.Empty : rightChild.FirstElementChild.TextContent.Trim();
                        innerInfo += innerData;
                    }
                    stringToReturn += years + " " + innerInfo;

                    
                }
            }
            catch { }
            return stringToReturn;
        }

        public async Task<List<CV>> GetCVs(string category)
        {
            var cvsList = new List<CV>();

            try{

                var dbconnector = DatabaseConnector.GetDatabaseConnector();
                var currentCategories = await dbconnector.GetAll(dbconnector.categoryCollection);
                var categories = await GetCategory();
                var currentJob = categories.FirstOrDefault(x => x.Name == category);
                if (currentJob == null) return cvsList;
                var config = Configuration.Default.WithDefaultLoader();
                var document = await BrowsingContext.New(config).OpenAsync(Sol.RMDLink + currentJob.LinkResume);
                var table = document.GetElementById("result-table"); //QuerySelectorAll("div#result-table");
                var items = table.QuerySelectorAll("div.preview");
                foreach(var item in items)
                {
                    var h2 = item.QuerySelector("h2");
                    var link = h2.QuerySelector("a").GetAttribute("href");
                    document = await BrowsingContext.New(config).OpenAsync(Sol.RMDLink + "/resumes/"+ link);
                    var cv = new CV();
                    var h1 = document.QuerySelectorAll("h1");
                    cv.Position = document.QuerySelectorAll("h1")[0].TextContent.Trim();

                    var summary = document.QuerySelector("div.summary");
                    var summaryCols = summary.QuerySelectorAll("div.col");
                    cv.Salary = summaryCols[0].QuerySelectorAll("span")[1].TextContent.Trim();
                    var photoLink = document.QuerySelector("div#rsm_photo_left").QuerySelector("img").GetAttribute("src");

                    cv.Photo = ConvertImageToBASE64FromPath(Sol.RMDLink + photoLink);
                    cv.Category = category;
                    document.QuerySelectorAll("div.title_education").ToList().ForEach(x =>
                    {
                        switch(x.TextContent.Trim()){
                            case "Курсы и треннинги" : cv.Courses = GetInnerOfBlockAsString(x);break;
                            case "Учился": cv.Education = GetInnerOfBlockAsString(x); break;
                            case "Опыт работы": cv.Expirience = GetInnerOfBlockAsString(x); break;
                            case "Ключевые навыки": cv.Skills = GetInnerOfBlockAsString(x); break;
                        }
                    });
                    try
                    {
                        cv.Description = document.GetElementById("rsm_block_about").QuerySelector("p.bigger_lh").TextContent.Trim();
                    }catch{}
                    cv.Attachment = string.Empty;
                    cv.Languages = string.Empty;
                    cv.Phone = string.Empty;
                    cvsList.Add(cv);
                    if (cvsList.Count == 5)
                        return cvsList;
                }   

            }catch{
                
            }


            return cvsList;
        }


        public async Task<List<Job>> GetJobs(string jobcategory)
        {
            var jobList = new List<Job>();
            try
            {
                var dbconnector = DatabaseConnector.GetDatabaseConnector();
                var currentCategories = await dbconnector.GetAll(dbconnector.categoryCollection);
                var categories = await GetCategory();
                var currentJob = categories.FirstOrDefault(x => x.Name == jobcategory);
                if (currentJob == null) return jobList;
                var config = Configuration.Default.WithDefaultLoader();
                var document = await BrowsingContext.New(config).OpenAsync(Sol.RMDLink + currentJob.LinkVacancy);
                var indiv = document.QuerySelectorAll("div.in");
                var items = indiv[5].QuerySelectorAll("div.preview");
                foreach(var item in items)
                {
                    try
                    {
                        var spantitle = item.QuerySelectorAll("span").LastOrDefault();
                        var title = spantitle.TextContent.Trim();
                        var ul = item.QuerySelector("ul");
                        var lis = ul.QuerySelectorAll("li");
                        foreach (var li in lis)
                        {
                            try
                            {
                                var companyTitle = document.QuerySelector("ul.breadcrumbs");
                                var lisTitle = string.Empty;
                                try{
                                    lisTitle = companyTitle.QuerySelectorAll("li")[2].QuerySelector("a").TextContent.Trim();
                                }catch{}
                                var linkOnVacancy = li.QuerySelectorAll("a")[1].GetAttribute("href");
                                document = await BrowsingContext.New(config).OpenAsync(Sol.RMDLink + linkOnVacancy);
                                var preview = document.QuerySelectorAll("div.preview")[0];
                                var logo = preview.QuerySelector("div.logo");
                                var logoLink = string.Empty;
                                if (logo != null && logo.QuerySelector("img") != null)
                                    logoLink = logo.QuerySelector("img").GetAttribute("src");
                                var inbody = preview.QuerySelectorAll("div.inbody")[0];
                                var description = string.Empty;
                                var vacancy = inbody.QuerySelector("h1").TextContent.Trim();
                                foreach (var p in inbody.Children)
                                {
                                    description += p.TextContent.Trim();
                                }
                                var footBody = preview.QuerySelector("div.footbody");
                                if(footBody != null)
                                {
                                    var spans = footBody.QuerySelectorAll("span");
                                    foreach(var span in spans)
                                    {
                                        description += span.TextContent.Trim();
                                    }
                                }

                                var tempJob = new Job()
                                {
                                    Description = description,
                                    Position = vacancy,
                                    Email = ParseEmail(description),
                                    Category = jobcategory,
                                    Date = DateTime.Today.ToShortDateString(),
                                    Phone = ParsePhone(description),
                                    Photo = ConvertImageToBASE64FromPath(Sol.RMDLink + logoLink),
                                    Education = string.Empty,
                                    Experience = string.Empty,
                                    Salary = string.Empty,
                                    Location = string.Empty,
                                    Skills = string.Empty,
                                    Schedule = string.Empty,
                                    Company = lisTitle

                                };
                                jobList.Add(tempJob);
                                if (jobList.Count == 5)
                                    return jobList;
                            }
                            catch (Exception) { continue; }


                        }
                    }
                    catch (Exception) { continue; }
                }

            }
            catch (Exception) { return jobList; }


            return jobList;
        }

    }
}
