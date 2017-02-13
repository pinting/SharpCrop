using SharpCrop.FTP.Models;
using System;
using System.IO;
using System.Net;

namespace SharpCrop.FTP.Utils
{
    /// <summary>
    /// FTPUploader is a lightly modified version of the official FtpWebRequest example. It can upload a MemoryStream to the given
    /// server using a username and a password.
    /// </summary>
    public static class FtpUploader
    {
        /// <summary>
        /// Upload a MemoryStream object to the given FTP server.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="target"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public static void Upload(MemoryStream stream, Uri target, string username, string password)
        {
            var state = new FtpState();
            var request = (FtpWebRequest)WebRequest.Create(target);

            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(username, password);

            // Store the request in the object that we pass into the asynchronous operations.
            state.Request = request;
            state.Stream = stream;

            // Get the event to wait on.
            var waitObject = state.OperationComplete;

            // Asynchronously get the stream for the file contents.
            request.BeginGetRequestStream(EndGetStreamCallback, state);

            // Block the current thread until all operations are complete.
            waitObject.WaitOne();

            // The operations either completed or threw an exception.
            if (state.OperationException != null)
            {
                throw state.OperationException;
            }
        }
        
        private static void EndGetStreamCallback(IAsyncResult ar)
        {
            var state = (FtpState)ar.AsyncState;

            // End the asynchronous call to get the request stream.
            try
            {
                // Copy the file contents to the request stream.
                var requestStream = state.Request.EndGetRequestStream(ar);

                const int bufferLength = 2048;

                var buffer = new byte[bufferLength];
                int readBytes;
                var count = 0;

                do
                {
                    readBytes = state.Stream.Read(buffer, 0, bufferLength);
                    requestStream.Write(buffer, 0, readBytes);
                    count += readBytes;
                }
                while (readBytes != 0);

                // IMPORTANT: Close the request stream before sending the request.
                requestStream.Close();

                // Asynchronously get the response to the upload request.
                state.Request.BeginGetResponse(EndGetResponseCallback, state);
            }
            catch (Exception e)
            {
                // Return exceptions to the main application thread
                state.OperationException = e;
                state.OperationComplete.Set();
            }

        }

        /// <summary>
        /// The EndGetResponseCallback method completes a call to BeginGetResponse.
        /// </summary>
        /// <param name="ar"></param>
        private static void EndGetResponseCallback(IAsyncResult ar)
        {
            var state = (FtpState)ar.AsyncState;

            try
            {
                var response = (FtpWebResponse)state.Request.EndGetResponse(ar);

                response.Close();

                // Signal the main application thread that the operation is complete
                state.OperationComplete.Set();
            }
            catch (Exception e)
            {
                // Return exceptions to the main application thread
                state.OperationException = e;
                state.OperationComplete.Set();
            }
        }
    }
}
