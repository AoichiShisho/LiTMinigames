using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickLister : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string[] joysticks = Input.GetJoystickNames();

        for (int i = 0; i < joysticks.Length; i++)
        {
            if (!string.IsNullOrEmpty(joysticks[i]))
            {
                Debug.Log("Joystick " + (i + 1) + ": " + joysticks[i] + " (JoyNum: " + "Joystick" + (i + 1) + ")");
            }
        }
    }
}
