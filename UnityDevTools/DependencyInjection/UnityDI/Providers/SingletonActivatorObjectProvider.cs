using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityUnityDevTools.DependencyInjection
{
	/// <summary>
	/// SingletonActivatorObjectProvider.
	/// </summary>
	public class SingletonActivatorObjectProvider<T> : IObjectProvider where T : class
    {
        #region data

        private readonly IServiceProvider _serviceProvider;
        private object _instance;

        #endregion

        #region interface

        public SingletonActivatorObjectProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region IObjectProvider

        public object GetObject(params object[] args)
        {
            if (_instance == null)
            {
                _instance= ServiceProviderUtility.CreateInstance<T>(_serviceProvider, args);
            }

            return _instance;
        }

        #endregion
    }
}