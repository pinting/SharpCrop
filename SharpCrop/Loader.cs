using SharpCrop.Forms;
using SharpCrop.Provider;
using SharpCrop.Utils;
using System;
using System.Timers;
using System.Windows.Forms;

namespace SharpCrop
{
    public class Loader : ApplicationContext
    {
        private MainForm mainForm = null;
        private ClickForm clickForm;

        /// <summary>
        /// Loader is responsible for the loading of the application - bah. It gonna try to load
        /// a Provider from settings and when it is fails, it shows the user the MainForm.
        /// </summary>
        public Loader()
        {
            LoadForm(Settings.Default.Provider);
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
                    provider = new Dropbox.Provider();
                    break;
                default:
                    onResult(null);
                    return;
            }
            
            // Try to register Provider
            provider.Register(Settings.Default.Token, token =>
            {
                // On failure - exited by user or something went wrong
                if (token == null)
                {
                    ToastFactory.CreateToast("Failed to register provider!");
                    onResult(null);
                    return;
                }

                // When a new token was created because the old one was expired
                if (Settings.Default.Token != token)
                {
                    Settings.Default.Provider = name;
                    Settings.Default.Token = token;
                    Settings.Default.Save();

                    ToastFactory.CreateToast("Successfully registered provider!");
                }

                Application.DoEvents();
                onResult(provider);
            });
        }
    }
}
