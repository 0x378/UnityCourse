using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatusBar : MonoBehaviour
{
    public Text status;
    public Text message;
    public GameObject plane1, plane2, plane3;

    // HERO:
    public bool mouseMode = true;
    public bool damageEnabled = false;
    public int enemyCollisions;

    // PROJECTILES:
    public int numberOfProjectiles;

    // ENEMY:
    public int numberOfPlanes;
    public int planesDestroyed;

    // WAYPOINT:
    public Vector3[] waypointPositions;
    public bool waypointMovementEnabled = false;
    public bool sequentialWaypoints = true;
    public bool showWaypoints = true;

    public static Vector3 getRandomPosition()
    {
        Vector3 scene = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

        // Keep plane spawns away from edges:
        scene.x *= 0.875f;
        scene.y *= 0.875f;

        return new Vector3(Random.Range(-scene.x, scene.x), Random.Range(-scene.y, scene.y), 0f);
    }

    public static Quaternion getRandomDirection()
    {
        return Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }

    void Start()
    {
        message.text = ""; // Clear messages

        Hero.systemStatus = this;
        enemyCollisions = 0;

        Egg.systemStatus = this;
        Missile.systemStatus = this;
        numberOfProjectiles = 0;

        Plane.systemStatus = this;
        numberOfPlanes = 0;
        planesDestroyed = 0;

        Waypoint.systemStatus = this;
    }

    private void PlaneSpawner()
    {
        while (numberOfPlanes < 10)
        {
            float planeSelector = Random.Range(0f, 100f);

            if (planeSelector < 67) // Values of 0 to 67 for a 67% chance of Plane 1:
            {
                Instantiate(plane1, getRandomPosition(), getRandomDirection());
                numberOfPlanes++;
            }
            else if (planeSelector < 88) // Values of 67 to 88 for a 21% chance of Plane 2:
            {
                Instantiate(plane2, getRandomPosition(), getRandomDirection());
                numberOfPlanes++;
            }
            else // Values of 88 to 100, for a 12% chance of Plane 3:
            {
                Instantiate(plane3, getRandomPosition(), getRandomDirection());
                numberOfPlanes++;
            }
        }
    }

    void UpdateText()
    {
        status.text = "HERO:  Control[";

        if (mouseMode)
        {
            status.text += "mouse";
        }
        else
        {
            status.text += "keyboard";
        }

        status.text += "],  Damage[";

        if (damageEnabled)
        {
            status.text += "enabled";
        }
        else
        {
            status.text += "disabled";
        }

        status.text += "],  PlaneCollisions[" + enemyCollisions;
        status.text += "]\nENEMY:  Destroyed[" + planesDestroyed;
        status.text += "],  Waypoints[";

        if (sequentialWaypoints)
        {
            status.text += "sequential";
        }
        else
        {
            status.text += "random";
        }

        status.text += "],  PROJECTILES:  OnScreen[" + numberOfProjectiles + "]";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            showWaypoints = !showWaypoints;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            sequentialWaypoints = !sequentialWaypoints;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            waypointMovementEnabled = !waypointMovementEnabled;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reset the scene:
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Gameplay");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }

        PlaneSpawner();
        UpdateText();
    }
}
