using UnityEngine;

namespace UnityDevTools.Common
{
    /// <summary>
    /// DestroyOnStart.
    /// </summary>
    public class DestroyOnStart : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(gameObject);
        }
    }
}