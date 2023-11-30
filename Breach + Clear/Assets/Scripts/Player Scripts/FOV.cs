using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    public Camera targetCamera; // Reference to the camera you want to adjust
    public float desiredFOV = 60.0f; // Initial FOV value

    private void Start()
    {
        if (targetCamera == null)
        {
            // If the camera reference is not set in the Inspector, use the main camera
            targetCamera = Camera.main;
        }
        // Set the initial FOV value
        targetCamera.fieldOfView = desiredFOV;
    }

    private void Update()
    {
        // Update the camera's FOV to the desired value each frame
        targetCamera.fieldOfView = desiredFOV;
    }
}
