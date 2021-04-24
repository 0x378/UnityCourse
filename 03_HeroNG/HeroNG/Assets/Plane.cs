using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public float angleDegrees;
    public float distanceFromLeft;
    public float distanceFromRight;
    public float distanceFromTop;
    public float distanceFromBottom;

    public float velocity = 21f;
    public string type;

    public float angularVelocity = 0;
    private float angularAcceleration = 0;

    private Vector3 position;
    public Vector3 scene;

    private int currentHealth = 40;
    private int maxHealth = 40;

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

        currentHealth = maxHealth;
        angularVelocity = 0;
        angularAcceleration = 0;
    }

    public void damageBy(int amount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= amount;

            // Reduce the alpha channel of the plane color:
            Renderer renderer = gameObject.GetComponent<Renderer>();
            Color currentColor = renderer.material.color;
            currentColor.a *= Mathf.Pow(0.8f, amount * 4f / maxHealth);
            renderer.material.SetColor("_Color", currentColor);

            // If health is depleted:
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                systemStatus.numberOfPlanes--;
                systemStatus.planesDestroyed++;
                Destroy(gameObject);
            }
        }
    }

    // Chooses a random direction for the airplane's heading, but carefully in
    // order to steer away from walls. An exponential function is used for each
    // case, such that the angular acceleration away from the nearest wall is
    // stronger when near the wall, and decays with distance as there is no need
    // to turn around when far away.
    void UpdateDirection()
    {
        // Makes it more likely to slow down the current angular velocity:
        float rangeOffset = -1.5f * angularVelocity;


        angleDegrees = transform.localEulerAngles.z;

        distanceFromLeft = position.x + scene.x;  // position.x - (-scene.x)
        distanceFromRight = scene.x - position.x;
        distanceFromTop = scene.y - position.y;
        distanceFromBottom = position.y + scene.y;  // position.y - (-scene.y)

        // Turn around near walls:
        if (angleDegrees < 180) // Heading left
        {
            if (angleDegrees < 90) // Heading up-left
            {
                if (distanceFromLeft < distanceFromTop)
                { // Turn around clockwise
                    rangeOffset -= 512 * Mathf.Exp(-6f * distanceFromLeft / scene.x);
                }
                else
                { // Turn around counter-clockwise
                    rangeOffset += 512 * Mathf.Exp(-6f * distanceFromTop / scene.y);
                }
            }
            else // Heading down-left
            {
                if (distanceFromLeft < distanceFromBottom)
                { // Turn around counter-clockwise
                    rangeOffset += 512 * Mathf.Exp(-6f * distanceFromLeft / scene.x);
                }
                else
                { // Turn around clockwise
                    rangeOffset -= 512 * Mathf.Exp(-6f * distanceFromBottom / scene.y);
                }
            }
        }
        else // Heading right
        {
            if (angleDegrees > 270) // Heading up-right
            {
                if (distanceFromRight < distanceFromTop)
                { // Turn around counter-clockwise
                    rangeOffset += 512 * Mathf.Exp(-6f * distanceFromRight / scene.x);
                }
                else
                { // Turn around clockwise
                    rangeOffset -= 512 * Mathf.Exp(-6f * distanceFromTop / scene.y);
                }
            }
            else // Heading down-right
            {
                if (distanceFromRight < distanceFromBottom)
                { // Turn around clockwise
                    rangeOffset -= 512 * Mathf.Exp(-6f * distanceFromRight / scene.x);
                }
                else
                { // Turn around counter-clockwise
                    rangeOffset += 512 * Mathf.Exp(-6f * distanceFromBottom / scene.y);
                }
            }
        }

        angularAcceleration = Random.Range(rangeOffset - 512, rangeOffset + 512);

        // Limit the angular velocity to 90 degrees per second when applying its acceleration:
        angularVelocity += angularAcceleration * Time.deltaTime;

        if (angularVelocity > 90)
        {
            angularVelocity = 90;
        }
        else if (angularVelocity < -90)
        {
            angularVelocity = -90;
        }

        transform.Rotate(0, 0, angularVelocity * Time.deltaTime);
    }

    void UpdatePosition()
    {
        position = transform.position;

        float deltaV = velocity * Time.deltaTime;
        float angleRadians = transform.localEulerAngles.z * Mathf.Deg2Rad;
        position.x -= deltaV * Mathf.Sin(angleRadians);
        position.y += deltaV * Mathf.Cos(angleRadians);

        // Typically, here I would check for walls to keep the plane within bounds, but
        // there is no longer a need because of the process I used for choosing a heading.

        transform.position = position;
    }

    void Update()
    {
        scene = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        scene.x *= 0.9f;
        scene.y *= 0.9f;

        if (systemStatus.planeMovementEnabled)
        {
            UpdateDirection();
            UpdatePosition();
        }
    }
}
