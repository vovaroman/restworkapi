using System;
using System.Collections.Generic;

namespace restworkapi.Models.Database
{
    public class CV : IDatabaseObject
    {
        public int Id { get; set; }
        public string Position { get; set; } // 
        public string Description { get; set; } //
        public string Photo { get; set; } //
        public string Languages { get; set; }
        public string Attachment { get; set; }
        public string Category { get; set; } //
        public string Education { get; set; }
        public string Courses { get; set; }
        public string Expirience { get; set; } //
        public string Skills { get; set; } //
        public string Phone { get; set; }//
        public string Email { get; set; }//
        public string Salary { get; set; } //
        public int Version { get; set; } = 0;
        public int UserId { get; set; } = 0;
    }
}
