using Dropbox.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCrop.Interfaces
{
    interface IGrabber
    {
        void OnToken(Action<OAuth2Response> onToken);

        void Close();
    }
}
