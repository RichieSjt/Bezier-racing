using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject trackPoints;
    public List<List<Vector3>> bezierCurves;
    public GameObject theCar;
    public Vector3 carOffset;
    public PlayerType playerType;
    public string accelerationKey;
    public string brakeKey;
    private Vector3[] _vertices;
    private Vector3 _position;
    private Vector3 _targetPoint;
    private float _movementParam;
    private float _distance;
    private int _curveIdx;
    private Particle _particle;
    private float _maxSpeed;
    private float _currentSpeed;

    public enum PlayerType
    {
        Player, AI
    }

    private void OnEnable()
    {
        HealthSystem.playerDie += DestroyCar;        
    }

    private void OnDisable()
    {
        HealthSystem.playerDie -= DestroyCar; 
    }

    void Start()
    {
        _vertices = theCar.GetComponent<MeshFilter>().mesh.vertices;
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

        // Add the particle to the car
        theCar.AddComponent<Particle>();
        _particle = theCar.GetComponent<Particle>();
        _particle.radius = 1f;
        _particle.type = Particle.Type.Player;
        
        // Variables initialization
        _position = theCar.transform.position;
        _curveIdx = 0;
        _distance = 1000f;
        _maxSpeed = 0.003f;
        _currentSpeed = 0;
        playerType = PlayerType.Player;


        ParticleSystem.addCarParticle(theCar);
    }

    private void Update()
    {
        CheckCarAcceleration();

        // Move the particle with the car position
        _particle.sphere.transform.position = _position;
        _particle.position = _position;

        // Set the health bar position
        GetComponent<HealthSystem>().healthBar.setPosition(_position);

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
        
        _movementParam += _currentSpeed;

        // BEZIER
        List<Vector3> curve = bezierCurves[_curveIdx];

        // The target point is the last control point in the curve
        _targetPoint = curve[curve.Count-1];

        // Calculate the posticion using the bezier curve
        _position = Bezier.EvalBezier(curve, _movementParam);
        _distance = Math3D.Magnitude(_targetPoint - _position);

        _position += carOffset;

        // Direction
        Vector3 prev = Bezier.EvalBezier(curve, _movementParam-0.0005f);
        Vector3 dir = _position - prev;
        Vector3 du = dir.normalized;
        float angle = Mathf.Rad2Deg * Mathf.Atan(du.z/du.x);
        Debug.DrawLine(_position, dir, Color.red);

        // Set the transformation matrices to animate the car movement
        Matrix4x4 r = Transformations.RotateM(angle, Transformations.AXIS.AX_Y);
        Matrix4x4 t = Transformations.TranslateM(_position.x, _position.y, _position.z - 6.2f);
        
        // Apply the tranformation matrices to the vertices of the car
        theCar.GetComponent<MeshFilter>().mesh.vertices = Transformations.ApplyTransformation(_vertices, t * r);

        // theCar.GetComponent<Transform>().position = _position;
    }

    private void CheckCarAcceleration()
    {
        if (playerType == PlayerType.Player)
        {
            // Accelerate if the player press the acceleration key
            if(Input.GetKey(accelerationKey) && _currentSpeed < _maxSpeed)
                _currentSpeed += 0.00001f;
            
            // Brake if the player press brake key
            if(Input.GetKey(brakeKey) && _currentSpeed > 0)
                _currentSpeed -= 0.00002f;
            
            // Set to 0 to avoid the car going in reverse
            if (_currentSpeed <= 0)
                _currentSpeed = 0;
        }
        
        if (playerType == PlayerType.AI)
            _currentSpeed = _maxSpeed;
    }

    private void DestroyCar() {
        Destroy(theCar);
    }
}
