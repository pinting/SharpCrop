using System;

namespace SharpCrop.FTP.Models
{
    /// <summary>
    /// LoginCreds represent the state (savedState) in the upper layers of the application. This is where
    /// the FTP informations are stored.
    /// </summary>
    public class LoginCredentials
    {
        public string Username;
        public string Password;
        public Uri RemotePath;
        public Uri CopyPath;
    }
}
