﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace SharpCrop.FTP.Properties {
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [DebuggerNonUserCode()]
    [CompilerGenerated()]
    internal class Resources {
        
        private static ResourceManager resourceMan;
        
        private static CultureInfo resourceCulture;
        
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager {
            get {
                if (Equals(null, resourceMan)) {
                    ResourceManager temp = new ResourceManager("SharpCrop.FTP.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static string CopyPath {
            get {
                return ResourceManager.GetString("CopyPath", resourceCulture);
            }
        }
        
        internal static string Password {
            get {
                return ResourceManager.GetString("Password", resourceCulture);
            }
        }
        
        internal static string ProviderName {
            get {
                return ResourceManager.GetString("ProviderName", resourceCulture);
            }
        }
        
        internal static string RemotePath {
            get {
                return ResourceManager.GetString("RemotePath", resourceCulture);
            }
        }
        
        internal static string Username {
            get {
                return ResourceManager.GetString("Username", resourceCulture);
            }
        }
        
        internal static string Login {
            get {
                return ResourceManager.GetString("Login", resourceCulture);
            }
        }
    }
}
