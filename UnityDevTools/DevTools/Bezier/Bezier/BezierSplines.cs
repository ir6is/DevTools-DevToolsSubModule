using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierSplines : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    private float _smoothed = .5f;
    [SerializeField]
    private List<Vector3> _points = new List<Vector3>();
    [SerializeField]
    private List<BezierControlPointMode> _modes = new List<BezierControlPointMode>();

    private float? _lenght;
    private List<float?> _curvesLenght=new List<float?>();

    public int ControlPointCount
    {
        get
        {
            return _points.Count;
        }
    }

    public int CurveCount
    {
        get
        {
            return (_points.Count - 1) / 3;
        }
    }

    public float Lenght
    {
        get
        {
           if (!_lenght.HasValue)
            {
                _lenght = 0;

                if (ControlPointCount != 0)
                {
                    Vector3 p0 = GetControlPoint(0);
                    for (int i = 1; i < ControlPointCount; i += 3)
                    {
                        Vector3 p1 = GetControlPoint(i);
                        Vector3 p2 = GetControlPoint(i + 1);
                        Vector3 p3 = GetControlPoint(i + 2);

                        _lenght += Bezier.GetLenght(p0, p1, p2, p3);
                        p0 = p3;
                    }
                }

            }

            return _lenght.Value;
        }
    }

    public float GetDistanceFrom2CurveIndexes(int curveIndexStart,int curveIndexEnd)
    {
       return GetLenghtFrom2ControlPoints(curveIndexStart*3, curveIndexEnd*3);
    }

    public Vector3 GetControlPoint(int index)
    {
        return _points[index];
    }

    public Vector3 GeWorldControlPoint(int index)
    {
        return transform.TransformPoint(GetControlPoint(index));
    }

    public int ControlPoint2CurvePoint(int index) => index * 3;

    public void SetWorldControlPoint(int index, Vector3 point)
    {
        point = transform.InverseTransformPoint(point);
        SetControlPoint(index, point);
    }

    public void SetControlPoint(int index, Vector3 point)
    {
        _lenght = null;

        if (index % 3 == 0)
        {
            Vector3 delta = point - _points[index];

            if (index > 0)
            {
                _points[index - 1] += delta;
            }
            if (index + 1 < _points.Count)
            {
                _points[index + 1] += delta;
            }
        }
        _points[index] = point;
        EnforceMode(index, true);
    }

    public BezierControlPointMode GetControlPointMode(int index)
    {
        return _modes[(index + 1) / 3];
    }

    public void SetControlPointMode(int index, BezierControlPointMode mode)
    {
        int modeIndex = (index + 1) / 3;
        _modes[modeIndex] = mode;

        EnforceMode(index, true);
    }

    private void EnforceMode(int index, bool smothRequrs = false)
    {
        EnforceUtils.EnforceFree(index, _points, _modes);
        EnforceUtils.EnforceSmoothed(index, _points, _modes, _smoothed);
        EnforceUtils.EnforceAlignedAndMirrowed(index, _points, _modes);
        _lenght = null;
    }

    public Vector3 GetWorldPoint(float t)
    {
        var pointId = GetPointIdentificationEvenlly(t);
        return transform.TransformPoint(Bezier.GetPoint(_points[pointId.i], _points[pointId.i + 1], _points[pointId.i + 2], _points[pointId.i + 3], pointId.t)) ;
    }

    public Vector3 GetVelocity(float t)
    {
        var pointId = GetPointIdentificationEvenlly(t);
        return transform.TransformPoint(Bezier.GetFirstDerivative(_points[pointId.i], _points[pointId.i + 1], _points[pointId.i + 2], _points[pointId.i + 3], pointId.t)) - transform.position; ;
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    public void AddCurve()
    {
        Vector3 point = _points[_points.Count - 1];

        point.x += 1f;
        _points.Add(point);
        point.x += 1f;
        _points.Add(point);
        point.x += 1f;
        _points.Add(point);


        _modes.Add(_modes[_modes.Count - 2]);
        _curvesLenght.Add(null);
        EnforceMode(_points.Count - 4, true);
    }

    public void RemoveCurve()
    {
        _points.RemoveAt(_points.Count - 1);
        _points.RemoveAt(_points.Count - 1);
        _points.RemoveAt(_points.Count - 1);
        _modes.RemoveAt(_modes.Count - 1);
        _curvesLenght.RemoveAt(_curvesLenght.Count - 1);
        _lenght = null;
    }

    public void Reset()
    {
        _points = new List<Vector3> {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f)
        };
        _modes = new List<BezierControlPointMode> {
            BezierControlPointMode.Free,
            BezierControlPointMode.Free
        };

        _lenght = null;
    }

    // todo made it with curve points
    private float GetLenghtFrom2ControlPoints(int controlPointStartIndex, int controlPointIndexEnd)
    {
        var lenght = 0f;

        if (ControlPointCount != 0)
        {
            Vector3 p0 = GetControlPoint(controlPointStartIndex);
            for (int i = controlPointStartIndex + 1; i < controlPointIndexEnd; i += 3)
            {
                Vector3 p1 = GetControlPoint(i);
                Vector3 p2 = GetControlPoint(i + 1);
                Vector3 p3 = GetControlPoint(i + 2);

                lenght += Bezier.GetLenght(p0, p1, p2, p3);
                p0 = p3;
            }
        }

        return lenght;
    }

    // todo artem add cache
    private (int i, float t) GetPointIdentificationUnEvenlly(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = _points.Count - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }


        return (i, t);
    }

    private (int i, float t) GetPointIdentificationEvenlly(float t)
    {
        t = Mathf.Clamp01(t);

        var fullLenght = Lenght;
        var targetLenght = t * fullLenght;
        var currentLenght = 0f;

        for (int i = 0; i < ControlPointCount - 3; i += 3)
        {
            var nextLenght = GetDistanceFrom2CurveIndexes(i / 3, i / 3 + 1);
            currentLenght += nextLenght;

            if (currentLenght >= targetLenght)
            {
                t = (targetLenght - (currentLenght - nextLenght)) / nextLenght;
                return (i, t);
            }
        }

        return (0, 0);
    }
}
