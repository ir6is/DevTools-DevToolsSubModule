using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAspectComponent : MonoBehaviour
{
    private const float m_rightAspect = 0.5625f;

    private Camera m_camera;
    private float m_initialFov;

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
        m_initialFov = m_camera.fieldOfView;
    }
    void Start()
    {
        var oldHorFiledOfView = Camera.VerticalToHorizontalFieldOfView(m_initialFov, m_rightAspect);
        m_camera.fieldOfView = Camera.HorizontalToVerticalFieldOfView(oldHorFiledOfView, m_camera.aspect);
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            Start();
        }
    }
}
