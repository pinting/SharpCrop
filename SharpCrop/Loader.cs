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

        public Loader()
        {
            Load(Settings.Default.Provider);
        }

        public void Load(string name)
        {
            GetProvider(name, provider =>
            {
                if (provider != null)
                {
                    clickForm = new ClickForm(provider);
                    clickForm.Show();
                    return;
                }

                if (mainForm == null)
                {
                    mainForm = new MainForm(this);
                }

                mainForm.Show();
            });
        }

        public void GetProvider(string name, Action<IProvider> onResult)
        {
            IProvider provider;

            switch (name)
            {
                case "Dropbox":
                    provider = new Dropbox.Provider();
                    break;
                default:
                    onResult(null);
                    return;
            }

            provider.Register(Settings.Default.Token, token =>
            {
                if (token == null)
                {
                    ToastFactory.CreateToast("Failed to register provider!");
                    onResult(null);
                    return;
                }

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
