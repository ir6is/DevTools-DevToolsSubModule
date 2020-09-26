
using System.Collections.Generic;
using UnityEngine;

namespace UnityDevTools.Common
{
    public static class RandomizeUtility
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static List<T> CreateShuffledList<T>(this IEnumerable<T> list)
        {
            var newList = new List<T>(list);
            newList.Shuffle();
            return newList;
        }
    }
}