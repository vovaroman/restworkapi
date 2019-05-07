using System;
namespace restworkapi.Models.Database
{
    public interface IDatabaseObject
    {
        int Id { get; set; }
        int Version { get; set; }
    }
}
