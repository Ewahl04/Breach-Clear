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


    void Start()
    {
        Time.fixedDeltaTime = 0.005f;
        mainCamera.fieldOfView = startFOV;
    }

    void FixedUpdate()
    {
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, zoomFOV, startFOV);

        if (GameManager.isSprinting)
        {
            mainCamera.fieldOfView += zoomSpeed * Time.deltaTime;
            GameManager.isScoped = false;
            animator.SetBool("Aiming", GameManager.isScoped);
            animator.SetBool("Sprinting", true);
        }

        else
        {

            animator.SetBool("Sprinting", false);
            
            if (Input.GetMouseButton(1) && !GameManager.isSprinting && !GameManager.reloading)
            {
                mainCamera.fieldOfView -= zoomSpeed * Time.deltaTime;
                GameManager.isScoped = true;
                animator.SetBool("Aiming", GameManager.isScoped);
            }

            else
            {
                mainCamera.fieldOfView += zoomSpeed * Time.deltaTime;
                GameManager.isScoped = false;
                animator.SetBool("Aiming", GameManager.isScoped);
            }
        }
    }    
}