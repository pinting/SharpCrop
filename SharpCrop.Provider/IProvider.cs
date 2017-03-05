using System.IO;
using System.Threading.Tasks;

namespace SharpCrop.Provider
{
    public interface IProvider
    {
        string Id { get; }

        string Name { get; }

        Task<string> Register(string savedState = null, bool showForm = true);

        Task<string> Upload(string name, MemoryStream stream);
    }
}
