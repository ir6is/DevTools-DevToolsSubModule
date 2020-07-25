#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierSplines))]
public class BezierSplinesInspector : Editor
{
    private const int _stepsPerCurve = 10;
    private const float _directionScale = 0.5f;

    private static Color[] _modeColors = {
        Color.white,
        Color.yellow,
        Color.cyan,
        Color.red
    };

    private BezierSplines _spline;
    private Transform _handleTransform;
    private Quaternion _handleRotation;
    private int selectedIndex = -1;

    public override void OnInspectorGUI()
    {
        _spline = target as BezierSplines;
        DrawDefaultInspector();

        EditorGUI.BeginChangeCheck();

        if (selectedIndex >= 0 && selectedIndex < _spline.ControlPointCount)
        {
            DrawSelectedPointInspector();
        }
        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(_spline, "Add Curve");
            _spline.AddCurve();
            EditorUtility.SetDirty(_spline);
        }
        if (GUILayout.Button("Remove Curve"))
        {
            Undo.RecordObject(_spline, "Remove Curve");
            _spline.RemoveCurve();
            EditorUtility.SetDirty(_spline);
        }
    }

    private void DrawSelectedPointInspector()
    {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", _spline.GetControlPoint(selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_spline, "Move Point");
            EditorUtility.SetDirty(_spline);
            _spline.SetControlPoint(selectedIndex, point);
        }
        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)EditorGUILayout.EnumPopup("Mode", _spline.GetControlPointMode(selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_spline, "Change Point Mode");
            _spline.SetControlPointMode(selectedIndex, mode);
            EditorUtility.SetDirty(_spline);
        }
    }

    private void OnSceneGUI()
    {
        _spline = target as BezierSplines;
        _handleTransform = _spline.transform;
        _handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            _handleTransform.rotation : Quaternion.identity;

        if (_spline.ControlPointCount != 0)
        {
            Vector3 p0 = ShowPoint(0);
            for (int i = 1; i < _spline.ControlPointCount; i += 3)
            {
                Vector3 p1 = ShowPoint(i);
                Vector3 p2 = ShowPoint(i + 1);
                Vector3 p3 = ShowPoint(i + 2);

                Handles.color = Color.gray;
                Handles.DrawLine(p0, p1);
                Handles.DrawLine(p2, p3);

                Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
                p0 = p3;
            }
            ShowDirections();
        }
    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = _spline.GetWorldPoint(0f);
        Handles.DrawLine(point, point + _spline.GetDirection(0f) * _directionScale);
        int steps = _stepsPerCurve * _spline.CurveCount;
        for (int i = 1; i <= steps; i++)
        {
            point = _spline.GetWorldPoint(i / (float)steps);
            Handles.DrawLine(point, point + _spline.GetDirection(i / (float)steps) * _directionScale);
        }
    }

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = _handleTransform.TransformPoint(_spline.GetControlPoint(index));
        float size = HandleUtility.GetHandleSize(point) * 3;

        Handles.color = _modeColors[(int)_spline.GetControlPointMode(index)];

        const float handleSize = 0.04f;
        const float pickSize = 0.06f;

        //if (index % 3 == 0) /// TODO
        {
            if (Handles.Button(point, _handleRotation, size * handleSize, size * pickSize, Handles.CubeHandleCap))
            {
                selectedIndex = index;
                Repaint();
            }
        }


        if (selectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, _handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_spline, "Move Point");
                EditorUtility.SetDirty(_spline);
                _spline.SetControlPoint(index, _handleTransform.InverseTransformPoint(point));
            }
        }
        return point;
    }
}

#endif