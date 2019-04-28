using System;
namespace restworkapi.Models.Database
{
    public class Category : IDatabaseObject
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
    }
}
