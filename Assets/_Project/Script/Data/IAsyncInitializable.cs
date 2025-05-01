using System.Collections;

namespace Data
{
    public interface IAsyncInitializable
    {
        IEnumerator InitializeAsync();
    }
}