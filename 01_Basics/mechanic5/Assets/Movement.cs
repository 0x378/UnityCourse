using UnityEngine;

public class Movement : MonoBehaviour
{
    public float spring_coefficient_x = 0.95f;
    public float spring_coefficient_y = 2.3f;
    public float decay_coefficient = 0.995f;
    public float speed = 10f;
    public float velocity_x = 0;
    public float velocity_y = 0;

    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 scene = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        scene.x -= 0.64f;
        scene.y -= 0.64f;

        // "w" can be replaced with any key
        // this section moves the character up
        if (Input.GetKey("w"))
        {
            velocity_y = speed;
        }

        // "s" can be replaced with any key
        // this section moves the character down
        if (Input.GetKey("s"))
        {
            velocity_y = -speed;
        }

        // "d" can be replaced with any key
        // this section moves the character right
        if (Input.GetKey("d"))
        {
            velocity_x = speed;
        }

        // "a" can be replaced with any key
        // this section moves the character left
        if (Input.GetKey("a"))
        {
            velocity_x = -speed;
        }

        velocity_x *= decay_coefficient;
        velocity_y *= decay_coefficient;

        velocity_x -= spring_coefficient_x * pos.x;
        velocity_y -= spring_coefficient_y * pos.y;

        pos.x += velocity_x * Time.deltaTime;
        pos.y += velocity_y * Time.deltaTime;

        transform.position = pos;
    }
}
