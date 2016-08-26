using SharpCrop.Provider;
using System;
using System.IO;
using SharpCrop.Provider.Models;
using System.Threading.Tasks;

namespace SharpCrop.GoogleDrive
{
    public class Provider : IProvider
    {
        public Task Register(string token, Action<string, ProviderState> onResult)
        {
            throw new NotImplementedException();
        }

        public Task<string> Upload(string name, MemoryStream stream)
        {
            throw new NotImplementedException();
        }
    }
}
