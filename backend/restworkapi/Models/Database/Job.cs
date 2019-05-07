using System;

namespace restworkapi.Models.Database
{
    public class Job : IDatabaseObject
    {
        public int Id { get; set; }
        public string Title { get; set; } // position?
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
        public int Version { get; set; } = 1;


    }
}
