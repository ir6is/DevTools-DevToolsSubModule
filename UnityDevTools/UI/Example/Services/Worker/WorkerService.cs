using UnityEngine;

namespace UnityDevTools.Ui.Example
{
    /// <summary>
    /// Implementation of <see cref="IWokrkerService"/>.
    /// </summary>
    public class WorkerService : IWokrkerService
    {
        #region data

        #endregion

        #region interface

        #endregion

        #region MonoBehaviour

        #endregion

        #region implementation

        #endregion
        public void DoImpotantWork()
        {
            Debug.Log("Work" +GetHashCode());
        }
    }
}