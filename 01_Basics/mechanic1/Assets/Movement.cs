using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 scene = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        scene.x -= 0.64f;
        scene.y -= 0.64f;

        float delta_x = 0;
        float delta_y = 0;
        float max_velocity = speed * Time.deltaTime;

        // "w" can be replaced with any key
        // this section moves the character up
        if (Input.GetKey("w"))
        {
            delta_y += 1;
        }

        // "s" can be replaced with any key
        // this section moves the character down
        if (Input.GetKey("s") || Input.GetKey("x"))
        {
            delta_y -= 1;
        }

        // "d" can be replaced with any key
        // this section moves the character right
        if (Input.GetKey("d"))
        {
            delta_x += 1;
        }

        // "a" can be replaced with any key
        // this section moves the character left
        if (Input.GetKey("a"))
        {
            delta_x -= 1;
        }

        // q = w + a
        if (Input.GetKey("q"))
        {
            delta_y += 1;
            delta_x -= 1;
        }

        // e = w + d
        if (Input.GetKey("e"))
        {
            delta_y += 1;
            delta_x += 1;
        }

        // z = s + a
        if (Input.GetKey("z"))
        {
            delta_y -= 1;
            delta_x -= 1;
        }

        // c = s + d
        if (Input.GetKey("c"))
        {
            delta_y -= 1;
            delta_x += 1;
        }

        // Using the Pythagorean theorem:
        float delta_magnitude = Mathf.Sqrt((delta_x * delta_x) + (delta_y * delta_y));

        if (delta_magnitude != 0.0f)
        {
            // Convert the delta x and y coordinate pair to a unit vector:
            delta_x /= delta_magnitude;
            delta_y /= delta_magnitude;

            // The unit vector ensures that the speed of the object never
            // exceeds the max_velocity limit in any direction, even if
            // multiple control keys are pressed simultaneously.
            pos.x += max_velocity * delta_x;
            pos.y += max_velocity * delta_y;

            if (pos.x > scene.x)
            {
                pos.x = scene.x;
            }

            if (pos.x < -scene.x)
            {
                pos.x = -scene.x;
            }

            if (pos.y > scene.y)
            {
                pos.y = scene.y;
            }

            if (pos.y < -scene.y)
            {
                pos.y = -scene.y;
            }
        }

        transform.position = pos;
    }
}
