# Unity Basics:
For this project, I started with a tutorial from [Tech For Good](https://github.com/t4guw) on using the [WASD keys for 2D movement](https://github.com/t4guw/100-Unity-Mechanics-for-Programmers/tree/master/programs/wasd_movement_2d), and created five separate modifications to its mechanics. I played around with various physics such as acceleration, gravity, air resistance, exponential decay, and spring constants to see how well I could implement each in Unity.

## Mechanic #1
A simple expansion to the WASD movement script, allowing the player to also move diagonally using Q, E, Z, and C, but limiting the velocity in any direction to a fixed value with the help of the Pythagorean theorem. <br/>
**[Click here](https://0x378.github.io/UnityCourse/01_Basics/mechanic1/WebGL_mechanic1) to see it in WebGL.**

## Mechanic #2
Reapplied the controls of mechanic #1 upon acceleration instead of velocity; the player will accelerate in the direction of the controls pressed, gradually building up speed, but losing half of its horizontal or vertical velocity component each time that it bounces off of a wall. <br/>
**[Click here](https://0x378.github.io/UnityCourse/01_Basics/mechanic2/WebGL_mechanic2) to see it in WebGL.**

## Mechanic #3
Added gravity, and changed the controls to use only the AWD keys, where W will act as a jump if the player is currently touching the bottom of the screen. <br/>
**[Click here](https://0x378.github.io/UnityCourse/01_Basics/mechanic3/WebGL_mechanic3) to see it in WebGL.**

## Mechanic #4
Revisiting the acceleration functionality of mechanic #2, I changed the bounds of the screen such that, if the player goes off the edge, it will be teleported to the opposite side and continue its trajectory. A terminal velocity is implemented using air resistance, with a deceleration magnitude directly proportional to the square of the current velocity. <br/>
**[Click here](https://0x378.github.io/UnityCourse/01_Basics/mechanic4/WebGL_mechanic4) to see it in WebGL.**

## Mechanic #5
Using WASD keys for player movement, as well as mouse clicks to relocate a yellow dot, the player is now tied to the dot using a “spring” effect. The force pulling the player towards the dot is directly proportional to the player’s distance away from the dot. <br/>
**[Click here](https://0x378.github.io/UnityCourse/01_Basics/mechanic5/WebGL_mechanic5) to see it in WebGL.**
