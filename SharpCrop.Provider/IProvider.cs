using System;
using System.Drawing;

namespace SharpCrop.Provider
{
    public interface IProvider
    {
        void Register(string token, Action<string> onResult);

        string Upload(Bitmap bitmap);
    }
}
