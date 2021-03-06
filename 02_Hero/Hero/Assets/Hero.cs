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
    public Egg projectile;
    public Missile missile;

    // Initialized upon startup:
    public HealthBar healthSrc;
    private HealthBar healthBar;
    public float velocity;         // Initial value: 20 units/sec (for keyboard-only mode)

    // Sprite image handling variables:
    private int imageState;         // Array index for sprite images
    private int numberOfImages;     // Number of sprite images in the array
    private float maximumImageTime; // Maximum elapsed time between sprite images
    private float currentImageTime; // The time at which the current sprite image appeared
    private SpriteRenderer currentSprite;

    // Weapon handling variables:
    public float ProjectileCooldownDuration = 0.2f; // Cooldown duration for eggs
    public float MissileCooldownDuration = 2.75f; // Cooldown duration for missiles
    private float previousProjectileTime;
    private float previousMissileTime;

    // Reinitialized upon each update:
    private Vector3 position;
    private Vector3 scene;
    private Vector3 mouse;

    // Initialized only once, by the StatusBar itself:
    public static StatusBar systemStatus;

    // Start is called before the first frame update
    void Start()
    {
        velocity = 20f;

        imageState = 0;
        numberOfImages = sprites.Length;
        maximumImageTime = 60f / (3f * numberOfImages * propellerRPM);
        currentImageTime = Time.time;
        previousProjectileTime = Time.time;
        previousMissileTime = Time.time;
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
        if (systemStatus.mouseMode)
        {
            velocity = 0;
        }
        else
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

        if (systemStatus.mouseMode)
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

            if (elapsedTime > ProjectileCooldownDuration)
            {
                Egg newEgg = Instantiate(projectile, position, transform.rotation) as Egg;
                systemStatus.numberOfProjectiles++;

                if (velocity > 0)
                {
                    newEgg.velocity += velocity;
                }

                previousProjectileTime = Time.time;
            }
        }

        if (Input.GetKey(KeyCode.F))
        {
            float elapsedTime = Time.time - previousMissileTime;

            if (elapsedTime > MissileCooldownDuration)
            {
                Missile newMissile = Instantiate(missile, position, transform.rotation) as Missile;
                systemStatus.numberOfProjectiles++;

                if (velocity > 0)
                {
                    newMissile.velocity += velocity;
                }

                previousMissileTime = Time.time;
            }
        }
    }

    public void damageBy(int amount)
    {
        if (systemStatus.damageEnabled)
        {
            healthBar.Subtract(amount);

            if (!healthBar.isAlive())
            {
                systemStatus.message.text = "You died! :'(\nPress R to reset.";
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name.Length > 4)
        {
            if (collider.gameObject.name.Substring(0, 5) == "Plane")
            {
                Plane enemy = collider.gameObject.GetComponent<Plane>();
                enemy.damageBy(100);
                systemStatus.enemyCollisions++;
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
            systemStatus.mouseMode = !systemStatus.mouseMode;

            if (systemStatus.mouseMode)
            {
                velocity = 0f;
            }
            else
            {
                velocity = 20f;
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            systemStatus.damageEnabled = !systemStatus.damageEnabled;

            if (systemStatus.damageEnabled)
            {
                // Create new health bar:
                healthBar = Instantiate(healthSrc, transform.position, Quaternion.identity) as HealthBar;
                healthBar.Setup(gameObject, 100);
            }
            else
            {
                // Triggers the health bar to delete itself:
                healthBar.parentObject = null;
            }
        }

        UpdateSpriteImage();
        UpdatePlayerDirection();
        UpdatePlayerVelocity();
        UpdatePlayerPosition();
        UpdateWeapon();
    }
}
