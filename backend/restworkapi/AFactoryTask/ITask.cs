using System;
using System.Threading.Tasks;

namespace restworkapi.AFactoryTask
{
    public interface ITask
    {
        Task<bool> Invoke();
        string Name { get; }
        string Status { get; set; }
    }
}
