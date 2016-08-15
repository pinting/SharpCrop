using Dropbox.Api;
using System;

namespace SharpCrop.Dropbox.Auth
{
    interface IToken
    {
        void OnToken(Action<OAuth2Response> onToken);

        void Close();
    }
}
