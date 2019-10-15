using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityUnityDevTools.DependencyInjection
{
    /// <summary>
    /// ActivatorObjectProvider.
    /// </summary>
    public class ActivatorObjectProvider<T> : IObjectProvider where T : class
    {
        #region data

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region interface

        public ActivatorObjectProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region implementation

        #endregion

        #region IObjectProvider

        public object GetObject(params object[] args)
        {
            return ServiceProviderUtility.CreateInstance<T>(_serviceProvider, args);
        }

        #endregion
    }
}