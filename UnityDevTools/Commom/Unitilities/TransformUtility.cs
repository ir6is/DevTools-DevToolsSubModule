﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformUtility
{
    public  static void DestroyAllChildrens(this Transform transform)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform wireTransform = transform.GetChild(i);

            if (Application.isPlaying)
            {
               GameObject.Destroy(wireTransform.gameObject);
            }
            else
            {
                GameObject.DestroyImmediate(wireTransform.gameObject);
            }
        }
    }
}
