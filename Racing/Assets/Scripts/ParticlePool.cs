using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : Particle
{
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
        previousPosition = position;
        acceleration = Vector3.zero;
        drag.y = 1;
    }

    private void Update()
    {
        if (Time.frameCount > 20)
        {
            forces.y += -gravity * mass * deltaTime;
            if (position.y > previousPosition.y) drag = -forces * dragUp;
            else if (position.y < previousPosition.y) drag = -forces * dragDown;
            else drag = Vector3.zero;
            forces += drag;

            base.Verlet(deltaTime);
        }
    }
}