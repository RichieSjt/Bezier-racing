using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSystem : MonoBehaviour
{
    public GameObject trackPoints;
    public List<List<Vector3>> bezierCurves;
    public Transform center;
    private Vector3 _position;
    private Vector3 _previousPosition;
    private Vector3 _targetPoint;
    private float _movementParam;
    private float _distance;
    private int _curveIdx;

    // Getters
    public Vector3 position => _position;
    public Vector3 previousPosition => _previousPosition;

    void Start()
    {
        bezierCurves = new List<List<Vector3>>();

        Transform bezierPoints = trackPoints.transform;
        
        int index = 0;
        // Obtaining the children of track points (Curve containing points)
        foreach (Transform curve in bezierPoints)
        {
            bezierCurves.Add(new List<Vector3>());
            
            // Obtaining  the points of each individual curve and storing it in a List<List<Vector3>>
            foreach (Transform point in curve) {
                bezierCurves[index].Add(point.position);
            }
            index++;
        }
        
        // Variables initialization
        _position = center.position;
        _curveIdx = 0;
        _distance = 1000f;
    }

    private void Update()
    {
        // If we reach the target point
        if(_distance < 0.2f)
        {            
            // Change the curve index
            if (_curveIdx == bezierCurves.Count-1)
                _curveIdx = 0;
            else
                _curveIdx += 1;
            
            // Reset param
            _movementParam = 0;
        }
        
        _movementParam += 0.003f;

        // BEZIER
        List<Vector3> curve = bezierCurves[_curveIdx];

        // The target point is the last control point in the curve
        _targetPoint = curve[curve.Count-1];

        // Calculate the posticion using the bezier curve
        _position = Bezier.EvalBezier(curve, _movementParam);
        _distance = Math3D.Magnitude(_targetPoint - _position);
        _previousPosition = Bezier.EvalBezier(curve, _movementParam-0.0005f);

        center.position = _position;
    }
}
