using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystem : MonoBehaviour
{
    public int N;
    public float poolHeight;
    public float poolWidth;
    public float poolLength;
    public Vector3 poolOrigin;
    public float deltaTime;
    List<GameObject> particles;
    float midLength;
    float midWidth;

    // Start is called before the first frame update
    void Start()
    {
        midLength = poolLength / 2.0f;
        midWidth = poolWidth / 2.0f;
        particles = new List<GameObject>();
        
        for(int i = 0; i < N; i++)
        {
            GameObject go = new GameObject(); 
            go.AddComponent<ParticlePool>();
            ParticlePool p = go.GetComponent<ParticlePool>();

            float x = Random.Range(poolOrigin.x - midLength * 0.95f, poolOrigin.x + midLength * 0.95f);
            float y = Random.Range(poolOrigin.y + poolHeight * 0.8f, poolOrigin.y + poolHeight * 0.95f);
            float z = Random.Range(poolOrigin.z - midWidth * 0.95f, poolOrigin.z + midWidth * 0.95f);
            p.position = new Vector3(x, y, z);
            p.forces = Vector3.zero;
            p.forces.x = Random.Range(-5.0f, 5.0f);
            p.forces.z = Random.Range(-5.0f, 5.0f);
            p.radius = Random.Range(0.2f, 0.5f);
            p.gravity = 9.81f;
            p.mass = p.radius * 2.0f;
            p.restitutionCoefficient = 0.1f;
            p.dragUp = 0.000001f;
            p.dragDown = 0.1f;
            p.deltaTime = deltaTime;
            particles.Add(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject p1 in particles)
        {
            ParticlePool particle1 = p1.GetComponent<ParticlePool>();
            CheckPoolWalls(particle1);
            
            bool p1Collision = false;
            Color originalColor = particle1.color;

            foreach (GameObject p2 in particles)
            {
                ParticlePool particle2 = p2.GetComponent<ParticlePool>();

                if (p1.GetInstanceID() != p2.GetInstanceID())
                {
                    bool collision = particle1.CheckCollision(particle2);

                    if (collision)
                    {
                        particle1.sphere.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
                        particle2.sphere.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
                        p1Collision = true;
                    }
                }
            }

            if (!p1Collision)
            {
                particle1.sphere.GetComponent<MeshRenderer>().material.SetColor("_Color", originalColor);
            }
        }
    }

    public void CheckPoolWalls(ParticlePool particle)
    {
        float topLimit = (poolOrigin.y + poolHeight) - particle.radius;
        float bottomLimit = poolOrigin.y + particle.radius;
        float rightLimit = (poolOrigin.x + midLength) - particle.radius;
        float leftLimit = (poolOrigin.x - midLength) + particle.radius;
        float backLimit = (poolOrigin.z + midWidth) - particle.radius;
        float frontLimit = (poolOrigin.z - midWidth) + particle.radius;

        // Bottom
        if(particle.position.y < bottomLimit)
        {
            particle.forces.y = -particle.forces.y * particle.restitutionCoefficient;
            float diff = particle.previousPosition.y - particle.position.y;
            particle.position.y = particle.radius;
            particle.previousPosition.y = particle.radius - diff;
        }

        // Top
        if(particle.position.y > topLimit)
        {
            particle.forces.y = -particle.forces.y * particle.restitutionCoefficient;
            float diff = particle.position.y - particle.previousPosition.y;
            particle.position.y = topLimit;
            particle.previousPosition.y = topLimit + diff;
        }

        // Right side
        if(particle.position.x > rightLimit)
        {
            particle.forces.x = -particle.forces.x * particle.restitutionCoefficient;
            float diff = particle.position.x - particle.previousPosition.x;
            particle.position.x = rightLimit;
            particle.previousPosition.x = rightLimit + diff;
        }

        // Left Side
        if(particle.position.x < leftLimit)
        {
            particle.forces.x = -particle.forces.x * particle.restitutionCoefficient;
            float diff = particle.previousPosition.x - particle.position.x;
            particle.position.x = leftLimit;
            particle.previousPosition.x = leftLimit - diff;
        }

        // Back side
        if(particle.position.z > backLimit)
        {
            particle.forces.z = -particle.forces.z * particle.restitutionCoefficient;
            float diff = particle.position.z - particle.previousPosition.z;
            particle.position.z = backLimit;
            particle.previousPosition.z = backLimit + diff;
        }

        // Front Side
        if(particle.position.z < frontLimit)
        {
            particle.forces.z = -particle.forces.z * particle.restitutionCoefficient;
            float diff = particle.previousPosition.z - particle.position.z;
            particle.position.z = frontLimit;
            particle.previousPosition.z = frontLimit - diff;
        }
    }
}
