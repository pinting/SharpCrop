using SharpCrop.Dropbox.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCrop.Dropbox
{
    public static class DropboxToken
    {
        public static void GetToken(Action<string> result)
        {
            try
            {
                var server = new LocalServer(result);
            }
            catch
            {
                var webview = new WebView();
                // ...
            }
        }
    }
}
