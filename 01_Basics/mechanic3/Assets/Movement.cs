using UnityEngine;

public class Movement : MonoBehaviour
{
    public float decay_coefficient = 0.996f;
    public float acceleration = 18f;
    public float gravity = -30f;
    public float velocity_x = 0;
    public float velocity_y = 0;

    void Update()
    {
        velocity_y += gravity * Time.deltaTime;
        velocity_x *= decay_coefficient;

        Vector3 pos = transform.position;
        Vector3 scene = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        scene.x -= 0.64f;
        scene.y -= 0.64f;

        float max_acceleration = acceleration * Time.deltaTime;

        // "d" can be replaced with any key
        // this section moves the character right
        if (Input.GetKey("d"))
        {
            velocity_x += max_acceleration;
        }

        // "a" can be replaced with any key
        // this section moves the character left
        if (Input.GetKey("a"))
        {
            velocity_x -= max_acceleration;
        }

        // If currently touching the ground:
        if (pos.y <= -scene.y)
        {
            // Jump key:
            if (Input.GetKey("w"))
            {
                velocity_y = 15f;
            }
            else
            {
                // Bounce off the ground if there's downward velocity:
                if (velocity_y < 0)
                {
                    velocity_y = -velocity_y / 4;
                }
            }
        }

        pos.x += velocity_x * Time.deltaTime;
        pos.y += velocity_y * Time.deltaTime;

        // Prevent falling below the bottom of the screen:
        if (pos.y < -scene.y)
        {
            pos.y = -scene.y;
        }

        // Bounce off of the walls:
        if (pos.x > scene.x)
        {
            pos.x = scene.x;

            if (velocity_x > 0)
            {
                velocity_x = -velocity_x / 2;
            }
        }

        if (pos.x < -scene.x)
        {
            pos.x = -scene.x;

            if (velocity_x < 0)
            {
                velocity_x = -velocity_x / 2;
            }
        }

        transform.position = pos;
    }
}
