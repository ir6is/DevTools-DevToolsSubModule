using System;
using System.Collections.Generic;

namespace UnityUnityDevTools.DependencyInjection
{
    public interface IObjectProvider
    {
        object GetObject(params object[] args);
    }
}