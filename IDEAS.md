# Chronodrifter ideas

### Levels
- massless cubes floating in space, need to float downwards on one of them to get to a button, then reverse back up
- Construct a tower or bridge and then destroy it before an enemy can follow
- Break a destructible object, then reassemble to stop enemies
- Push a cube over somewhere else with you, do some task that fills in lava on the ground behind the cube, then ride the cube back
- Enemy with freeze ray that will repeatedly freeze you when you are in their vision
	- Need to set up some sequence of events that will push you to safety
- Some bad thing happens after a certain amount of (forward) time, need to get through to the end of the level without using too much forward time

### New objects/mechanics
- Instead of freezing time fully, slows down by 10x or something
	- Would feel more fluid and cool
	- A bit hard to do because would need to interpolate 10x, but would probably be fine
- Portals
- Projectiles that you can shoot, then reverse time to make enemy hit it
	- Tenet inverse bullets
	- Laser that is on or off at each timestep, can reverse it to replay on/off states
- Destructible objects
	- reassemble in reverse
- Suicide bomber bot
	- circular bot that constantly rolls towards the player
	- special variant: cannot be time-reversed? Or maybe none of them can be time reversed
		- What happens if you reverse time after it explodes? Could be a cool mechanic - you have to let it explode to break through a barrier, but then need to rebuild the barrier behind you because something is following you?
- Liquids (water and lava)
	- computationally hard to reverse
- Drag/carry objects with E
	- Save object's position relative to player, lerp to it
	- Player.mass += object.mass while carrying
- Implement a general control/signal framework to make doors, buttons, levers etc easier to create and use
	- i.e. each ControlObject (button, lever) can have one (or more?) targets. Each TargetObject (door, piston, cube dropper) has a "powered" variable that the controller can set or unset.

### Bosses
- Enemy that is not affected by time reversal
- Boss that can control time (maybe removes your time control powers too?)
	- Can have various levels of it: maybe one can only pause time, harder one can reverse it too
	- Can they reverse the player?
	- Would need a good AI (@gabe???)