using UnityEngine;

public class Movement : MonoBehaviour
{
    public float acceleration = 11f;
    public float gravity = -30f;
    public float velocity_x = 0;
    public float velocity_y = 0;

    void Update()
    {
        velocity_y += gravity * Time.deltaTime;

        Vector3 pos = transform.position;
        Vector3 scene = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        scene.x -= 0.64f;
        scene.y -= 0.64f;

        float max_acceleration = acceleration * Time.deltaTime;

        // Jump key:
        if (Input.GetKey("w") && pos.y <= -scene.y)
        {
            velocity_y = 15f;
        }
        else if (pos.y < -scene.y)
        {
            pos.y = -scene.y;

            if (velocity_y < 0)
            {
                velocity_y = -velocity_y / 4;
            }
        }

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

        pos.x += velocity_x * Time.deltaTime;
        pos.y += velocity_y * Time.deltaTime;

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
