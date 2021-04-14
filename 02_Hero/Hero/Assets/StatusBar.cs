using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBar : MonoBehaviour
{
    public GameObject plane1, plane2, plane3;

    public int numberOfPlanes;
    public int numberOfProjectiles;

    public static Vector3 getRandomPosition()
    {
        Vector3 scene = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        // Keep plane spawns away from edges:
        scene.x *= 0.9f;
        scene.y *= 0.9f;

        return new Vector3(Random.Range(-scene.x, scene.x), Random.Range(-scene.y, scene.y), 0);
    }

    public static Quaternion getRandomDirection()
    {
        return Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }

    void Start()
    {
        Plane.systemStatus = this;

        numberOfPlanes = 0;
        numberOfProjectiles = 0;
    }

    private void PlaneSpawner()
    {
        while (numberOfPlanes < 10)
        {
            float planeSelector = Random.Range(0f, 100f);

            if (planeSelector < 67) // Values of 0 to 67 for a 67% chance of Plane 1:
            {
                Instantiate(plane1, getRandomPosition(), getRandomDirection());
                numberOfPlanes++;
            }
            else if (planeSelector < 88) // Values of 67 to 88 for a 21% chance of Plane 2:
            {
                Instantiate(plane2, getRandomPosition(), getRandomDirection());
                numberOfPlanes++;
            }
            else // Values of 88 to 100, for a 12% chance of Plane 3:
            {
                Instantiate(plane3, getRandomPosition(), getRandomDirection());
                numberOfPlanes++;
            }
        }
    }

    void Update()
    {
        PlaneSpawner();
    }
}