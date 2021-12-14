# Chronodrifter ideas
(ideas in **bold** are either urgent/easy to add or hard but really cool)

### General
- **player animations**
	- idle, moving, death
- **death screen (instead of just immediately restarting)?**
	- OR: when you die, quickly reverses all the time that you used and then restarts the level
- **sounds - cube collision? player jump/landing, death, etc. Door opening, button being pressed...**
- original music (current music is a free Unity asset)? Not necessary but would be cool
- some sort of backstory/motivation? or even just a more distinct theme or personality, like how Portal has GLaDOS
	- a goal? something you are working towards?
- can the player get more powerful over time? new abilities etc?
- **better UI for main menu, level screen, popups**
- add parent class to TimeReversibleObject
  - stack of any arbitrary object positions, stack size = `nIters`
  - functions: GetLastState() and PushState()
  - it takes care of `nIters`++/--
  - use for Square, MovingPillar
  - could use special case for objects with no start/end? (i.e. elevator) auto set nIters to infinity?
  - automatically don't add to stack if object has not changed state
	- instead, increment delta time of top stack element
- **some clear visual distinction between time-reversible and non-time-reversible objects**
	- maybe time-reversible objects glow?

### New levels
- massless cubes floating in space, need to float downwards on one of them to get to a button, then reverse back up
- Construct a tower or bridge and then destroy it before an enemy can follow
- Break a destructible object, then reassemble to stop enemies
- **Push a cube over the floor with you, do some task that removes the floor behind you, then ride the cube back**
	- would need the bridge/floor itself to not be time-reversible
- Some bad thing happens after a certain amount of (forward) time, need to get through to the end of the level without using too much net-forward-time

### New objects/mechanics
- Instead of freezing time fully, slows down by 10x or something
	- Would feel more fluid and cool
	- A bit hard to do because would need to interpolate 10x, but would probably be fine
	- how to add to object stack when moving slowly? Would adding every 10th work? Or would it not be effectively recreated when played back at full speed again?
- Destructible objects
	- reassemble in reverse
- Liquids (water and lava)
	- computationally hard to reverse
- **Drag/carry objects with E**
	- Save object's position relative to player, lerp to it
	- Player.mass += object.mass while carrying
- **Implement a general control/signal framework to make doors, buttons, levers etc easier to create and use**
	- i.e. each `ControlObject` (button, lever) can have one (or more?) targets. Each `ActivatedObject` (door, piston, cube dropper) has a "powered" variable that the controller can set or unset.
- Portals
	- probably can't make a portal gun because that's too close to Portal, but can have teleportation gateways (that objects/projectiles can go through as well)
	- could be an `ActivatedObject` that (sometimes) needs a button/lever
- Projectiles that you can shoot, then reverse time to make enemy hit it
	- i.e. Tenet inverse bullets
	- Stationary laser (`ActivatedObject`) that is on or off at each timestep, can reverse it
	- Many cool puzzles that could involve this
- **HARD IDEA: Can split yourself into multiple timelines to accomplish multiple tasks???**
	- would move forward and do things, then reverse self, then when you resume forward-ness you are on a new timeline instead of the same one. So your old self still does what you did last time, but now you can do something else in parallel. Would need a key to "collapse timelines" and get rid of the parallel versions of you
	- would this be too hard/not-fun to think about when playing?

### New enemies
- Enemy with freeze ray that will repeatedly freeze you when you are in their vision
	- could be static turret or moving enemy
	- Need to set up some sequence of events that will push you to safety
- **Suicide bomber bot**
	- circular bot that constantly rolls towards the player
	- special variant: cannot be time-reversed? Or maybe none of them can be time reversed
		- If it can be reversed: what happens if you reverse time after it explodes? Does it regenerate?
		- If it cannot be reversed: can the explosion be reversed?
- Enemy/boss that is not affected by time reversal
- **Boss that can control time (maybe removes your time control powers too?)**
	- Can have various levels of it: maybe one can only pause time, harder one can reverse it too
	- Can they reverse the player?
	- Would need a good AI (@gabe???)
