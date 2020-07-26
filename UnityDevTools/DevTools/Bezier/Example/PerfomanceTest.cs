using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfomanceTest : MonoBehaviour
{
    private const float SpeedRange = 5f;
    private const int Period = 10;

    public BezierSplines bezierSplines;
    public GameObject ObjectToMove;
     public Bounds Bounds = new Bounds(Vector3.zero, Vector3.one);
    [Range(2, 2000)] public int PointsCount = 100;
    [Range(1, 500)] public int ObjectsCount = 100;

    private float startTime = -1000;



    private List<GameObject> m_gameObjects= new List<GameObject>();
    private List<float> m_distances= new List<float>();
    private List<float> m_speeds= new List<float>();

    private Vector3[] oldPos;
    private Vector3[] newPos;

    void Start()
    {
        bezierSplines.SetControlPointMode(0, BezierControlPointMode.Aligned);
        bezierSplines.SetControlPointMode(1, BezierControlPointMode.Aligned);

        oldPos = new Vector3[PointsCount];
        newPos = new Vector3[PointsCount];

        for (var i = 2; i < PointsCount; i++)
        {
            bezierSplines.AddCurve();
            bezierSplines.SetWorldControlPoint(i*3, RandomVector());
        }

        var totalDistance = bezierSplines.Lenght;
        oldDistance = bezierSplines.Lenght;
        for (var i = 0; i < ObjectsCount; i++)
        {
            var obj = Instantiate(ObjectToMove,transform);
            obj.transform.parent = transform;

            m_distances.Add(Random.Range(0, totalDistance));
            m_gameObjects.Add(obj);
            m_speeds.Add(Random.Range(0, 2) == 0 ? Random.Range(-SpeedRange, -SpeedRange * 0.3f) : Random.Range(SpeedRange * 0.3f, SpeedRange));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > Period)
        {
            startTime = Time.time;
            for (var i = 0; i < PointsCount; i++)
            {
                oldPos[i] = newPos[i];
                newPos[i] = RandomVector();
            }
        }

        var ratio = (Time.time - startTime) / Period;

        for (var i = 0; i < PointsCount; i++)
        {
            bezierSplines.SetWorldControlPoint(i * 3, Vector3.Lerp(oldPos[i], newPos[i], ratio));
        }


        //move objects
        var totalDistance = bezierSplines.Lenght;
        if (ObjectToMove != null)
        {
            var remapRatio = totalDistance / oldDistance;
            for (var i = 0; i < ObjectsCount; i++)
            {
                var distance = m_distances[i];

                //since curve's length changed-remap
                distance = distance * remapRatio;

                distance = distance + m_speeds[i] * Time.deltaTime;
                if (distance < 0)
                {
                    m_speeds[i] = -m_speeds[i];
                    distance = 0;
                }
                else if (distance > totalDistance)
                {
                    m_speeds[i] = -m_speeds[i];
                    distance = totalDistance;
                }
                m_distances[i] = distance;

                
                m_gameObjects[i].transform.position = bezierSplines.GetWordPointByDitsance(distance); ;
            }
        }
        oldDistance = totalDistance;
    }

    private float oldDistance;

    private Vector3 RandomVector()
    {
        return new Vector3(Range(0), Range(1), Range(2));
    }

    private float Range(int index)
    {
        return Random.Range(Bounds.min[index], Bounds.max[index]);
    }
}
