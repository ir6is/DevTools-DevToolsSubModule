using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BezierExample : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_points;

    [SerializeField]
    private BezierSplines m_bezierSplines;
    public float offcet;
    public float speedDecr = 100;
    // Update is called once per framess
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(m_bezierSplines.CurveCount);
        }
        if (m_points != null && m_bezierSplines != null)
        {
            for (int i = 0; i < m_points.Length; i++)
            {
                   m_points[i].transform.position = m_bezierSplines.GetWorldPoint(i / (float)(m_points.Length - 1));
            }
        }
    }
}
