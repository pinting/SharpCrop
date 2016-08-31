using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpCrop.GoogleDrive.Utils
{
    /// <summary>
    /// A simple in-memory key-value store for Google Drive. This is gonna be serialized and held as
    /// a token in the main application config.
    /// </summary>
    public class MemoryStore : IDataStore
    {
        private Dictionary<string, string> storage = new Dictionary<string, string>();

        /// <summary>
        /// Consturct a store with an optional previous state.
        /// </summary>
        /// <param name="state"></param>
        public MemoryStore(string state = null)
        {
            Import(state);
        }

        /// <summary>
        /// Export dictionary as a string.
        /// </summary>
        /// <returns></returns>
        public string Export()
        {
            return JsonConvert.SerializeObject(storage);
        }

        /// <summary>
        /// Import a dictionary which is serialized into a string.
        /// </summary>
        /// <param name="serialized"></param>
        public void Import(string serialized)
        {
            if (!string.IsNullOrEmpty(serialized))
            {
                storage = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialized);
            }
        }

        /// <summary>
        /// Clear the store.
        /// </summary>
        /// <returns></returns>
        public Task ClearAsync()
        {
            storage.Clear();

            return Task.Delay(0);
        }

        /// <summary>
        /// Delete en element by key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task DeleteAsync<T>(string key)
        {
            if (!string.IsNullOrEmpty(key) && storage.ContainsKey(key))
            {
                storage.Remove(key);
            }

            return Task.Delay(0);
        }

        /// <summary>
        /// Get the given element by key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(string key)
        {
            var result = new TaskCompletionSource<T>();

            if (!string.IsNullOrEmpty(key) && storage.ContainsKey(key))
            {
                result.SetResult(JsonConvert.DeserializeObject<T>(storage[key]));
            }
            else
            {
                result.SetResult(default(T));
            }

            return result.Task;
        }

        /// <summary>
        /// Store a key-value pair.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task StoreAsync<T>(string key, T value)
        {
            if (!string.IsNullOrEmpty(key) && !storage.ContainsKey(key))
            {
                storage.Add(key, JsonConvert.SerializeObject(value));
            }

            return Task.Delay(0);
        }
    }
}
