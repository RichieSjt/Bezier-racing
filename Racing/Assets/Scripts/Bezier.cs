/*
    Eduardo Herrera Juárez A01651153
    Pedro Rangel Palacios A01650939
    Ricardo Juárez Tepos A01650943
    Manuel Alejandro Hernández López A01650390
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    List<Vector3> controlPoints;
    
    float movement;
    bool finished;
    float speed;
    GameObject greenSphere;

    // Start is called before the first frame update
    void Start()
    {
        movement = 0;
        finished = false;
        speed = 0.001f;
        greenSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        greenSphere.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        greenSphere.GetComponent<Transform>().localScale = new Vector3(0.3f, 0.3f, 0.3f);
        
        controlPoints = new List<Vector3>();
        controlPoints.Add(new Vector3(0, 0, 0));
        controlPoints.Add(new Vector3(1, 5, 0));
        controlPoints.Add(new Vector3(5, 10, 0));
        controlPoints.Add(new Vector3(8, 5, 0));
        controlPoints.Add(new Vector3(10, 0, 0));

        // Showing control points as red spheres
        for(int i = 0; i < controlPoints.Count; i++) {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            sphere.GetComponent<Transform>().position = controlPoints[i];
            sphere.GetComponent<Transform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        
        int bezierIterations = 20;

        // Calculating bezier points
        for (int i = 0; i < bezierIterations; i++) {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Vector3 position = EvalBezier(controlPoints, (1.0f / bezierIterations) * i);
            sphere.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
            sphere.GetComponent<Transform>().position = position;
            sphere.GetComponent<Transform>().localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }

        Vector3 test1 = EvalBezier(controlPoints, 0);
        Debug.Log(test1);

        Vector3 test2 = EvalBezier(controlPoints, 1.0f);
        Debug.Log(test2);
    }

    // Update is called once per frame
    void Update()
    {
        if (movement >= 1.0f) finished = true;
        if (movement <= 0) finished = false;

        if (finished) {
            movement -= speed;
        }
        else
        {
            movement += speed;
        }

        greenSphere.GetComponent<Transform>().position = EvalBezier(controlPoints, movement);
    }

    Vector3 EvalBezier(List<Vector3> P, float t) 
    {
        int n = P.Count;
        Vector3 p = Vector3.zero;

        for(int i = 0; i < n; i++)
        {
            p += Combination(n-1, i) * Mathf.Pow(1.0f - t, n-1 - i) * Mathf.Pow(t, i) * P[i];
        }
        
        return p;
    }

    int Factorial(int n)
    {
        if (n == 0) return 1;
        else return n * Factorial(n - 1);
    }

    float Combination(int n, int i)
    {
        return Factorial(n) / (Factorial(i) * Factorial(n - i));
    }
}
