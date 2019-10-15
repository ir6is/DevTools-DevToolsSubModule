using UnityEngine;
using UnityEngine.UI;

namespace UnityDevTools.Ui.Example
{
	/// <summary>
	/// ConctriteController.
	/// </summary>
	public class ConctriteController :Controller<View>
    {
        #region data

        #endregion

        #region interface

        public ConctriteController(View view, IDoubleWorker doubleWorker, int inputInt) : base(view)
        {
            Debug.Log("input params" + inputInt.ToString());
            view.GetComponent<Button>().onClick.AddListener(() => doubleWorker.DoubleWork());
        }

        #endregion

        #region MonoBehaviour

        #endregion

        #region implementation

        #endregion
    }
}