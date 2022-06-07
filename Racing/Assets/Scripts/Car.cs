using System;
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
    public int speed {get; set;}
    private Vector3 _position;
    private Vector3 _previousPosition;
    private Vector3[] _vertices;
    private Particle _particle;
    public int pointIdx {get; set;}
    private Boolean _moving;
    public static event Action<GameObject> setCar;

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
        _particle.type = Particle.Type.Player;
        _particle.isInvincible = false;
        
        // Variables initialization
        _moving = false;

        setCar?.Invoke(theCar);
        // ParticleSystem.addCarParticle(theCar);
        
    }

    private void LateUpdate()
    {
        CheckCarAcceleration();

        if(_moving) {
            if (pointIdx >= TrackSystem.bezierPath.Count-speed)
                pointIdx = 0;
            else
                pointIdx+=speed;
        }

        _position = TrackSystem.getPosition(pointIdx);
        if(pointIdx == 0) 
            _previousPosition = TrackSystem.getPosition(TrackSystem.bezierPath.Count-1);
        else
            _previousPosition = TrackSystem.getPosition(pointIdx-1);

        // Set the car offset
        _position += carOffset;
        _previousPosition += carOffset;

        // Move the particle with the car position
        _particle.sphere.transform.position = _position;
        _particle.position = _position;

        // Set the health bar position
        GetComponent<HealthSystem>().healthBar.setPosition(_position);

        // Direction
        Vector3 dir = _position - _previousPosition;
        Vector3 du = dir.normalized;
        float angle = Mathf.Rad2Deg * Mathf.Atan(du.z/du.x);
        // Debug.DrawLine(_position, dir, Color.red);

        // Set the transformation matrices to animate the car movement
        Matrix4x4 r = Transformations.RotateM(angle, Transformations.AXIS.AX_Y);
        Matrix4x4 t = Transformations.TranslateM(_position.x, _position.y, _position.z - 6.2f);
        
        // Apply the tranformation matrices to the vertices of the car
        theCar.GetComponent<MeshFilter>().mesh.vertices = Transformations.ApplyTransformation(_vertices, t * r);

        // theCar.GetComponent<Transform>().position = _position;
    }

    private void CheckCarAcceleration()
    {   
        if (playerType == PlayerType.AI)
            _moving = true;


        if (playerType == PlayerType.Player)
        {
            // Accelerate if the player press the acceleration key
            if(Input.GetKey(accelerationKey))
                _moving = true;
            
            // Brake if the player press brake key
            if(Input.GetKey(brakeKey))
                _moving = false;
        
        }
    }
    
    private void DestroyCar() {
        theCar.GetComponent<Particle>().sphere.SetActive(false);
        theCar.SetActive(false);
    }
}
