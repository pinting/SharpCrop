using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpCrop.Models;
using SharpCrop.Properties;
using SharpCrop.Provider;

namespace SharpCrop.Utils
{
    /// <summary>
    /// ProviderManager is responsible for handling IProvider objects.
    /// </summary>
    public static class ProviderManager
    {
        private static readonly Dictionary<string, IProvider> registeredProviders = new Dictionary<string, IProvider>();
        private static readonly List<LoadedProvider> loadedProviders = new List<LoadedProvider>();

        /// <summary>
        /// Init loaded providers list.
        /// </summary>
        private static void LoadProviders()
        {
            foreach (var type in Constants.Providers)
            {
                // I know, I know, this looks bad
                // I create a new Instance to write down the Name and Id
                // To help the bellow elsewere, to create another new instance
                var provider = (IProvider)Activator.CreateInstance(type);

                loadedProviders.Add(new LoadedProvider()
                {
                    Id = provider.Id,
                    Name = provider.Name,
                    ProviderType = type
                });
            }
        }

        /// <summary>
        /// Register providers from USER SETTINGS, where they were previously saved!
        /// </summary>
        public static void RestoreProviders()
        {
            // Load available IProvider types into memory
            if (loadedProviders.Count == 0)
            {
                LoadProviders();
            }

            var tasks = new List<Task>();

            // ToList() is needed because the original Dictionary is changing while we iterating
            ConfigHelper.Current.SafeProviders
                .ToList()
                .ForEach(p => tasks.Add(
                    RegisterProvider(GetProviderById(p.Value.Id), p.Key, p.Value.State, false)));

            // Wait for tasks to finish
            tasks.ForEach(async p => await p);
        }

        /// <summary>
        /// Unregister a provider.
        /// </summary>
        /// <param name="name"></param>
        public static void ClearProvider(string name)
        {
            // Remove from the registered providers
            if (registeredProviders.ContainsKey(name))
            {
                registeredProviders.Remove(name);
            }

            // Remove from the configuration file
            if (ConfigHelper.Current.SafeProviders.ContainsKey(name))
            {
                ConfigHelper.Current.SafeProviders.Remove(name);
            }
        }

        /// <summary>
        /// Get a new instance of a IProvider class by name.
        /// </summary>
        /// <param name="name">Name of the class, can be language specific.</param>
        /// <returns></returns>
        public static IProvider GetProviderByName(string name)
        {
            var loaded = loadedProviders.FirstOrDefault(e => e.Name == name);

            if (loaded != null)
            {
                return (IProvider)Activator.CreateInstance(loaded.ProviderType);
            }

            return null;
        }

        /// <summary>
        /// Get a new instance of a IProvider class by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IProvider GetProviderById(string id)
        {
            var loaded = loadedProviders.FirstOrDefault(e => e.Id == id);

            if (loaded != null)
            {
                return (IProvider)Activator.CreateInstance(loaded.ProviderType);
            }

            return null;
        }

        /// <summary>
        /// Try to register a provider.
        /// </summary>
        /// <param name="provider">An IProvider class.</param>
        /// <param name="nick">The nick of the provider - CAN BE ANYTHING!</param>
        /// <param name="savedState">A saved state that the Provider will try to interpret - if it fails, the registration form will be used.</param>
        /// <param name="showForm">Enable or disable registration form.</param>
        /// <returns></returns>
        public static async Task<bool> RegisterProvider(IProvider provider, string nick, string savedState = null, bool showForm = true)
        {
            // If there is already a REGISTERED PROVIDER with this (exact) nick, return with false
            if (provider == null || registeredProviders.ContainsKey(nick))
            {
                return false;
            }

            // Try to register the provider form the savedState
            var state = await provider.Register(savedState, showForm);

            if (showForm && state == null)
            {
                ToastFactory.Create(string.Format(Resources.ProviderRegistrationFailed, provider.Name));
            }
            else if (showForm && state != savedState)
            {
                // If the token is not changed, there was no new registration
                ToastFactory.Create(string.Format(Resources.ProviderRegistrationSucceed, provider.Name));
            }

            // If the the registration failed, return with false
            if (state == null)
            {
                return false;
            }

            // Save the provider to the config
            ConfigHelper.Current.Providers[nick] = new SavedProvider()
            {
                Id = provider.Id,
                State = state
            };

            // Save the provider as registered
            registeredProviders[nick] = provider;

            return true;
        }

        /// <summary>
        /// Currently registered (working) providers.
        /// </summary>
        public static IReadOnlyDictionary<string, IProvider> RegisteredProviders => registeredProviders;

        /// <summary>
        /// Currently loaded (available) providers.
        /// </summary>
        public static IReadOnlyList<LoadedProvider> LoadedProviders => loadedProviders;
    }
}
