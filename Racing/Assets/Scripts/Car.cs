using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject theCar;
    public Vector3 carOffset {get; set;}
    public PlayerType playerType {get; set;}
    public string accelerationKey {get; set;}
    public string brakeKey {get; set;}
    public Vector3 position {get; set;}
    public Vector3 previousPosition {get; set;}
    private Vector3[] _vertices;
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

        // Add the particle to the car
        theCar.AddComponent<Particle>();
        _particle = theCar.GetComponent<Particle>();
        _particle.radius = 1f;
        
        // Variables initialization
        _maxSpeed = 0.003f;
        _currentSpeed = 0;
        playerType = PlayerType.Player;

        ParticleSystem.addCarParticle(theCar);
    }

    private void LateUpdate()
    {
        CheckCarAcceleration();

        position += carOffset;
        previousPosition += carOffset;

        // Move the particle with the car position
        _particle.sphere.transform.position = position;
        _particle.position = position;

        // Set the health bar position
        GetComponent<HealthSystem>().healthBar.setPosition(position);

        // Direction
        Vector3 dir = position - previousPosition;
        Vector3 du = dir.normalized;
        float angle = Mathf.Rad2Deg * Mathf.Atan(du.z/du.x);
        Debug.DrawLine(position, dir, Color.red);


        // Set the transformation matrices to animate the car movement
        Matrix4x4 r = Transformations.RotateM(angle, Transformations.AXIS.AX_Y);
        Matrix4x4 t = Transformations.TranslateM(position.x, position.y, position.z);
        
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
