using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnPointerDownComponent : MonoBehaviour
{
    public event Action<OnPointerDownComponent> OnpointerDown;

    private void OnMouseDown()
    {
        //Debug.Log($"ARTEM DOWN {EventSystem.current.IsPointerOverGameObject()}");
        if (EventSystem.current != null && !EventSystem.current.IsPointerOverGameObject())
        {
            OnpointerDown?.Invoke(this);
        }
    }
}
