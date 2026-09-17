using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CourbeBezier : MonoBehaviour
{
    public Transform Point1;
    public Transform Point2;
    public Transform Point3;
    public Transform Point4;

    public bool courbeCubique = true;
    public int resolution = 50;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        DessinerCourbe();
    }

    void Update()
    {
        DessinerCourbe();
    }

    void DessinerCourbe()
    {
        if (Point1 == null || Point2 == null || Point3 == null)
            return;

        lineRenderer.positionCount = resolution + 1;

        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;

            Vector3 position;

            if (courbeCubique && Point4 != null)
            {
                position = CourbeCubique(
                    Point1.position,
                    Point2.position,
                    Point3.position,
                    Point4.position,
                    t
                );
            }
            else
            {
                position = CourbeQuadratique(
                    Point1.position,
                    Point2.position,
                    Point3.position,
                    t
                );
            }

            lineRenderer.SetPosition(i, position);
        }
    }

    Vector3 CourbeQuadratique(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;

        return u * u * p0
             + 2 * u * t * p1
             + t * t * p2;
    }

    Vector3 CourbeCubique(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;

        return u * u * u * p0
             + 3 * u * u * t * p1
             + 3 * u * t * t * p2
             + t * t * t * p3;
    }
}