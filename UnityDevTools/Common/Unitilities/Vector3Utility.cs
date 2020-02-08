using UnityEngine;

namespace UnityDevTools.Common
{
    public static class Vector3Utility
    {
        public static bool IsApproximateExt(this Vector3 a, Vector3 b)
        {
            return a.IsApproximateExt(b);
        }

        public static bool IsApproximate(Vector3 a, Vector3 b)
        {
            var result = true;

            for (int i = 0; i < 3; i++)
            {
                result = result && Mathf.Approximately(a[i], b[i]);
            }

            return result;
        }
    }
}