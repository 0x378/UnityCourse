# Unity Basics:
For this project, I started with a tutorial from [Tech For Good](https://github.com/t4guw) on using the [WASD keys for 2D movement](https://github.com/t4guw/100-Unity-Mechanics-for-Programmers/tree/master/programs/wasd_movement_2d), and created five separate modifications to its mechanics. I played around with various physics such as acceleration, gravity, air resistance, exponential decay, and spring constants to see how well I could implement each in Unity.

## Mechanic #1
A simple expansion upon the WASD movement script, allowing the player to also move diagonally using Q, E, Z, and C, but limiting the velocity in any direction to a fixed value with the help of the Pythagorean theorem. <br/>
**[Click here to see it in WebGL!](https://0x378.github.io/UnityCourse/01_Basics/mechanic1/WebGL_mechanic1)**

## Mechanic #2
Reapplied the controls of mechanic #1 to the player's acceleration instead of velocity; the player will accelerate in the direction of the controls being pressed, gradually building up speed, but losing half of its horizontal or vertical velocity component each time that it bounces off of a wall. <br/>
**[Click here to see it in WebGL!](https://0x378.github.io/UnityCourse/01_Basics/mechanic2/WebGL_mechanic2)**

## Mechanic #3
Added gravity, but reduced the controls to use only the A, W, and D keys, where W will allow the player to jump if currently touching the bottom of the screen. <br/>
**[Click here to see it in WebGL!](https://0x378.github.io/UnityCourse/01_Basics/mechanic3/WebGL_mechanic3)**

## Mechanic #4
Revisiting the acceleration functionality from mechanic #2, I added air resistance in order to simulate terminal velocity, with a deceleration magnitude directly proportional to the square of the current velocity. I also changed the bounds of the screen such that, if the player goes off the edge, it will be teleported to the opposite side to continue its trajectory. <br/>
**[Click here to see it in WebGL!](https://0x378.github.io/UnityCourse/01_Basics/mechanic4/WebGL_mechanic4)**

## Mechanic #5
Using WASD keys for player movement, as well as mouse clicks to relocate a yellow dot, the player is now tied to a point of origin using a “spring” effect. A force pulling the player towards the dot is directly proportional to the player’s distance away from the dot. <br/>
**[Click here to see it in WebGL!](https://0x378.github.io/UnityCourse/01_Basics/mechanic5/WebGL_mechanic5)**
