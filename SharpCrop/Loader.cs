using SharpCrop.Forms;
using SharpCrop.Provider;
using SharpCrop.Provider.Models;
using SharpCrop.Utils;
using System;
using System.Windows.Forms;

namespace SharpCrop
{
    public class Loader : ApplicationContext
    {
        private Form mainForm = null;
        private Form clickForm;

        /// <summary>
        /// Loader is responsible for the loading of the application - bah. It gonna try to load
        /// a Provider from settings and when it is fails, it shows the user the MainForm.
        /// </summary>
        public Loader()
        {
            LoadForm(ConfigHelper.Memory.Provider);
        }

        /// <summary>
        /// Load the required form for the given Provider.
        /// </summary>
        /// <param name="name"></param>
        public void LoadForm(string name)
        {
            GetProvider(name, provider =>
            {
                // Provider was created successfully
                if (provider != null)
                {
                    clickForm = new ClickForm(provider);
                    clickForm.Show();
                    return;
                }

                // Provider could not be created, no saved Provider name was found
                if (mainForm == null)
                {
                    mainForm = new MainForm(this);
                }

                mainForm.Show();
            });
        }

        /// <summary>
        /// Get a Provider by name and give it back with a callback function.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="onResult"></param>
        public void GetProvider(string name, Action<IProvider> onResult)
        {
            IProvider provider;

            // Translate name into a real instance.
            switch (name)
            {
                case "Dropbox":
                    provider = new DropboxOld.Provider();
                    break;
                default:
                    onResult(null);
                    return;
            }
            
            // Try to register Provider
            provider.Register(ConfigHelper.Memory.Token, (token, state) =>
            {
                if (token == null)
                {
                    switch(state)
                    {
                        case ProviderState.ServiceError:
                            ToastFactory.CreateToast("Failed to register provider!");
                            break;
                        case ProviderState.UserError:
                            ToastFactory.CreateToast("User aborted the registration!");
                            break;
                        case ProviderState.PermissionError:
                            ToastFactory.CreateToast("You have no permission to do that!");
                            break;
                    }

                    onResult(null);
                    return;
                }
                
                if (state == ProviderState.Refresh)
                {
                    ConfigHelper.Memory.Provider = name;
                    ConfigHelper.Memory.Token = token;
                    ToastFactory.CreateToast("Successfully registered provider!");
                }

                Application.DoEvents();
                onResult(provider);
            });
        }
    }
}
