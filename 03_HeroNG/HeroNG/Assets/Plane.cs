using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    private float velocity;
    private string type;

    private float angleDegrees;
    public float angularVelocity = 0;
    private float angularAcceleration = 0;
    private float maximumAngularAcceleration;

    private Vector3 position;
    public Vector3 scene;

    private int currentHealth;
    private int maxHealth;

    // Initialized only once, by the StatusBar itself:
    public static StatusBar systemStatus;

    // Current waypoint:
    public int waypointIndex = 0;

    void Start()
    {
        waypointIndex = Random.Range(0, 6);

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
            velocity = 20f;
            maximumAngularAcceleration = 190f;
        }
        else if (type == "Plane2")
        {
            maxHealth = 60;
            velocity = 29f;
            maximumAngularAcceleration = 280f;
        }
        else // "Plane3" or other:
        {
            maxHealth = 100;
            velocity = 15f;
            maximumAngularAcceleration = 40f;
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

    void getNextWaypoint()
    {
        if (systemStatus.sequentialWaypoints) // Sequentially:
        {
            waypointIndex++;
        }
        else // At random:
        {
            waypointIndex += Random.Range(1, 6);
        }

        waypointIndex %= 6;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name.Length > 3)
        {
            if (collider.gameObject.name.Substring(0, 4) == "Wayp")
            {
                Waypoint collisionWaypoint = collider.gameObject.GetComponent<Waypoint>();

                if (waypointIndex == collisionWaypoint.myIndex)
                {
                    getNextWaypoint();
                }
            }
        }
    }

    // Rotate the plane's heading to face the current waypoint:
    void UpdateDirection()
    {
        angleDegrees = transform.localEulerAngles.z;

        Vector3 waypointHeading = systemStatus.waypointPositions[waypointIndex] - position;
        float targetAngle = Mathf.Atan2(-waypointHeading.x, waypointHeading.y) * Mathf.Rad2Deg;
        float waypointAngleDifference = Waypoint.angleWithin180(targetAngle - angleDegrees);

        float maximumAngularVelocity = Mathf.Sqrt(Mathf.Abs(maximumAngularAcceleration * waypointAngleDifference));

        if (waypointAngleDifference > 0)
        {
            if (angularVelocity < maximumAngularVelocity)
            {
                angularAcceleration = maximumAngularAcceleration;
            }
            else
            {
                angularAcceleration = -maximumAngularAcceleration;
            }
        }
        else
        {
            if (angularVelocity > -maximumAngularVelocity)
            {
                angularAcceleration = -maximumAngularAcceleration;
            }
            else
            {
                angularAcceleration = maximumAngularAcceleration;
            }
        }

        angularVelocity += angularAcceleration * Time.deltaTime;
        transform.Rotate(0, 0, angularVelocity * Time.deltaTime);

        //transform.Rotate(0f, 0f, waypointAngleDifference / 360f);
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
        scene.x *= 0.875f;
        scene.y *= 0.875f;

        UpdateDirection();
        UpdatePosition();
    }
}
