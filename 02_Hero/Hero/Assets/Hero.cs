using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    // Initialized by user:
    public float propellerRPM;
    public Sprite[] sprites;

    // Initialized upon startup:
    private int health;
    private bool mouseMode;         // 0 = keyboard only, 1 = mouse
    private bool damageEnabled;     // 0 = no player damage, 1 = enabled
    private float velocity;         // Initial value: 20 units/sec (for keyboard-only mode)

    private int imageState;         // Array index for sprite images
    private int numberOfImages;     // Number of sprite images in the array
    private float maximumImageTime; // Maximum elapsed time between sprite images
    private float currentImageTime; // The time at which the current sprite image appeared
    private SpriteRenderer currentSprite;

    // Reinitialized upon each update:
    private Vector3 position;
    private Vector3 scene;
    private Vector3 mouse;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        mouseMode = true;
        damageEnabled = false;
        imageState = 0;
        numberOfImages = sprites.Length;
        maximumImageTime = 60f / (3f * numberOfImages * propellerRPM);
        currentImageTime = Time.time;
        currentSprite = gameObject.GetComponent<SpriteRenderer>();

        velocity = 20f;
    }

    private void UpdateSpriteImage()
    {
        float elapsedImageTime = Time.time - currentImageTime;

        if (elapsedImageTime > maximumImageTime)
        {
            imageState++;

            if (imageState >= numberOfImages)
            {
                imageState = 0;
            }

            currentSprite.sprite = sprites[imageState];
            currentImageTime = Time.time;
        }
    }

    private void UpdatePlayerDirection()
    {
        float deltaAngle = 0;

        if (Input.GetKey(KeyCode.A))
        {
            deltaAngle += 45 * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            deltaAngle -= 45 * Time.deltaTime;
        }

        transform.Rotate(0, 0, deltaAngle);
    }

    private void UpdatePlayerPosition()
    {
        position = transform.position;

        if (mouseMode)
        {
            position.x = mouse.x;
            position.y = mouse.y;
        }
        else // Keyboard Mode:
        {

        }

        if (position.x < -scene.x)
        {
            position.x = -scene.x;
        }
        else if (position.x > scene.x)
        {
            position.x = scene.x;
        }

        if (position.y < -scene.y)
        {
            position.y = -scene.y;
        }
        else if (position.y > scene.y)
        {
            position.y = scene.y;
        }

        transform.position = position;
    }

    public int getHealth()
    {
        return health;
    }

    public void damageBy(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            health = 0;
            Destroy(this);
        }
    }

    public bool mouseIsEnabled()
    {
        return mouseMode;
    }

    public bool damageIsEnabled()
    {
        return damageEnabled;
    }

    // Update is called once per frame
    void Update()
    {
        scene = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.M))
        {
            mouseMode = !mouseMode;
        }

        UpdateSpriteImage();
        UpdatePlayerDirection();
        UpdatePlayerPosition();
    }
}
