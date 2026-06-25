# Prototype 4

An arcade-style sumo battle completed during the Unity Junior Programmer pathway.

## What I Learned

### [Lesson 4.1 - Watch Where You’re Going](https://learn.unity.com/pathway/junior-programmer/unit/gameplay-mechanics/tutorial/lesson-4-1-watch-where-you-re-going-2?version=6.3)

**New functionality**

- **Camera rotates around the island based on horizontal input:** [RotateCamera](Assets/Scripts/RotateCamera.cs) reads the `Move` action's X value from the [new Input System action map](Assets/InputSystem_Actions.inputactions).
- **Player rolls in the direction of the camera based on vertical input:** [PlayerController](Assets/Scripts/PlayerController.cs) reads the same new Input System action's Y value and applies force along the focal point's forward direction.

**New concepts & skills**

- **Texture wraps:** control how a texture behaves when its UV coordinates extend beyond its normal range.
- **Camera as child object:** parenting the camera to the focal point makes it orbit the island when the focal point rotates.
- **Global vs. local coordinates:** global coordinates describe the scene, while local coordinates are relative to an object's parent.
- **Get direction of another object:** subtract positions and normalize the result to produce a direction vector, as used by [EnemyController](Assets/Scripts/EnemyController.cs).

### [Lesson 4.2 - Follow the Player](https://learn.unity.com/pathway/junior-programmer/unit/gameplay-mechanics/tutorial/lesson-plan-4-2-follow-the-player?version=6.3)

**New functionality**

- **Enemy spawns at a random location on the island:** [SpawnManager](Assets/Scripts/SpawnManager.cs) generates random X and Z coordinates inside the arena.
- **Enemy follows the player around:** [EnemyController](Assets/Scripts/EnemyController.cs) continually applies force toward the player's position.
- **Spheres bounce off each other:** a bouncy **Physics Material** configures the player and enemies to rebound during collisions.

**New concepts & skills**

- **Physics Materials:** configure collider friction and bounciness without writing collision code.
- **Defining vectors in 3D space:** `Vector3` stores X, Y, and Z values for positions and directions.
- **Normalizing values:** `.normalized` keeps a direction vector's length at one so distance does not multiply movement force.
- **Methods with return values:** `GenerateSpawnPosition` calculates and returns a `Vector3` for object spawning in [SpawnManager](Assets/Scripts/SpawnManager.cs).

### [Lesson 4.3 - PowerUp and CountDown](https://learn.unity.com/pathway/junior-programmer/unit/gameplay-mechanics/tutorial/lesson-4-3-powerup-and-countdown?version=6.3)

**New functionality**

- **A visual indicator appears when the player collects a powerup:** [PlayerController](Assets/Scripts/PlayerController.cs) activates and positions the powerup indicator.
- **Enemies go flying when hit while the powerup is active:** [PlayerController](Assets/Scripts/PlayerController.cs) applies an impulse away from the player.
- **The powerup ability and indicator disappear after a set time:** a seven-second countdown disables both in [PlayerController](Assets/Scripts/PlayerController.cs).

**New concepts & skills**

- Debug concatenation.
- **Local component variables:** cached component references such as `Rigidbody` avoid repeated lookups and make later code clearer.
- **`IEnumerator` and `WaitForSeconds()`:** an iterator method can pause its sequence for a specified duration.
- **Coroutines:** Unity runs timed logic across multiple frames without blocking the game.
- `SetActive(true/false)`.

### [Lesson 4.4 - For-Loops For Waves](https://learn.unity.com/pathway/junior-programmer/unit/gameplay-mechanics/tutorial/lesson-4-4-for-loops-for-waves?version=6.3)

**New functionality**

- **Enemies spawn in waves:** [SpawnManager](Assets/Scripts/SpawnManager.cs) creates a batch after the previous enemies are defeated.
- **The number of enemies increases after every defeated wave:** `waveNumber` is incremented and passed to the next spawn loop in [SpawnManager](Assets/Scripts/SpawnManager.cs).
- **A new powerup spawns with every wave:** [SpawnManager](Assets/Scripts/SpawnManager.cs) always creates a knockback powerup and may also create a rocket powerup.

**New concepts & skills**

- For-loops.
- Increment (`++`) operator.
- **Custom methods with parameters:** `SpawnEnemyWave(int enemyToSpawn)` receives the current wave size.
- **`FindObjectsByType`:** finds active components of a specified type, which [SpawnManager](Assets/Scripts/SpawnManager.cs) uses to detect when no standard enemies remain.

## Extra

- **Rocket powerup:** each wave has a 25% chance to spawn one; collecting it fires three volleys of homing rockets at every enemy through [SpawnManager](Assets/Scripts/SpawnManager.cs), [PlayerController](Assets/Scripts/PlayerController.cs), [PowerupPickup](Assets/Scripts/PowerupPickup.cs), and [RocketProjectile](Assets/Scripts/RocketProjectile.cs).
- **Jump Smash:** pressing Space through the new Input System's `Jump` action launches a radial knockback attack with a ten-second cooldown and UI indicator in [PlayerController](Assets/Scripts/PlayerController.cs).
- **Boss waves:** every fifth wave spawns a random boss through [SpawnManager](Assets/Scripts/SpawnManager.cs) and [BossController](Assets/Scripts/BossController.cs).
  - **Heavy Boss:** has greater mass and periodically spawns two-enemy reinforcements.
  - **Sniper Boss:** periodically fires homing rockets at the player.
