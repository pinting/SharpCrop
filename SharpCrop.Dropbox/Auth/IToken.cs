using Dropbox.Api;
using System;

namespace SharpCrop.Dropbox.Auth
{
    interface IToken
    {
        void OnToken(Action<string> onToken);

        void Close();
    }
}
