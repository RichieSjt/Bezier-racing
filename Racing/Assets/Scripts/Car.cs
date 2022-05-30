using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject trackPoints;
    public List<Vector3> points;
    public GameObject theCar;
    public GameObject sph;
    private Vector3[] _vertices;
    private Vector3 _position;
    private Vector3 _startPoint;
    private Vector3 _targetPoint;
    private float _param;
    private float _d;
    private int _pointIdx;

    void Start()
    {
        _vertices = theCar.GetComponent<MeshFilter>().mesh.vertices;

        int children = trackPoints.transform.childCount;
        
        for (int i = 0; i < children; ++i)
        {
            Vector3 point = trackPoints.transform.GetChild(i).GetComponent<Transform>().transform.position;
            points.Add(point);
        }

        _position = theCar.transform.position;
        _startPoint = _position;
        _pointIdx = 1;
        _targetPoint = points[_pointIdx];

        sph = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }

    // Update is called once per frame
    void Update()
    {
        sph.transform.position = _position;

        _param += 0.001f;
        _position = Math3D.Interpolation(_startPoint, _targetPoint, _param);
        _d = Math3D.Magnitude(_targetPoint - _position);
        
        // If we reach the target point
        if(_d < 0.1f)
        {
            // Set the new start position
            _startPoint = points[_pointIdx];
            
            // Change the target index
            if (_pointIdx == points.Count-1)
                _pointIdx = 0;
            else
                _pointIdx += 1;
            
            // Change the targert point
            _targetPoint = points[_pointIdx];
            _param = 0;
        }

        // Vector3 prev = Math3D.Interpolation(startPoint, targetPoint, param - 0.00005f);
        // Vector3 dir = position - prev;
        // Vector3 du = dir.normalized;

        Matrix4x4 t = Transformations.TranslateM(_position.x, _position.y, _position.z);
        theCar.GetComponent<MeshFilter>().mesh.vertices = Transformations.ApplyTransformation(_vertices, t);
    }
}
