using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public string type;

    // Initialized for each plane, within Start():
    public HealthBar healthBar;

    // Initialized only once, by the StatusBar itself:
    public static StatusBar systemStatus;

    void Start()
    {
        healthBar = Instantiate(healthBar, transform.position, Quaternion.identity) as HealthBar;

        // Acquire the plane type from the object name:
        if (gameObject.name.Length > 5)
        {
            type = gameObject.name.Substring(0, 6);
        }
        else
        {
            type = gameObject.name;
        }

        // Determine hit points based on plane type:
        if (type == "Plane1")
        {
            healthBar.Setup(gameObject, 40);
        }
        else if (type == "Plane2")
        {
            healthBar.Setup(gameObject, 60);
        }
        else // "Plane3" or other:
        {
            healthBar.Setup(gameObject, 100);
        }
    }

    public void damageBy(int amount)
    {
        if (healthBar.isAlive())
        {
            healthBar.Subtract(amount);

            // If health is depleted:
            if (!healthBar.isAlive())
            {
                systemStatus.numberOfPlanes--;
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {

    }
}
