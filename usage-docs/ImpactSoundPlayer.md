## ImpactSoundPlayer

Diegetic sounds can be tough and scaling only volume with impact force doesn't sound right. May as well have three distinct sounds right?

Requires:
- VRC Pickup
- VRC Object Sync
- Rigidbody
- Audio Source attached to GameObject (3)

Small Impact Sound Object - Type: GameObject
- Enables GameObject under the condition that the velocity on impact with another collider is less than the Medium Impact Threshold.
Medium Impact Sound Object - Type: GameObject
- Enables GameObject under the condition that the velocity on impact with another collider is more than the Medium Impact Threshold, but less than the Large Impact Threshold.
Large Impact Sound Object - Type: GameObject
- Enables GameObject under the condition that the velocity on impact with another collider is more than the Large Impact Threshold.
Medium Impact Threshold - Type: Float
- The velocity requirement for a Medium Impact Sound Object to trigger over the Small.
Large Impact Threshold - Type: Float
- The velocity requirement for a Large Impact Sound Object to trigger over the Medium.