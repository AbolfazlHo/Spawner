# 2D Spawner Package

A powerful and flexible 2D Spawner package for Unity, designed to streamline the process of spawning 2D objects with advanced features like automated spawning, collision safety, and grid-based placement.

## Features

* **Automated Spawning**: Easily configure objects to spawn automatically with customizable intervals.
* **Limitation Controls**: Set limitations based on time or count for automatic spawning.
* **Collision Safe Placement**: Ensures spawned objects do not overlap with existing colliders, finding a free space if needed.
* **Grid-based Placement**: Effortlessly place objects in a predefined grid structure.
* **Event-Driven**: Utilize Unity Events for flexible callbacks on spawn start, spawn end, and limitation reached.
* **Highly Customizable**: Exposes key parameters in the Inspector for easy setup without coding.

## Installation

### Prerequisites

This package relies on [UniTask](https://github.com/Cysharp/UniTask) for asynchronous operations. Please ensure UniTask is installed in your Unity project **before** installing this 2D Spawner package.
First add UniTask into your project from its repository in github.

### 2D Spawner installation

#### Install via git url using unity package manager
To install `2D Spawner` via git url using unity package manager follow these steps.
 1. Open your Unity project
 2. Go to `Windows > Package Manager`.
 3. Click the `+` icon in the top-left conner of the package manager window.
 4. Select `Add package from git URL ...`.
 5. paste the following URL:
 ```
https://github.com/AbolfazlHo/Spawner.git#main
```
 
## How to Use

### 1.Basic Setup

1. Create an empty GameObject in your scene (e.g., `MySpawner`).
2. Add the `Spawner` component to it.
3. Assign the 2D Prefabs you want to spawn to the `Spawnables` list.
4. Define a `Spawn Area GameObject` (e.g., a Quad with a Collider2D) to control where objects can spawn.

### 2.Automated Spawning

1. On the `Spawner` component, enable `Spawn Automatically`.
2. Expand the `Spawn Automation Settings` foldout.
3. Set the `Per Spawn Interval` (delay between spawns).
4. If `Stop Spawning Automatically` is enabled, configure `Limitation Settings`:
    * Choose `Limitation Type` (Count or Time).
    * Set `Limit Count By` (e.g., 10 objects) or `Limit Time By` (e.g., 60 seconds).

### 3.Collision Safe Placement

1. Enable `Is Collision` Safe on the `Spawner` component.
2. Expand `Collision Safety Settings`.
3. Ensure your `Spawnable` prefabs have appropriate `Collider2D` components. The system will try to find a free spot if the initial position is occupied.

### 4.Grid-based Placement

1. Within Collision Safety Settings, enable Is Grid Placement.
2. Expand Grid Placement Settings.
3. Set Spawnable Size (the base size of your spawned objects).
4. Adjust Padding to add space between grid cells.
5. Configure Rows Count or Columns Count to define your grid dimensions. The system will automatically calculate cell sizes.
    * If Columns Count is greater than 0, row and column are calculated based on it.
    * Otherwise, row and column are calculated based on Rows Count.

### 5.Events

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

