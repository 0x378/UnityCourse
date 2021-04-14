using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public string type;

    // Initialized only once, by the StatusBar itself:
    public static StatusBar systemStatus;

    void Start()
    {
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
            maxHealth = 40;
        }
        else if (type == "Plane2")
        {
            maxHealth = 60;
        }
        else // "Plane3" or other:
        {
            maxHealth = 100;
        }

        health = maxHealth;
    }

    // Returns a value between 0 and 1, where 1 represents full health:
    public float getHealth()
    {
        return health / maxHealth;
    }

    public void damageBy(int amount)
    {
        if (health > 0)
        {
            health -= amount;

            // If health is depleted:
            if (health <= 0)
            {
                health = 0;
                systemStatus.numberOfPlanes--;
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {

    }
}
