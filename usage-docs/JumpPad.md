## Jump Pad Controller

This script provides a fully functional Jump Pad system for use in VRChat worlds. The Jump Pad propels players or objects towards a target with a smooth arc, complete with optional animation and debug logging.

## Requires:
- **Animator** component (optional but recommended for visual feedback)
- **Colliders** for trigger detection

## Debug - Type: Boolean
- Enables or disables debug messages for troubleshooting.

## Launch Animation - Type: Animator
- The Animator used to play a launch animation.
- **Default Parameter Name:** `isLaunch`

## Jump Target - Type: Transform
- Specifies the target point where the Jump Pad propels the player or object.

## Self - Type: Transform
- The Transform of the Jump Pad itself, used for calculating the jump arc.

## Arc Time - Type: Float
- The duration (in seconds) for the arc trajectory.
  - **Default Value:** `1.2`

## Walk Limit - Type: Float
- The restricted walk speed for players during the jump.
  - **Default Value:** `0.2`

## Run Limit - Type: Float
- The restricted run speed for players during the jump.
  - **Default Value:** `0.4`

## Strafe Limit - Type: Float
- The restricted strafe speed for players during the jump.
  - **Default Value:** `0.2`

---

### Usage Instructions:
1. Attach this script to the Jump Pad GameObject in Unity.
2. Assign the **Jump Target** Transform to define the destination of the jump.
3. (Optional) Assign an **Animator** component and set the animation parameter name if different from the default.
4. Configure the **Arc Time**, **Walk Limit**, **Run Limit**, and **Strafe Limit** as needed.

### Script Behavior:
- When a player or object enters the Jump Pad:
  - Calculates and applies the velocity required to reach the **Jump Target**.
  - Limits the player’s movement for the duration of the jump.
  - Activates the launch animation (if an Animator is assigned).
  - Synchronizes the event using **VRC Udon Networking**.
- Once the jump completes:
  - Restores the player’s movement to default speeds.
  - Deactivates the launch animation.

### Debug Mode:
- Outputs detailed logs to Unity’s console, including calculated velocities and state changes, to assist with testing and fine-tuning.
