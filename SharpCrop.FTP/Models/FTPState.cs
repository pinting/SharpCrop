using System;
using System.IO;
using System.Net;
using System.Threading;

namespace SharpCrop.FTP.Models
{
    public class FTPState
    {
        private ManualResetEvent wait;

        public FTPState()
        {
            wait = new ManualResetEvent(false);
        }

        public ManualResetEvent OperationComplete
        {
            get { return wait; }
        }

        public FtpWebRequest Request;
        public MemoryStream Stream;
        public Exception OperationException;
        public string StatusDescription;
    }
}
