# Breakout Clone

A classic Breakout-style game created in Unity for the NMD220 course.

## Game Features

- 5 levels with various brick patterns
- Multiple brick types including:
  - Normal bricks
  - Armored bricks (require multiple hits)
  - Exploding bricks (damage nearby bricks)
  - Directional bricks (force ball direction)
- 4 power-ups:
  - Paddle Extend: Increases paddle size temporarily
  - Multi-Ball: Spawns multiple balls
  - Slow Ball: Decreases ball speed temporarily
  - Fast Paddle: Increases paddle movement speed temporarily
- 1 negative power-up:
  - Inverted Controls: Temporarily flips the paddle controls

## Controls

- Move paddle: Left/Right arrow keys or mouse movement
- Launch ball: Space bar
- Restart game: R key

## How to Play

1. Clear all bricks to progress to the next level
2. Avoid letting the ball fall off the screen
3. Collect power-ups to gain advantages
4. Manage your lives (3 lives to start)
5. Complete all 5 levels to win

## Development Details

- Created with Unity 6000.0.x
- Uses 2D physics for ball and paddle movement
- Implements object-oriented design patterns

## Implementation Guide

### Scene Setup
1. Create a new 2D scene in Unity
2. Configure the camera:
   - Position: (0, 0, -10)
   - Size: 5 (orthographic)
   - Background color: Black or dark blue

### Core Game Objects
1. Paddle:
   - Sprite (Square)
   - Scale: (2, 0.5, 1)
   - Position: (0, -4, 0)
   - Components: Box Collider 2D, Rigidbody 2D (Kinematic), PaddleController script
   - Tag: "Paddle"

2. Ball:
   - Sprite (Circle)
   - Scale: (0.5, 0.5, 1)
   - Position: (0, -3.5, 0)
   - Components: Circle Collider 2D, Rigidbody 2D, BallController script
   - Tag: "Ball"

3. Walls:
   - Create LeftWall, RightWall, and TopWall
   - Add Box Collider 2D to each
   - Position at screen edges
   - Set as static

4. Death Zone:
   - Empty GameObject with Box Collider 2D (trigger)
   - Position below screen
   - Tag: "DeathZone"

### Manager Setup
1. GameManager:
   - Add GameManager script
   - Configure UI elements
   - Set up audio source

2. LevelManager:
   - Add LevelManager script
   - Set up level prefabs array
   - Configure audio source

3. PowerUpManager:
   - Add PowerUpManager script
   - Set up power-up prefabs array

### Brick Prefabs
1. Normal Brick:
   - Sprite (Square)
   - Scale: (1, 0.5, 1)
   - Components: Box Collider 2D, Brick script

2. Armored Brick:
   - Based on Normal Brick
   - Use ArmoredBrick script
   - Configure hit points and colors

3. Exploding Brick:
   - Based on Normal Brick
   - Use ExplodingBrick script
   - Set explosion radius and effects

4. Directional Brick:
   - Based on Normal Brick
   - Use DirectionalBrick script
   - Configure bounce direction

### Power-up Prefabs
1. Base Power-up:
   - Sprite (Square)
   - Scale: (0.5, 0.5, 1)
   - Components: Box Collider 2D (trigger), Rigidbody 2D (Kinematic), PowerUp script

2. Specific Power-ups:
   - PaddleExtendPowerUp
   - MultiBallPowerUp
   - SlowBallPowerUp
   - FastPaddlePowerUp
   - InvertControlsPowerUp

### UI Setup
1. Create Canvas (Screen Space - Overlay)
2. Add elements:
   - Score Text (top-left)
   - Lives Text (top-right)
   - Game Over Panel (center)
   - Restart Button

### Physics Configuration
1. Edit > Project Settings > Physics 2D
2. Set up layers:
   - Default
   - Brick
   - Paddle
   - Ball
   - PowerUp
3. Configure collision matrix

### Final Steps
1. Set up Inspector references:
   - Link paddle to BallController
   - Configure power-up prefabs
   - Set up level prefabs
   - Link UI elements

2. Testing checklist:
   - Paddle movement
   - Ball launching
   - Brick destruction
   - Power-up effects
   - Level progression
   - Game over conditions
   - Restart functionality

## Project Structure
```
Assets/
├── Scripts/
│   ├── Bricks/
│   │   ├── Brick.cs
│   │   ├── ArmoredBrick.cs
│   │   ├── ExplodingBrick.cs
│   │   └── DirectionalBrick.cs
│   ├── PowerUps/
│   │   ├── PowerUp.cs
│   │   ├── PaddleExtendPowerUp.cs
│   │   ├── MultiBallPowerUp.cs
│   │   ├── SlowBallPowerUp.cs
│   │   ├── FastPaddlePowerUp.cs
│   │   └── InvertControlsPowerUp.cs
│   ├── BallController.cs
│   ├── PaddleController.cs
│   ├── GameManager.cs
│   ├── LevelManager.cs
│   └── PowerUpManager.cs
├── Prefabs/
│   ├── Bricks/
│   ├── PowerUps/
│   └── Levels/
└── Scenes/
```

## Credits
Created by Mert Biricik for NMD220 