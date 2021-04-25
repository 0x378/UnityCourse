using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private Vector3 currentPositionRatio;
    private Vector3 initialPositionRatio;
    private Vector3 currentPosition;
    private Vector3 scene;

    private int currentHealth;
    private int maxHealth;

    public float velocity = 10f;
    private float angleDegrees;
    private float angularVelocity = 0;
    private float angularAcceleration = 0;

    // Initialized only once, by the StatusBar itself:
    public static StatusBar systemStatus;

    // This waypoint's position array index within the Status Bar:
    public int myIndex;

    // Start is called before the first frame update
    void Start()
    {
        scene = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        currentPosition = transform.position;
        initialPositionRatio = new Vector3(currentPosition.x / scene.x, currentPosition.y / scene.y);
        currentPositionRatio = initialPositionRatio;

        maxHealth = 40;
        currentHealth = maxHealth;
    }

    private void updateOpacity()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        Color currentColor = renderer.material.color;

        if (systemStatus.showWaypoints)
        {
            currentColor.a = currentHealth / (float)maxHealth;
        }
        else
        {
            currentColor.a = 0;
        }

        renderer.material.SetColor("_Color", currentColor);
    }

    public void damageBy(int amount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= amount;

            if (currentHealth < 0)
            {
                currentHealth = 0; // To avoid setting a negative alpha channel
            }

            // Reduce the alpha channel of the waypoint color:
            updateOpacity();
        }
    }

    Vector3 positionToRatio(Vector3 inputPosition)
    {
        return new Vector3(inputPosition.x / scene.x, inputPosition.y / scene.y, 1);
    }

    Vector3 ratioToPosition(Vector3 inputRatio)
    {
        return new Vector3(inputRatio.x * scene.x, inputRatio.y * scene.y, transform.position.z);
    }

    void UpdateDirection()
    {
        // Makes it more likely to slow down the current angular velocity:
        float rangeOffset = -1.5f * angularVelocity;


        angleDegrees = transform.localEulerAngles.z;

        float distanceFromLeft = currentPosition.x + scene.x;  // position.x - (-scene.x)
        float distanceFromRight = scene.x - currentPosition.x;
        float distanceFromTop = scene.y - currentPosition.y;
        float distanceFromBottom = currentPosition.y + scene.y;  // position.y - (-scene.y)

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

        angularAcceleration = Random.Range(rangeOffset - 512f, rangeOffset + 512f);

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

    // Offset the input value by plus or minus 15 units:
    float randomOffset(float inputValue)
    {
        return Random.Range(inputValue - 15f, inputValue + 15f);
    }

    // Update is called once per frame
    void Update()
    {
        scene = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        if (currentHealth > 0)
        {
            currentPosition = ratioToPosition(currentPositionRatio);
        }
        else // Relocate the waypoint and restore its health:
        {
            Vector3 initialPosition = ratioToPosition(initialPositionRatio);

            float randomX = randomOffset(initialPosition.x);
            float randomY = randomOffset(initialPosition.y);

            currentPosition = new Vector3(randomX, randomY, transform.position.z);

            currentHealth = maxHealth;
        }

        updateOpacity();
        transform.position = currentPosition;
        systemStatus.waypointPositions[myIndex] = currentPosition;
        currentPositionRatio = positionToRatio(currentPosition);
    }
}
