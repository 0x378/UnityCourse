using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private int level;
    private int maximum;

    public GameObject parentObject; // The object which this health bar belongs to
    public GameObject bar;          // Represents the current health
    public GameObject background;   // Represents the maximum possible health

    private bool isEnabled = false;

    public bool isAlive()
    {
        return (level > 0);
    }

    private void UpdateBar()
    {
        bar.transform.localScale = new Vector3(0.3f * level, 1, 1);
    }

    public void Heal()
    {
        level = maximum;
        UpdateBar();
    }

    public void Add(int amount)
    {
        level += amount;

        if (level > maximum)
        {
            level = maximum;
        }

        UpdateBar();
    }

    public void Subtract(int amount)
    {
        level -= amount;

        if (level < 0)
        {
            level = 0;
        }

        UpdateBar();
    }

    void Update() // Maintain position above parent:
    {
        if (isEnabled)
        {
            if (parentObject == null)
            {
                isEnabled = false;

                // Delete the health bar if the parent object is destroyed:
                Destroy(gameObject);
            }
            else
            {
                gameObject.transform.position = parentObject.transform.position;
            }
        }
    }

    public void Setup(GameObject callerObject, int maxHealth)
    {
        maximum = maxHealth;
        parentObject = callerObject;
        background.transform.localScale = new Vector3(0.3f * maximum, 1, 1);
        Heal();
        isEnabled = true;
    }
}
