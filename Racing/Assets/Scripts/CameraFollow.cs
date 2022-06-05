using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetCar;
    public float smoothSpeed = 10f;
	public float sensitivity = 20f;
    public Vector3 offset;
    private Vector2 rotation = Vector2.zero;

    void LateUpdate()
    {
        // Following the car smoothly
        Vector3 desiredPos = targetCar.GetComponent<Particle>().sphere.transform.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.position = smoothPos;
    }

    private void Update()
    {
        // Rotating camera using mouse
        rotation.y += Input.GetAxis("Mouse X");
		rotation.x += -Input.GetAxis("Mouse Y");
		transform.eulerAngles = (Vector2)rotation * sensitivity;

        // Move the camera with the WASD keys
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(x, 0.0f, z));
    }
}