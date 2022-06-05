using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        // The sprite is always facing to the camera
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}