using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimZoom : MonoBehaviour
{
    public Camera mainCamera;
    public float zoomFOV = 40; // The amount to zoom in when right-clicking
    public float startFOV = 50;
    public float zoomSpeed = 200f;
    private float currentFOV;
    public Animator animator;
    private bool isScoped = false;


    void Start()
    {
        Time.fixedDeltaTime = 0.005f;
        mainCamera.fieldOfView = startFOV;
    }

    void FixedUpdate()
    {
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, zoomFOV, startFOV);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            mainCamera.fieldOfView += zoomSpeed * Time.deltaTime;
            isScoped = false;
            animator.SetBool("Aiming", isScoped);
            animator.SetBool("Sprinting", true);
        }

        else
        {

            animator.SetBool("Sprinting", false);
            
            if (Input.GetMouseButton(1) && !Input.GetKey(KeyCode.LeftShift))
            {
                mainCamera.fieldOfView -= zoomSpeed * Time.deltaTime;
                isScoped = true;
                animator.SetBool("Aiming", isScoped);
            }

            else
            {
                mainCamera.fieldOfView += zoomSpeed * Time.deltaTime;
                isScoped = false;
                animator.SetBool("Aiming", isScoped);
            }
        }
    }    
}