using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float radius;
    public Vector3 position;
    public Color color;
    public GameObject sphere;     // game object for the particle

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
}