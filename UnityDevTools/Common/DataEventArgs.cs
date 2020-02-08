using System;

namespace UnityDevTools.Common
{
    /// <summary>
    /// DataEventArgs.
    /// </summary>
    public class DataEventArgs<T> : EventArgs
    {
        public T Value { get; }

        public DataEventArgs(T value)
        {
            Value = value;
        }
    }
}