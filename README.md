# 2D Spawner Package

![Unity](https://img.shields.io/badge/Unity-6000.0.30f1%2B-black?logo=unity)
![License](https://img.shields.io/github/license/AbolfazlHo/Spawner)

A powerful and flexible 2D Spawner package for Unity, designed to streamline the process of spawning 2D objects with advanced features like automated spawning, collision safety, and grid-based placement.

## 📖 Table of Contents
* [Features](#-features)
* [Quick Start](#-quick-start)
* [Installation](#-installation)
* [How to Use](#-how-to-use)
    * [Basic Setup](#1-basic-setup)
    * [Automated Spawning](#2-automated-spawning)
    * [Collision Safe Placement](#3-collision-safe-placement)
    * [Grid-based Placement](#4-grid-based-placement)
    * [Events](#5-events)

* [API Reference](#-api-reference)
    * [Spawner](#spawner-monobehaviour)    
    * [Spawnable](#spawnable)
    * [Automation](#automation)
    * [Limitation](#limitation)
    * [CollisionSafety](#collisionsafety)
    * [GridPlacement](#gridplacement)
    * [Inspector Integration](#inspector-integration)
    * [Example Usage](#example-usage)
    
* [Contributions](#-contributions)
* [License](#-license)
    
## 🚀 Features

* **Automated Spawning**: Easily configure objects to spawn automatically with customizable intervals.
* **Limitation Controls**: Set limitations based on time or count for automatic spawning.
* **Collision Safe Placement**: Ensures spawned objects do not overlap with existing colliders, finding a free space if needed.
* **Grid-based Placement**: Effortlessly place objects in a predefined grid structure.
* **Event-Driven**: Utilize Unity Events for flexible callbacks on spawn start, spawn end, and limitation reached.
* **Highly Customizable**: Exposes key parameters in the Inspector for easy setup without coding.

## 📦 Installation

### Prerequisites

This package relies on [UniTask](https://github.com/Cysharp/UniTask) for asynchronous operations. Please ensure UniTask is installed in your Unity project **before** installing this 2D Spawner package.
First, add UniTask to your project from its GitHub repository.

### 2D Spawner installation

#### Install via Git URL using Unity Package Manager
To install `2D Spawner` via git url using unity package manager follow these steps.
 1. Open your Unity project
 2. Go to `Windows > Package Manager`.
 3. Click the `+` icon in the top-left corner of the package manager window.
 4. Select `Add package from git URL ...`.
 5. Paste the following URL and click "Add":
 ```
https://github.com/AbolfazlHo/Spawner.git#main
```
 
 ## ⚡ Quick Start
 
 1. Install `UniTask`.
 2. Install the `2D Spawner` package via Git URL.
 3. Create an empty GameObject (e.g., `MySpawner`) in your scene.
 4. Add the `Spawner` component to it.
 5. Assign the desired 2D prefabs to the `Spawnables` list.
 6. Add a spawn area GameObject (with a Collider2D) and assign it in the `Spawner` component.
 7. Enable `Spawn Automatically`, or call `Spawn()` manually from script.
 
## 🛠 How to Use

### 1. Basic Setup

1. Create an empty GameObject in your scene (e.g., `MySpawner`).
2. Add the `Spawner` component to it.
3. Assign the 2D Prefabs you want to spawn to the `Spawnables` list.
4. Define a `Spawn Area GameObject` (e.g., a Quad with a Collider2D) to control where objects can spawn.

### 2. Automated Spawning

1. On the `Spawner` component, enable `Spawn Automatically`.
2. Expand the `Spawn Automation Settings` foldout.
3. Set the `Per Spawn Interval` (delay between spawns).
4. If `Stop Spawning Automatically` is enabled, configure `Limitation Settings`:
    * Choose `Limitation Type` (Count or Time).
    * Set `Limit Count By` (e.g., 10 objects) or `Limit Time By` (e.g., 60 seconds).

### 3. Collision Safe Placement

1. Enable `Is Collision Safe` on the `Spawner` component.
2. Expand `Collision Safety Settings`.
3. Ensure your `Spawnable` prefabs have appropriate `Collider2D` components. The system will try to find a free spot if the initial position is occupied.

### 4. Grid-based Placement

1. Within `Collision Safety Settings`, enable `Is Grid Placement`.
2. Expand `Grid Placement Settings`.
3. Set `Spawnable Size` (the base size of your spawned objects).
4. Adjust `Padding` to add space between grid cells.
5. Configure `Rows Count` or `Columns Count` to define your grid dimensions. The system will automatically calculate cell sizes.
    * If `Columns Count` is greater than 0, row and column are calculated based on it.
    * Otherwise, row and column are calculated based on Rows Count.

### 5. Events

The `Spawner`, `Automation`, and `Limitation` components expose `UnityEvents` that you can hook into for custom logic:

* Spawner: 
    * `On Spawnable Spawned Event`: Invoked whenever a new object is spawned.
    
* Automation:
    * `On Spawn Start Event`: Invoked when automatic spawning begins.
    * `On Spawn End Event`: Invoked when automatic spawning finishes (either naturally or when stopped).
    
* Limitation:
    * `On Spawn Start Event`: Invoked by `Automation` when its spawning starts (if automatic spawning is limited).
    * `On Limitation Reached Event`: Invoked when the defined spawn count or time limit is reached.
    * `On Spawn End Event`: Invoked by `Automation` when its spawning ends (if automatic spawning is limited).
    
## 🧩 API Reference

This API Reference provides a complete overview of the structure, properties, and behavior of the Spawner2D package.

---

### `Spawner` (MonoBehaviour)

Main class to control the spawning process of game objects in the scene.

#### Public Methods

##### `void Spawn()`

- Immediately spawns an object based on the current settings.

##### `void StopSpawning()`

- Stops the automatic spawning process (if active).

#### Inspector Properties

- `GameObject _spawnAreaGameObject`

  - GameObject that defines the spawn area. Typically contains a BoxCollider2D.

- `Spawnable[] _spawnables`

  - List of objects that can be spawned.

- `bool _spawnAutomatically`

  - Determines whether the spawn process starts automatically.

- `Automation _spawnAutomationSettings`

  - Settings related to automatic spawning.

- `bool _isCollisionSafe`

  - Enables collision safety checks during spawning.

- `CollisionSafety _collisionSafetySettings`

  - Settings related to collision safety (e.g., distance check or grid).

- `string _spawnableTag`

  - Custom tag for identifying spawnable objects.

- `UnityEvent<GameObject> _onSpawnableSpawnedEvent`

  - Event triggered after each object is spawned.

---

### `Spawnable`

Marker script to indicate that an object is spawnable.

---

### `Automation`

Controls the timing and conditions for starting/stopping automatic spawning.

#### Properties

- `float _perSpawnInterval`

  - Time interval between each spawn.

- `bool _stopSpawningAutomatically`

  - Determines whether the spawn process stops automatically.

- `Limitation _limitationSettings`

  - Limitations that stop the spawn process (by count or time).

- `UnityEvent _onSpawnStartEvent`

  - Event triggered at the start of spawning.

- `UnityEvent _onSpawnEndEvent`

  - Event triggered at the end of spawning.

> **Note**: Only one of `limitationSettings` or `onSpawnStart/EndEvent` is used depending on `stopSpawningAutomatically`.

---

### `Limitation`

Manages constraints for stopping the automatic spawning process.

#### Enum

```csharp
LimitationType { Time, Count }
```

#### Properties

- `LimitationType _limitationType`

  - Type of limitation (Time or Count).

- `float _limitTimeBy`

  - For Time: seconds after which spawning stops.

- `int _limitCountBy`

  - For Count: number of spawns after which spawning stops.

- `UnityEvent _onSpawnStartEvent`

- `UnityEvent _onLimitationReachedEvent`

- `UnityEvent _onSpawnEndEvent`

---

### `CollisionSafety`

Collision safety settings to prevent overlapping spawned objects.

#### Properties

- `bool _isPlacement`

  - Enables basic distance-based collision safety.

- `bool _isGridPlacement`

  - Enables grid-based placement.

- `GridPlacement _gridPlacementSettings`

  - Settings for grid-based placement.

---

### `GridPlacement`

Grid-based layout configuration.

#### Properties

- `Vector2 _cellSize`

  - Size of each grid cell.

- `Vector2Int _gridSize`

  - Number of cells along the X and Y axes.

- `Vector2 _gridOrigin`

  - Origin of the grid in world space.

---

### Inspector Integration

#### Custom Editors

`PropertyDrawer` classes are implemented for `Automation`, `Limitation`, and `CollisionSafety` to enhance the user experience in the Inspector. Conditional display of properties is used based on settings.

#### Main Editor: `SpawnerInspector`

Custom Inspector renderer for `Spawner` with intuitive layout and grouped controls including:

- Spawn Settings
- Automation
- Collision Safety
- Events

---

### Example Usage

```csharp
[SerializeField] private Spawner _spawner;

void Update()
{
    if (Input.GetKeyDown(KeyCode.Space))
    {
        _spawner.Spawn();
    }

    if (Input.GetKeyDown(KeyCode.LeftShift))
    {
        _spawner.StopSpawning();
    }
}
```

---

Let me know if you'd like more details about any class or internal behavior (e.g., UniTask usage).

## 🤝 Contributions

Contributions are welcome! If you find a bug or have a feature request, please open an issue on the [GitHub repository](https://github.com/AbolfazlHo/Spawner/issues).

## 📄 License

This package is distributed under the MIT License. See the [LICENSE](https://raw.githubusercontent.com/AbolfazlHo/Spawner/refs/heads/main/LICENSE) file for more information.



