using SharpCrop.Provider.Models;
using System;
using System.IO;

namespace SharpCrop.Provider
{
    public interface IProvider
    {
        void Register(string token, Action<string, ProviderState> onResult);

        string Upload(string name, MemoryStream stream);
    }
}
