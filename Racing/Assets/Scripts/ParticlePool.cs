using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : Particle
{
    public float poolHeight;
    public float poolWidth;
    public float mass;
    public float gravity;
    public float restitutionCoefficient;   // Restitution Coefficient (elastic=1, inelastic = 0)
    public float deltaTime;
    public Vector3 forces;
    public Vector3 acceleration;
    public float dragUp;
    public float dragDown;

    public Vector3 drag;
    public Vector3 previousPosition;
    Vector3 tempPosition;

    // Start is called before the first frame update
    void Start()
    {
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        sphere.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
        float cr = Random.Range(0.0f, 1.0f);
        float cg = Random.Range(0.0f, 1.0f);
        float cb = Random.Range(0.0f, 1.0f);
        color = new Color(cr, cg, cb);
        sphere.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        previousPosition = position;
        acceleration = Vector3.zero;
        drag.y = 1;
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount > 20)
        {
            forces.y += -gravity * mass * deltaTime;
            if (position.y > previousPosition.y) drag = -forces * dragUp;
            else if (position.y < previousPosition.y) drag = -forces * dragDown;
            else drag = Vector3.zero;
            forces += drag;

            Verlet(deltaTime);
        }
    }
    
    void Verlet(float dt)
    {
        tempPosition = position;                           // save p temporarily
        acceleration = forces / mass;                  // a = F/m
        position = 2 * position - previousPosition + (acceleration * dt * dt);   // Verlet
        previousPosition = tempPosition;                        // restore previous position
        sphere.transform.position = position;
    }

    
}