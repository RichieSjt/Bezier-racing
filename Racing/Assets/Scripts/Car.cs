using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject trackPoints;
    public List<List<Vector3>> bezierPoints;
    public GameObject theCar;
    public GameObject sph;
    private Vector3[] _vertices;
    private Vector3 _position;
    private Vector3 _startPoint;
    private Vector3 _targetPoint;
    private float _param;
    private float _d;
    private int _curveIdx;

    void Start()
    {
        _vertices = theCar.GetComponent<MeshFilter>().mesh.vertices;
        bezierPoints = new List<List<Vector3>>();

        Transform bezierCurves = trackPoints.transform;
        
        int index = 0;
        foreach (Transform curve in bezierCurves)
        {
            bezierPoints.Add(new List<Vector3>());

            foreach (Transform point in curve) {
                bezierPoints[index].Add(point.position);
            }
            index++;
        }

        _position = theCar.transform.position;
        _curveIdx = 0;
        _d = 1000f;
        sph = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }

    // Update is called once per frame
    void Update()
    {
        // If we reach the target point
        if(_d < 0.1f)
        {            
            // Change the target index
            if (_curveIdx == bezierPoints.Count-1)
                _curveIdx = 0;
            else
                _curveIdx += 1;
            
            _param = 0;
        }
        
        sph.transform.position = _position;

        _param += 0.001f;

        // Bezier
        List<Vector3> curve = bezierPoints[_curveIdx];

        _startPoint = curve[0];
        _targetPoint = curve[curve.Count-1];

        _position = Bezier.EvalBezier(curve, _param);
        _d = Math3D.Magnitude(_targetPoint - _position);

        Vector3 prev = Bezier.EvalBezier(curve, _param-0.0005f);
        Vector3 dir = _position - prev;
        Vector3 du = dir.normalized;
        float angle = Mathf.Rad2Deg * Mathf.Atan(du.z/du.x);

        Debug.DrawLine(_position, dir, Color.red);

        Matrix4x4 r = Transformations.RotateM(angle, Transformations.AXIS.AX_Y);
        Matrix4x4 t = Transformations.TranslateM(_position.x, _position.y, _position.z - 6.2f);
        
        theCar.GetComponent<MeshFilter>().mesh.vertices = Transformations.ApplyTransformation(_vertices, t * r);
    }
}
