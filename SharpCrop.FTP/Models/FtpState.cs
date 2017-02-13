using System;
using System.IO;
using System.Net;
using System.Threading;

namespace SharpCrop.FTP.Models
{
    public class FtpState
    {
        public FtpState()
        {
            OperationComplete = new ManualResetEvent(false);
        }

        public ManualResetEvent OperationComplete { get; }
        public FtpWebRequest Request;
        public MemoryStream Stream;
        public Exception OperationException;
    }
}