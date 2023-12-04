using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;
    private float currentSensX;
    private float currentSensY;
    public float zoomSensitivityReduction;
    public float FOV = 50;

    public Transform orientation;

    float xRotation;
    float yRotation;
    
    

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentSensX = sensX;
        currentSensY = sensY;
    }

    private void Update()
    {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate cam and player
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);

        if (GameManager.isScoped)
        {
            sensX = currentSensX / zoomSensitivityReduction;
            sensY = currentSensY / zoomSensitivityReduction;
        }

        else
        {
            sensX = currentSensX;
            sensY = currentSensY;
        }
    }



    public IEnumerator RecoilFire(float recoilX, float recoilY, float aimRecoilX, float aimRecoilY, float snappiness, float returnSpeed)
    {
        float elapsed = 0f;

        float startX = xRotation;
        float startY = yRotation;

        while (elapsed > returnSpeed + snappiness)
        {
            if (Input.GetMouseButton(1) && !Input.GetKey(KeyCode.LeftShift))
            {

            }

            else
            {
                float targetX = startX + recoilX;
                float targetY = startY + recoilY;

                if (xRotation != targetX && yRotation != targetY)
                {
                    float timeElapsed = 0f;

                    if (timeElapsed < snappiness)
                    {
                        xRotation = Mathf.Lerp(startX, targetX, timeElapsed / snappiness);
                        yRotation = Mathf.Lerp(startY, targetY, timeElapsed / snappiness);
                        timeElapsed += Time.deltaTime;
                    }

                    else
                    {
                        xRotation = targetX;
                        yRotation = targetY;
                    }
                }

                else
                {
                    float coiledX = targetX;
                    float coiledY = targetY;
                    float timeElapsed = 0f;

                    if (timeElapsed < returnSpeed)
                    {
                        xRotation = Mathf.Lerp(coiledX, startX, timeElapsed / returnSpeed);
                        yRotation = Mathf.Lerp(coiledY, startY, timeElapsed / returnSpeed);
                        timeElapsed += Time.deltaTime;
                    }

                    else
                    {
                        xRotation = startX;
                        yRotation = startY;
                    }
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
