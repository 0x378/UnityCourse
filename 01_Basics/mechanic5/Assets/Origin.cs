using UnityEngine;

public class Origin : MonoBehaviour
{
    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 scene = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            pos.x = mouse.x;
            pos.y = mouse.y;

            if (pos.x < -scene.x)
            {
                pos.x = -scene.x;
            }
            else if (pos.x > scene.x)
            {
                pos.x = scene.x;
            }

            if (pos.y < -scene.y)
            {
                pos.y = -scene.y;
            }
            else if (pos.y > scene.y)
            {
                pos.y = scene.y;
            }
        }

        transform.position = pos;
    }
}
