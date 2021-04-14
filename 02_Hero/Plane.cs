using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane1 : MonoBehaviour
{
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
    }

    public int getHealth()
    {
        return health;
    }

    public void damageBy(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            health = 0;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
