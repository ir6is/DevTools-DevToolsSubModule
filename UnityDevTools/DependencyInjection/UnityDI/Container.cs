using System;
using System.Collections.Generic;

namespace UnityUnityDevTools.DependencyInjection
{
    /// <summary>
    /// Container.
    /// </summary>
    public class Container : IServiceProvider
    {
        #region data

        private readonly Dictionary<Type, IObjectProvider> _providers = new Dictionary<Type, IObjectProvider>();

        #endregion

        #region interface

        public Container RegisterType<T>() where T : class
        {
            _providers[typeof(T)] = new ActivatorObjectProvider<T>(this);
            return this;

        }

        public Container RegisterType<TBase, TDerived>() where TDerived : class, TBase where TBase : class
        {
            _providers[typeof(TBase)] = new ActivatorObjectProvider<TDerived>(this);
            return this;
        }

        public Container RegisterSingleton<T>() where T : class, new()
        {
            _providers[typeof(T)] = new SingletonActivatorObjectProvider<T>(this);
            return this;
        }

        public Container RegisterSingleton<TBase, TDerived>() where TDerived : class, TBase where TBase : class
        {
            _providers[typeof(TBase)] = new SingletonActivatorObjectProvider<TDerived>(this);
            return this;
        }


        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public object GetService(Type serviceType)
        {
            return _providers[serviceType].GetObject();
        }

        #endregion
    }
}