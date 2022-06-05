using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Particle attached to the car to detect collisions with other particles
public class Particle : MonoBehaviour
{
    public float mass;
    public float gravity;
    public float restitutionCoefficient;   // Restitution Coefficient (elastic=1, inelastic = 0)
    public float deltaTime;
    public float damage = 5f;
    public Vector3 forces;
    public Vector3 acceleration;
    public float dragUp;
    public float dragDown;
    public Vector3 drag;
    public Vector3 previousPosition;
    public Vector3 tempPosition;
    public float radius;
    public Vector3 position;
    public Color color;
    public GameObject sphere;     // game object for the particle
    public Type type;

    public enum Type
    {
        PoolParticle, Player
    }
    private void Start()
    {
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        sphere.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
        float cr = Random.Range(0.0f, 1.0f);
        float cg = Random.Range(0.0f, 1.0f);
        float cb = Random.Range(0.0f, 1.0f);
        color = new Color(cr, cg, cb);
        sphere.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
    }

    public bool CheckCollision(Particle other)
    {
        float dx = other.position.x - position.x;
        float dy = other.position.y - position.y;
        float dz = other.position.z - position.z;

        float sumR = other.radius + radius;
        sumR *= sumR;

        return sumR > (dx * dx + dy * dy + dz * dz);
    }

    public void CollisionPhysics(Particle other)
    {
        forces = -forces * restitutionCoefficient;
        Vector3 diff = previousPosition - position;
        previousPosition = position - diff;

        other.forces = -other.forces * other.restitutionCoefficient;
        Vector3 otherDiff = other.previousPosition - other.position;
        other.previousPosition = other.position - otherDiff;
        

        // particle.forces.y = -particle.forces.y * particle.restitutionCoefficient;
        //     float diff = particle.position.y - particle.previousPosition.y;
        //     particle.position.y = topLimit;
        //     particle.previousPosition.y = topLimit + diff;
    }

    public void Verlet(float dt)
    {
        tempPosition = position;                                                 // save p temporarily
        acceleration = forces / mass;                                            // a = F/m
        position = 2 * position - previousPosition + (acceleration * dt * dt);   // Verlet
        previousPosition = tempPosition;                                         // restore previous position
        sphere.transform.position = position;
    }
}