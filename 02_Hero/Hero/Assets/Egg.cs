using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    private float velocity;
    private Vector3 position;
    private Vector3 scene;

    void Start()
    {
        velocity = 40f; // units per second
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name.Length > 4)
        {
            if (collider.gameObject.name.Substring(0, 5) == "Plane")
            {
                Plane enemy = collider.gameObject.GetComponent<Plane>();
                enemy.damageBy(10);
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        scene = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        position = transform.position;

        float deltaV = velocity * Time.deltaTime;
        float angleRadians = transform.localEulerAngles.z * Mathf.Deg2Rad;
        position.x -= deltaV * Mathf.Sin(angleRadians);
        position.y += deltaV * Mathf.Cos(angleRadians);

        if (position.y < -scene.y || scene.y < position.y || position.x < -scene.x || scene.x < position.x)
        {
            Destroy(gameObject);
        }

        transform.position = position;
    }
}
