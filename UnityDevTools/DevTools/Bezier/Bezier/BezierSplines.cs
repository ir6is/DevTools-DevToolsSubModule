using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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

    [SerializeField]
    [HideInInspector]
    private float _lenght=float.NaN;
    [SerializeField]
    [HideInInspector]
    private List<float> _curvesLenght = new List<float>() { float.NaN };

    public int ControlPointCount => _points.Count;
    public int CurveCount => (_points.Count - 1) / 3;

    public float Lenght
    {
        get
        {
            if (float.IsNaN(_lenght))
            {
                _lenght = 0;

                for (int i = 0; i < CurveCount - 1; i++)
                {
                    _lenght += GetLenghtFrom2ControlPoints(i, i + 1);
                }
            }

            return _lenght;
        }
    }

    public float GetDistanceFrom2CurveIndexes(int curveIndexStart, int curveIndexEnd) => GetLenghtFrom2ControlPoints(curveIndexStart * 3, curveIndexEnd * 3);

    public Vector3 GetControlPoint(int index) => _points[index];

    public Vector3 GeWorldControlPoint(int index) => transform.TransformPoint(GetControlPoint(index));
    public Vector3 GetWordPointByDitsance(float distance)
    {
        var pointId = GetPointIdentificatorFromDistance(distance);
        return transform.TransformPoint(Bezier.GetPoint(_points[pointId.i], _points[pointId.i + 1], _points[pointId.i + 2], _points[pointId.i + 3], pointId.t));
    }

    public Vector3 GetDirection(float t) => GetVelocity(t).normalized;

    public Vector3 GetWorldPoint(float t)
    {
        var pointId = GetPointIdentificationEvenlly(t);
        return transform.TransformPoint(Bezier.GetPoint(_points[pointId.i], _points[pointId.i + 1], _points[pointId.i + 2], _points[pointId.i + 3], pointId.t));
    }

    public Vector3 GetVelocity(float t)
    {
        var pointId = GetPointIdentificationEvenlly(t);

        return transform.TransformPoint(Bezier.GetFirstDerivative(_points[pointId.i], _points[pointId.i + 1], _points[pointId.i + 2], _points[pointId.i + 3], pointId.t)) - transform.position; ;
    }


    public BezierControlPointMode GetControlPointMode(int index)
    {
        return _modes[(index + 1) / 3];
    }

    public void SetWorldControlPoint(int index, Vector3 point)
    {
        point = transform.InverseTransformPoint(point);
        SetControlPoint(index, point);
    }

    public void SetControlPoint(int index, Vector3 point)
    {
        _lenght = float.NaN;

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
        EnforceMode(index);
    }

    public void SetControlPointMode(int index, BezierControlPointMode mode)
    {
        int modeIndex = (index + 1) / 3;
        _modes[modeIndex] = mode;

        EnforceMode(index);
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
        _curvesLenght.Add(float.NaN);
        EnforceMode(_points.Count - 4);
        _lenght = float.NaN;
    }

    public void InsertCurve(int curveIndex, BezierControlPointMode controlPointMode=BezierControlPointMode.Free)
    {
        Vector3 point = _points[curveIndex*3];

        _points.Insert(curveIndex * 3+1,point);
        _points.Insert(curveIndex * 3 + 1, point);
        _points.Insert(curveIndex * 3 + 1, point);

        _modes.Insert(curveIndex, controlPointMode);
        _curvesLenght.Insert(curveIndex,float.NaN);
        _lenght = float.NaN;
        EnforceMode(curveIndex);
    }

    public void RemoveCurve()
    {
        if (CurveCount != 1)
        {
            _points.RemoveAt(_points.Count - 1);
            _points.RemoveAt(_points.Count - 1);
            _points.RemoveAt(_points.Count - 1);
            _modes.RemoveAt(_modes.Count - 1);
            _curvesLenght.RemoveAt(_curvesLenght.Count - 1);
            _lenght = float.NaN;
        }
    }

    public void RemoveAtCurve(int indexRemoveAt)
    {
        if (CurveCount != 1)
        {
            _points.RemoveAt(indexRemoveAt*3);
            _points.RemoveAt(indexRemoveAt * 3); 
            _points.RemoveAt(indexRemoveAt * 3);

            _modes.RemoveAt(indexRemoveAt);
            _curvesLenght.RemoveAt(indexRemoveAt);
            _lenght = float.NaN;
        }
    }

    public void Reset()
    {
        _curvesLenght = new List<float>()
        {
            float.NaN
        };

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

        _lenght = float.NaN;
    }

    private float GetLenghtFrom2ControlPoints(int controlPointStartIndex, int controlPointIndexEnd)
    {
        var lenght = 0f;

        if (ControlPointCount != 0)
        {
            for (int i = controlPointStartIndex; i < controlPointIndexEnd; i += 3)
            {
                var curveIndexe = i / 3;

                if (float.IsNaN(_curvesLenght[curveIndexe]))
                {
                    Vector3 p0 = GetControlPoint(i);
                    Vector3 p1 = GetControlPoint(i + 1);
                    Vector3 p2 = GetControlPoint(i + 2);
                    Vector3 p3 = GetControlPoint(i + 3);

                    var curveLenght = Bezier.GetLenght(p0, p1, p2, p3);
                    _curvesLenght[curveIndexe] = curveLenght;
                    lenght += curveLenght;
                }
                else
                {

                    lenght += _curvesLenght[curveIndexe];
                }
            }
        }

        return lenght;
    }

    private void EnforceMode(int index)
    {
        EnforceUtils.EnforceFree(index, _points, _modes);
        EnforceUtils.EnforceSmoothed(index, _points, _modes, _smoothed);
        EnforceUtils.EnforceAlignedAndMirrowed(index, _points, _modes);
        _lenght = float.NaN;

        var curveIndex = index / 3;

        if (curveIndex != _curvesLenght.Count)
        {
            _curvesLenght[curveIndex] = float.NaN;
        }

        if (index % 3 == 0 && index >= 3)
        {
            _curvesLenght[curveIndex - 1] = float.NaN;
        }
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
        var fullLenght = Lenght;
        t = Mathf.Clamp01(t);

        var targetLenght = t * fullLenght;
        return GetPointIdentificatorFromDistance(targetLenght);
    }

    private (int i, float t) GetPointIdentificatorFromDistance(float distance)
    {
        var fullLenght = Lenght;

        distance = Mathf.Clamp(distance, 0, fullLenght);

        var currentLenght = 0f;

        for (int i = 0; i < ControlPointCount - 3; i += 3)
        {
            var nextLenght = GetDistanceFrom2CurveIndexes(i / 3, i / 3 + 1);
            currentLenght += nextLenght;

            if (currentLenght >= distance)
            {
                var t = (distance - (currentLenght - nextLenght)) / nextLenght;
                return (i, t);
            }
        }

        return (0, 0);
    }
}
