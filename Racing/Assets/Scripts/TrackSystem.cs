using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSystem : MonoBehaviour
{
    public GameObject trackPoints;
    public List<List<Vector3>> bezierCurves;
    private static List<Vector3> _bezierPath;
    public Transform center;
    private static int _speed;
    public static int speed => _speed;
    private int _pointIdx;

    // Getters
    public static List<Vector3> bezierPath => _bezierPath;

    void Start()
    {
        bezierCurves = new List<List<Vector3>>();
        _bezierPath = new List<Vector3>();

        Transform bezierPoints = trackPoints.transform;
        
        int index = 0;
        // Obtaining the children of track points (Curve containing points)
        foreach (Transform forcurve in bezierPoints)
        {
            bezierCurves.Add(new List<Vector3>());
            
            // Obtaining  the points of each individual curve and storing it in a List<List<Vector3>>
            foreach (Transform point in forcurve) {
                bezierCurves[index].Add(point.position);
            }
            index++;
        }

        // BEZIER
        foreach (List<Vector3> curve in bezierCurves)
        {
            // 100 iterations of bezier for each curve
            for (float _movementParam = 0; _movementParam <= 1; _movementParam += 0.001f)
            {
                _bezierPath.Add(Bezier.EvalBezier(curve, _movementParam));
            }
            

        }

        // Variables initialization
        _pointIdx = 0;
        center.position = bezierPath[_pointIdx];
        _speed = 4;
    }

    public static Vector3 getPosition(int idx)
    {
        return _bezierPath[idx];
    }

    private void Update()
    {
        if (_pointIdx >= bezierPath.Count-speed)
                _pointIdx = 0;
        else
            _pointIdx+=speed;
        
        center.position = bezierPath[_pointIdx];
    }
}
