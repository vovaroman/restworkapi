using System;
using System.Threading.Tasks;

namespace restworkapi.AFactoryTask
{
	public class Dummy : ITask
    {
        public Dummy(){}

        public string Name => throw new NotImplementedException();

        public string Status { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task<bool> Invoke()
        {
            return false;
        }
    }
}
