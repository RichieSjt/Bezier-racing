using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject theCar;
    private Vector3[] originals;
    private Vector3 pos;
    public Vector3 start;
    public Vector3 end;
    public float param;
    public float d;

    void Start()
    {
        originals = theCar.GetComponent<MeshFilter>().mesh.vertices;
        Matrix4x4 t = Transformations.TranslateM(1, 0, 0);
        theCar.GetComponent<MeshFilter>().mesh.vertices = Transformations.ApplyTransformation(originals, t);

        // In the project there are only translate and rotate

    }

    // Update is called once per frame
    void Update()
    {

        if(d > 0)
        {
            param += 0.001f;
            pos = Interpolation(start, end, param);
            Debug.Log("POS:" + pos.ToString("F5"));
            d = Math3D.Magnitude(end - pos);
            Debug.Log("D: " + d);
            Vector3 prev = Interpolation(start, end, param - 0.00005f);
            Vector3 dir = pos - prev;
            Vector3 du = dir.normalized;

            Matrix4x4 t = Transformations.TranslateM(pos.x, pos.y, pos.z);
            theCar.GetComponent<MeshFilter>().mesh.vertices = Transformations.ApplyTransformation(originals, t);
        }
    }

    public static Vector3 Interpolation(Vector3 start, Vector3 end, float param) 
    {
        return (end*param) + (start*-param);
    }
}
