using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float acceleration = 150f;
    public float velocity = 20f;
    private Vector3 position;
    private Vector3 scene;

    // Initialized only once, by the StatusBar itself:
    public static StatusBar systemStatus;

    private bool isEnabled = true;

    // Prevents duplicate counts upon deletion:
    private void disableAndRemove()
    {
        if (isEnabled)
        {
            isEnabled = false;
            systemStatus.numberOfProjectiles--;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isEnabled && collider.gameObject.name.Length > 4)
        {
            string type = collider.gameObject.name.Substring(0, 5);

            if (type == "Plane")
            {
                Plane enemy = collider.gameObject.GetComponent<Plane>();
                enemy.damageBy(80);
                disableAndRemove();
            }

            if (systemStatus.showWaypoints && type == "Waypo")
            {
                Waypoint waypoint = collider.gameObject.GetComponent<Waypoint>();
                waypoint.damageBy(80);
                disableAndRemove();
            }
        }
    }

    void Update()
    {
        scene = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        position = transform.position;

        velocity += acceleration * Time.deltaTime;
        float deltaV = velocity * Time.deltaTime;
        float angleRadians = transform.localEulerAngles.z * Mathf.Deg2Rad;
        position.x -= deltaV * Mathf.Sin(angleRadians);
        position.y += deltaV * Mathf.Cos(angleRadians);

        if (position.y < -scene.y || scene.y < position.y || position.x < -scene.x || scene.x < position.x)
        {
            disableAndRemove();
        }

        transform.position = position;
    }
}
