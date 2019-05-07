using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;

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

        public async Task<bool> Invoke()
        {
            try
            {
                    HtmlWeb w = new HtmlWeb();
                    var htmldoc = await w.LoadFromWebAsync(this.linkToParse);
                    HtmlNode mainNode = htmldoc.DocumentNode.SelectNodes($"//div[@class='{fields["BaseClass"]}']")[0];
                    foreach (HtmlNode col in mainNode.ChildNodes)
                    {
                        Console.WriteLine(col);
                    }
                    //foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//span[@class='" + ClassToGet + "']"))
                    //{
                    //    string value = node.InnerText;
                    //    // etc...
                    //}
            }
            catch (Exception ex) { return false; }



            return true;
        }

    }
}
