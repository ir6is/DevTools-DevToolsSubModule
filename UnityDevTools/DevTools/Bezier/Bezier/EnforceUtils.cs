using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnforceUtils
{
    public static void EnforceFree(int index, IReadOnlyList<Vector3> points, IReadOnlyList<BezierControlPointMode> modes)
    {
        int modeIndex = (index + 1) / 3;
        var mode = modes[modeIndex];

        if (mode == BezierControlPointMode.Free)
        {
            return;
        }
    }

    public static void EnforceAlignedAndMirrowed(int index, List<Vector3> points, IReadOnlyList<BezierControlPointMode> modes)
    {
        int modeIndex = (index + 1) / 3;
        var mode = modes[modeIndex];
        var middleIndex = modeIndex * 3;

        if ((mode == BezierControlPointMode.Aligned || mode == BezierControlPointMode.Mirrored) &&!(modeIndex == 0 || modeIndex == modes.Count - 1))
        {
            int fixedIndex, enforcedIndex;

            if (index <= middleIndex)
            {
                fixedIndex = middleIndex - 1;
                if (fixedIndex < 0)
                {
                    fixedIndex = points.Count - 2;
                }
                enforcedIndex = middleIndex + 1;
                if (enforcedIndex >= points.Count)
                {
                    enforcedIndex = 1;
                }
            }
            else
            {
                fixedIndex = middleIndex + 1;
                if (fixedIndex >= points.Count)
                {
                    fixedIndex = 1;
                }
                enforcedIndex = middleIndex - 1;
                if (enforcedIndex < 0)
                {
                    enforcedIndex = points.Count - 2;
                }
            }

            Vector3 middle = points[middleIndex];
            Vector3 enforcedTangent = middle - points[fixedIndex];

            if (mode == BezierControlPointMode.Aligned)
            {
                enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
            }

            points[enforcedIndex] = middle + enforcedTangent;
        }
    }


    // TODO FIX IT change in editor 
    public static void EnforceSmoothed(int index, List<Vector3> points, IReadOnlyList<BezierControlPointMode> modes,float smoothed)
    {
        EnforceSmoothed(index, points, modes, smoothed,true);
    }
    private static void EnforceSmoothed(int index, List<Vector3> points, IReadOnlyList<BezierControlPointMode> modes, float smoothed, bool recursion)
    {
        int modeIndex = (index + 1) / 3;
        var mode = modes[modeIndex];

        if (mode == BezierControlPointMode.Smoothed)
        {
            var middleIndex = modeIndex * 3;

            if (middleIndex != 0)
            {
                //var rotation = Quaternion.identity;
                //try
                //{
                //    if (index%3==0)
                //    {
                //        var a = points[(index-3)];
                //        var b = points[index];
                //        var c = points[(index+3)];

                //        //rotation = Quaternion.FromToRotation(b - a, c - b);
                //        //rotation.Normalize();
                //        Debug.Log(Quaternion.FromToRotation(b - a, c - b).normalized.eulerAngles);

                //    }
                //}
                //catch
                //{

                //}

                //var point = Vector3.Lerp(points[middleIndex], points[middleIndex - 3], smoothed);
                //points[middleIndex - 1] = RotatePointAroundPivot(point,points[middleIndex], rotation);

                points[middleIndex - 1] = Vector3.Lerp(points[middleIndex], points[middleIndex - 3], smoothed);

                if (recursion)
                {
                    EnforceSmoothed(middleIndex - 3, points, modes, smoothed, false);
                }
            }

            if (middleIndex != points.Count - 1)
            {
                points[middleIndex + 1] = Vector3.Lerp(points[middleIndex], points[middleIndex + 3], smoothed);

                if (recursion)
                {
                    EnforceSmoothed(middleIndex + 3, points, modes, smoothed, false);
                }
            }
        }
    }

    private static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angles)
    {
        return angles * (point - pivot) + pivot;
    }
}
