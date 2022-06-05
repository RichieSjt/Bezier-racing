using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform bar;
    public Vector3 offset;

    public void setSize(float sizeNormalized) {
        // Scale the bar depending on the health (values beetween 0-1)
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }

    public void setPosition(Vector3 position)
    {
        // Move the health bar to the offset position
        transform.position = position + offset;
    }

    private void LateUpdate()
    {
        // The health bar is always facing to the camera
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }


}