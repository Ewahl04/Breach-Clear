using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDotBehavior : MonoBehaviour
{
    public GameObject redDot;

    void Update()
    {
        if (!GameManager.isScoped && !GameManager.reloading && !GameManager.isSprinting)
        {
            redDot.SetActive(true);
        }

        else
        {
            redDot.SetActive(false);
        }
    }
}
