using SharpCrop.Provider.Models;
using System;
using System.Drawing;

namespace SharpCrop.Provider
{
    public interface IProvider
    {
        void Register(string token, Action<string, ProviderState> onResult);

        string Upload(Bitmap bitmap);
    }
}
