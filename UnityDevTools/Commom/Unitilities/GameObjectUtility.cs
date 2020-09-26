using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectUtility : MonoBehaviour
{
    public static void MoveToLayer(GameObject root, int layer)
    {
        root.layer = layer;
        foreach (Transform child in root.transform)
            MoveToLayer(child.gameObject, layer);
    }
}
