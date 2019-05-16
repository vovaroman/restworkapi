using System;
namespace restworkapi.WebSpider.Models
{
    public class Job
    {
        public static Job Clone(Job job)
        {
            return job.MemberwiseClone() as Job;
        }

        public string Position { get; set; } // position?
        public string Company { get; set; }
        public string Description { get; set; } // 
        public string Location { get; set; }
        public string Category { get; set; } // 
        public string Experience { get; set; }
        public string Schedule { get; set; }
        public string Education { get; set; }
        public string Date { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Salary { get; set; } // 
        public string Skills { get; set; } //
        public string Photo { get; set; } //
    }
}
