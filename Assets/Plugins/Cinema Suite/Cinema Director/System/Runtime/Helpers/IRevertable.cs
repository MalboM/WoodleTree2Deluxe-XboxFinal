// Cinema Suite
using UnityEngine;

namespace CinemaDirector.Helpers
{
    /// <summary>
    /// Implement this interface with any timeline item that manipulates data in a scene.
    /// </summary>
    public interface IRevertable
    {
        RevertMode EditorRevertMode { get; set; }
        RevertMode RuntimeRevertMode { get; set; }

        RevertInfo[] CacheState();
    }
}
