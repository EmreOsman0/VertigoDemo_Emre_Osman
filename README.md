# Wheel of Fortune – Vertigo Games Demo

A risk-and-reward wheel of fortune game made in **Unity 6 (6000.3.23f1)** for the Vertigo Games Game Developer demo.

## How to play

- Press **SPIN**. The wheel lands on a random slice, and you collect that reward.
- Every spin moves you one **zone** further, and rewards grow with the zone (`amount × zone`).
- One slice is a **bomb**. Hit it and you lose everything you collected in this run.
- **Every 5th zone** is a **safe zone** with a Silver wheel and no bomb.
- **Every 30th zone** is a **super zone** with a Golden wheel, special rewards, and no bomb.
- You can **LEAVE** with your rewards only on a safe or super zone, and only while the wheel isn't spinning.

## Screenshots & video

| 20:9 | 16:9 | 4:3 |
| --- | --- | --- |
| ![20:9](Docs/Screenshots/20x9.png) | ![16:9](Docs/Screenshots/16x9.png) | ![4:3](Docs/Screenshots/4x3.png) |

Gameplay video: _add link_

The APK is attached to the latest **GitHub Release**.

## Project structure

```
Assets/_Project
├── Art          UI sprites and the sprite atlas
├── Data         ScriptableObject assets: rewards, wheels, zone wheel table
├── Prefabs      ui_wheel, ui_slice, ui_rewards, ui_reward_item
├── Scenes       MainScene
└── Scripts
    ├── Configs  RewardData, WheelSlice, WheelConfig, ZoneWheelTable
    ├── Game     GameModel, GameController
    ├── Wheel    WheelView, WheelSliceView
    ├── Rewards  RewardsView, RewardItemView
    └── Editor   GameControllerEditor (debug tools)
```

## Architecture

| Class | Responsibility |
| --- | --- |
| `GameModel` | Plain C# game state (no Unity): current zone, collected rewards, zone rules (safe, super, can leave) |
| `GameController` | Connects buttons, model and views; runs the game flow (spin, bomb, leave, restart) |
| `WheelView` | Builds the wheel from a `WheelConfig`, reuses slice objects, spins with DOTween |
| `WheelSliceView` | Shows one slice's icon and amount |
| `RewardsView` / `RewardItemView` | Scrollable list of collected rewards, reusing item objects |
| `WheelConfig` | ScriptableObject: wheel sprite, pointer sprite, slices |
| `RewardData` | ScriptableObject: reward name, icon, bomb flag |
| `ZoneWheelTable` | ScriptableObject: picks the wheel for each zone (custom wheel, or default × zone) |

**Design notes**
- **Model / view separation.** `GameModel` holds the rules and doesn't depend on Unity, so it can be unit tested. Views only display.
- **ScriptableObjects for data.** Rewards, wheels and per-zone wheels are editable in the editor without touching code.
- **Object reuse.** Wheel slices and reward items are created once and reused instead of destroyed and recreated.
- **Editor-time validation.** Wrong setups, such as a bomb on a Silver or Golden wheel or a wrong wheel type for a zone, are reported in the editor through `OnValidate`.
- **UI rules from the brief.** Button references are set in `OnValidate`, click listeners are added in code (no editor OnClick), names go general to specific with `_value` on runtime-changed elements, and Raycast Target and Maskable are off where not needed.

## Editor tools

In Play mode, select `ui_game`. The GameController inspector shows a **Debug** section:
- **Go To Zone**: jump to any zone, for example 5 (safe) or 30 (super).
- **Spin To Bomb**: spin the current wheel so it lands on the bomb.

## Packages

- DOTween (animations)
- TextMeshPro
