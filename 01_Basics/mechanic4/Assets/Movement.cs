using UnityEngine;

public class Movement : MonoBehaviour
{
    public float drag_coefficient = 0.00003f;
    public float acceleration = 10f;
    public float velocity_x = 0;
    public float velocity_y = 0;

    void Update()
    {
        if(velocity_x > 0){
            velocity_x -= drag_coefficient * velocity_x * velocity_x;
        }
        else
        {
            velocity_x += drag_coefficient * velocity_x * velocity_x;
        }

        if (velocity_y > 0)
        {
            velocity_y -= drag_coefficient * velocity_y * velocity_y;
        }
        else
        {
            velocity_y += drag_coefficient * velocity_y * velocity_y;
        }
        

        Vector3 pos = transform.position;
        Vector3 scene = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        scene.x += 0.64f;
        scene.y += 0.64f;

        float delta_x = 0;
        float delta_y = 0;
        float max_acceleration = acceleration * Time.deltaTime;

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

            // The unit vector ensures that the acceleration of the object never
            // exceeds the max_acceleration limit in any direction, even if
            // multiple control keys are pressed simultaneously.
            velocity_x += max_acceleration * delta_x;
            velocity_y += max_acceleration * delta_y;
        }

        pos.x += velocity_x * Time.deltaTime;
        pos.y += velocity_y * Time.deltaTime;

        if (pos.x > scene.x)
        {
            pos.x = -scene.x;
        }

        if (pos.x < -scene.x)
        {
            pos.x = scene.x;
        }

        if (pos.y > scene.y)
        {
            pos.y = -scene.y;
        }

        if (pos.y < -scene.y)
        {
            pos.y = scene.y;
        }

        transform.position = pos;
    }
}
