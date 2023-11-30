using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDotBehavior : MonoBehaviour
{
    public GameObject redDot;

    void Update()
    {
        if (Input.GetMouseButton(1) && !Input.GetKey(KeyCode.LeftShift))  // Check for right mouse button down
        {
            redDot.SetActive(false);
        }

        else //check for right mouse button up
        {
            redDot.SetActive(true);
        }
    }
}
