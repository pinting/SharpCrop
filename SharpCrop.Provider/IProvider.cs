using SharpCrop.Provider.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SharpCrop.Provider
{
    public interface IProvider
    {
        Task Register(string token, Action<string, ProviderState> onResult);

        Task<string> Upload(string name, MemoryStream stream);
    }
}
