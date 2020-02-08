using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityDevTools.Common
{
    /// <summary>
    /// CoroutineUtility.
    /// </summary>
    public static class CoroutineUtility
    {
        public static IEnumerator LerpCoroutine(Action<float> action, float animationTime)
        {
            var startTime = Time.time;
            var endTime = startTime + animationTime;

            while (endTime >= Time.time)
            {
                yield return null;
                var currentPersent = (Time.time - startTime) / animationTime;
                action(currentPersent);
            }
        }

        public static Task StartTaskCoroutine(Coroutine coroutine)
        {
            var sourse = new TaskCompletionSource<object>();
            StartCoroutine(coroutine, sourse);
            return sourse.Task;
        }

        private static IEnumerator StartCoroutine(Coroutine enumerator, TaskCompletionSource<object> sourse)
        {
            yield return enumerator;
            sourse.SetResult(null);
        }
    }
}