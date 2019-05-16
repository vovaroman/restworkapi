using System;
namespace restworkapi.WebSpider.Models
{
    public class Category
    {
        public static Category Clone(Category category) {
            return category.MemberwiseClone() as Category;
        }
       

        public string Name { get; set; }
        public string LinkVacancy { get; set; }
        public string LinkResume { get => LinkVacancy.Replace("vacancies", "resumes").Replace("vacancy","resumes"); set => LinkResume = value; }
    }

}
