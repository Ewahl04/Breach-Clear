using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Variables")]
    static public bool GameIsPaused = false;
    static public bool reloading = false;
    static public bool isScoped = false;
    static public bool isSprinting = false;

    [Header("Keybinds")]
    static public KeyCode sprintKey = KeyCode.LeftShift;
    static public KeyCode crouchKey = KeyCode.LeftControl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
