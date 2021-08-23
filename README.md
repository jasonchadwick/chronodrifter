# chronodrifter

## https://jasonchadwick.github.io/chronodrifter/

Platform puzzler where the direction of time flow can change.

| ![towerjump-gif](Assets/ExampleMedia/tower.gif) |
|:--:|
|Player getting on top of a tower of blocks by knocking them over and then reversing time.|

## TODO
- player can grab and move objects
  - lots of potential to use this with time reversal
- player animations
- player death animation/effect
- time freeze just slows it down to 0.1 or something
  - hard to do in reverse
- button-operated-laser
  - can set up traps for enemies
- enemies (generally not affected by time reversal)
  - static turret or mobile enemy that shoots a ray that freezes you in time (or reverses you!)
  - most simple: bomb robot that rolls after you and explodes (CURRENTLY IN PROGRESS)
  - intelligent enemy that has the same time reversing powers as you
    - Many options for what the AI could look like here
- more levels and obstacles
- more music/sounds
  - cube collision sounds?
  - pillar sounds
- better UI (buttons, main menu etc)
- destructible objects (reassemble when inverted)
- flowing liquids?
- TimeReversibleObject class - make a smaller class, maybe in Utils, that this uses
  - stack of any arbitrary object + nIters
  - functions: GetLastState() and PushState()
  - it takes care of nIters++/--
  - use for Square, MovingPillar
  - could use special case for elevator? store velocity, auto set nIters to infinity?