using System.IO;
using System.Threading.Tasks;

namespace SharpCrop.Provider
{
    public interface IProvider
    {
        Task<string> Register(string savedState, bool showForm = true);

        Task<string> Upload(string name, MemoryStream stream);
    }
}
