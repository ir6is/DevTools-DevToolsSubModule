using System;
using System.Collections.Generic;
using UnityUnityDevTools.DependencyInjection;
using UnityEngine;

namespace UnityDevTools.Ui
{
    /// <summary>
    /// UIManager.
    /// </summary>
    public class UIService : MonoBehaviour
    {
        #region data

        [SerializeField]
        private Canvas _canvas;
        [SerializeField]
        private View _btnPrefab;

        private IServiceProvider _container;

        #endregion

        #region interface

        public void Initialize(IServiceProvider container)
        {
            _container = container;
        }

        public TController CreateController<TController>(params object[] args)
        {
            var argsList = new List<object>(args);
            var view = Instantiate(_btnPrefab, _canvas.transform); // нашел вью
            argsList.Add(view);
            return (TController)ServiceProviderUtility.CreateInstance<TController>(_container, argsList.ToArray()); //activatorUtilities
        }

        #endregion

        #region MonoBehaviour

        #endregion

        #region implementation

        private void testc(Type type)
        {
            var s = type.GetGenericArguments();
            var d = typeof(Controller<int>).GetGenericArguments();
        }

        #endregion
    }
}