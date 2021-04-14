using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    // Initialized by user:
    public float propellerRPM = 150f;
    public float maximumAcceleration = 15f;
    public float maximumVelocity = 80f;
    public Sprite[] sprites;
    public GameObject projectile;

    // Initialized upon startup:
    public HealthBar healthBar;
    private bool mouseMode;         // 0 = keyboard only, 1 = mouse
    private bool damageEnabled;     // 0 = no player damage, 1 = enabled
    public float velocity;         // Initial value: 20 units/sec (for keyboard-only mode)

    // Sprite image handling variables:
    private int imageState;         // Array index for sprite images
    private int numberOfImages;     // Number of sprite images in the array
    private float maximumImageTime; // Maximum elapsed time between sprite images
    private float currentImageTime; // The time at which the current sprite image appeared
    private SpriteRenderer currentSprite;

    // Weapon handling variables:
    public float WeaponCooldownDuration = 0.2f; // Cooldown duration for weapon
    private float previousProjectileTime;

    // Reinitialized upon each update:
    private Vector3 position;
    private Vector3 scene;
    private Vector3 mouse;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = Instantiate(healthBar, transform.position, Quaternion.identity) as HealthBar;
        healthBar.Setup(gameObject, 100);
        mouseMode = true;
        damageEnabled = false;
        velocity = 20f;

        imageState = 0;
        numberOfImages = sprites.Length;
        maximumImageTime = 60f / (3f * numberOfImages * propellerRPM);
        currentImageTime = Time.time;
        previousProjectileTime = Time.time;
        currentSprite = gameObject.GetComponent<SpriteRenderer>();
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

    // Allows the player to accelerate forwards or backwards, but
    // limiting the maximum velocity by reducing the acceleration
    // towards zero as current velocity approaches the terminal value.
    private void UpdatePlayerVelocity()
    {
        // Go faster if currently moving forward, or slower if backward:
        if (Input.GetKey(KeyCode.W))
        {
            velocity += maximumAcceleration * Time.deltaTime * (1 - velocity / maximumVelocity);
        }

        // Go slower if currently moving forward, or faster if backward:
        if (Input.GetKey(KeyCode.S))
        {
            velocity -= maximumAcceleration * Time.deltaTime * (1 + velocity / maximumVelocity);
        }
    }

    private void UpdatePositionMouseMode()
    {
        position.x = mouse.x;
        position.y = mouse.y;

        // Wall checks:
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
    }

    public void UpdatePositionKeyboardMode()
    {
        float deltaV = velocity * Time.deltaTime;
        float angleRadians = transform.localEulerAngles.z * Mathf.Deg2Rad;
        position.x -= deltaV * Mathf.Sin(angleRadians);
        position.y += deltaV * Mathf.Cos(angleRadians);

        // Wall checks:
        if (position.x < -scene.x) // Bounce off the left wall:
        {
            position.x = -scene.x;
            transform.eulerAngles = new Vector3(0, 0, -transform.localEulerAngles.z);
        }
        else if (position.x > scene.x) // Bounce off the right wall:
        {
            position.x = scene.x;
            transform.eulerAngles = new Vector3(0, 0, -transform.localEulerAngles.z);
        }

        if (position.y < -scene.y) // Bounce off the top wall:
        {
            position.y = -scene.y;
            transform.eulerAngles = new Vector3(0, 0, 180 - transform.localEulerAngles.z);
        }
        else if (position.y > scene.y) // Bounce off the bottom wall:
        {
            position.y = scene.y;
            transform.eulerAngles = new Vector3(0, 0, 180 - transform.localEulerAngles.z);
        }
    }

    public void UpdatePlayerPosition()
    {
        position = transform.position;

        if (mouseMode)
        {
            UpdatePositionMouseMode();
        }
        else
        {
            UpdatePositionKeyboardMode();
        }

        transform.position = position;
    }

    public void UpdateWeapon()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float elapsedTime = Time.time - previousProjectileTime;

            if (elapsedTime > WeaponCooldownDuration)
            {
                Instantiate(projectile, position, transform.rotation);
                previousProjectileTime = Time.time;
            }
        }
    }

    public void damageBy(int amount)
    {
        if (damageEnabled)
        {
            healthBar.Subtract(amount);

            if (!healthBar.isAlive())
            {
                Destroy(gameObject);
            }
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name.Length > 4)
        {
            if (collider.gameObject.name.Substring(0, 5) == "Plane")
            {
                Plane enemy = collider.gameObject.GetComponent<Plane>();
                enemy.damageBy(100);
                this.damageBy(10);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        scene = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.M))
        {
            mouseMode = !mouseMode;
            velocity = 20f;
        }

        UpdateSpriteImage();
        UpdatePlayerDirection();
        UpdatePlayerVelocity();
        UpdatePlayerPosition();
        UpdateWeapon();
    }
}
