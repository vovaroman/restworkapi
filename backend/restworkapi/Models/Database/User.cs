using System;
namespace restworkapi.Models.Database
{
    public class User : IDatabaseObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Version { get; set; }
    }
}
