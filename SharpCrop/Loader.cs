using SharpCrop.Forms;
using SharpCrop.Provider;
using SharpCrop.Provider.Models;
using SharpCrop.Utils;
using System;
using System.Threading.Tasks;
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
            // Needed to construct this here to be in the right SynchronizationContext
            mainForm = new MainForm(this);

            LoadForm(ConfigHelper.Memory.Provider);
        }

        /// <summary>
        /// Load the required form for the given Provider.
        /// </summary>
        /// <param name="name"></param>
        public async void LoadForm(string name)
        {
            await GetProvider(name, provider =>
            {
                if (provider != null)
                {
                    clickForm = new ClickForm(provider);
                    clickForm.Show();
                }
                else
                {
                    mainForm.Show();
                }
            });
        }

        /// <summary>
        /// Get a Provider by name and give it back with a callback function.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="onResult"></param>
        public async Task GetProvider(string name, Action<IProvider> onResult)
        {
            IProvider provider;

            // Translate name into a real instance.
            switch (name)
            {
                case "Dropbox":
                    provider = new Dropbox.Provider();
                    break;
                default:
                    onResult(null);
                    return;
            }

            // Try to register Provider
            await provider.Register(ConfigHelper.Memory.Token, (token, state) =>
            {
                if (token == null)
                {
                    switch (state)
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

                if (state == ProviderState.NewToken)
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
