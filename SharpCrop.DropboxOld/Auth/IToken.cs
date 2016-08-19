using Dropbox.Api;
using SharpCrop.Provider.Models;
using System;

namespace SharpCrop.DropboxOld.Auth
{
    public interface IToken
    {
        void OnToken(Action<string, ProviderState> onToken);

        void Close();
    }
}
