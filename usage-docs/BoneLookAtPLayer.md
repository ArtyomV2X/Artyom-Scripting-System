# BoneLookAtPlayer
Intended as a rotation for objects like cameras, NPCs, or other items that should face the nearest player.

## Requires:
- Nothing

## Object To Rotate - Type: GameObject
- Takes a bone GameObject (or any other GameObject for all I care) and sets it as the target to rotate.
Rotation Speed - Type: Float
- Limits the rotation speed to this value in degrees per second.
Rotation Offset - Type: Vector3
- In the case that the bone is offset from the root object and needs an offset, the target point will be offset by this amount in degrees per axis of transform.