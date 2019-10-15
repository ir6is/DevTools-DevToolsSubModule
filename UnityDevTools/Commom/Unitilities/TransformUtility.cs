using UnityEngine;

namespace UnityDevTools.Common
{
    /// <summary>
    /// TransformUtility.
    /// </summary>
    public static class TransformUtility
    {
        public static void CopyTransformFullExt(this Transform myTransform, Transform copyTransform)
        {
            CopyTransformFull(myTransform, copyTransform);
        }

        public static void CopyTransformFull(Transform myTransform, Transform copyTransform)
        {
            myTransform.position = copyTransform.position;
            myTransform.rotation = copyTransform.rotation;
            myTransform.localScale = copyTransform.localScale;
        }
    }
}