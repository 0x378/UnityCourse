using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScript : MonoBehaviour
{
    float startTime;
    float elapsedTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        elapsedTime = Time.time - startTime;

        // End the program:
        if (elapsedTime > 2.5f)
        {
            Application.Quit();
        }
    }
}
