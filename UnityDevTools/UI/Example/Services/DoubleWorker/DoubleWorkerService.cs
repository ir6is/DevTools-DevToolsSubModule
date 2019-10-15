using UnityEngine;

namespace UnityDevTools.Ui.Example
{
    /// <summary>
    /// DoubleWorkerService.
    /// </summary>
    public class DoubleWorkerService : IDoubleWorker
    {
        #region data
        private IWokrkerService _workerService;
        #endregion

        #region interface

        public DoubleWorkerService(IWokrkerService workerService)
        {
            _workerService = workerService;
        }

        #endregion

        #region MonoBehaviour

        #endregion

        #region implementation

        #endregion
        public void DoubleWork()
        {
            _workerService.DoImpotantWork();
            Debug.Log("DoubleWork "+ GetHashCode());
        }
    }
}