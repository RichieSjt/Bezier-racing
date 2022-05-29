using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject trackPoints;
    public List<Vector3> points;
    public GameObject theCar;
    public GameObject sph;
    private Vector3[] originals;
    private Vector3 pos;
    private Vector3 start;
    public float param;
    public float d;
    int pointIdx;

    void Start()
    {
        originals = theCar.GetComponent<MeshFilter>().mesh.vertices;
        // Matrix4x4 t = Transformations.TranslateM(1, 0, 0);
        // theCar.GetComponent<MeshFilter>().mesh.vertices = Transformations.ApplyTransformation(originals, t);
        pos = theCar.transform.position;
        start = pos;

        int children = trackPoints.transform.childCount;
        
        for (int i = 0; i < children; ++i)
        {
            Vector3 point = trackPoints.transform.GetChild(i).GetComponent<Transform>().transform.position;
            points.Add(point);
        }
        pointIdx = 1;
        sph = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPoint = points[pointIdx];
        Debug.Log("CURRENT: " + currentPoint);
        // Debug.Log("CURRENT INDEX: " + pointIdx);

        Debug.Log("CAR POS: " + pos);

        sph.transform.position = pos;
        
        param += 0.001f;
        pos = Interpolation(start, currentPoint, param);
        d = Math3D.Magnitude(currentPoint - pos);
        
        // If we reach the target point
        if(d < 8f)
        {
            // c 0 1 2
            start = points[pointIdx];

            if (pointIdx == points.Count-1)
                pointIdx = 0;
            else
                pointIdx += 1;
            
            Debug.Log("NEW POINT: " + pointIdx);
        }

        Vector3 prev = Interpolation(start, currentPoint, param - 0.00005f);
        Vector3 dir = pos - prev;
        Vector3 du = dir.normalized;

        Matrix4x4 t = Transformations.TranslateM(pos.x, pos.y, pos.z);
        theCar.GetComponent<MeshFilter>().mesh.vertices = Transformations.ApplyTransformation(originals, t);
        
    }

    public static Vector3 Interpolation(Vector3 start, Vector3 end, float param) 
    {
        return (end*param) + (start*-param);
    }
}
