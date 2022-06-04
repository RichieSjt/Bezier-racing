using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetCar;
    public float smoothSpeed = 10f;
    public Vector3 offset;

    void LateUpdate()
    {
        Vector3 desiredPos = targetCar.GetComponent<Particle>().sphere.transform.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.position = smoothPos;

        transform.position = smoothPos;

        transform.LookAt(targetCar.GetComponent<Particle>().sphere.transform.position);
    }
}