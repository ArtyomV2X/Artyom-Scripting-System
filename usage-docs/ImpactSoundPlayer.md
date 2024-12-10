## Impact Sound Player

Diegetic sounds can be tough and scaling only volume with impact force doesn't sound right. May as well have three distinct sounds, right?

## Requires:
- **VRC Pickup**
- **VRC Object Sync**
- **Rigidbody**
- **Audio Source attached to GameObject (3)**

## Small Impact Sound Object - Type: GameObject
- Enables the GameObject under the condition that the velocity on impact with another collider is less than the **Medium Impact Threshold**.

## Medium Impact Sound Object - Type: GameObject
- Enables the GameObject under the condition that the velocity on impact with another collider is more than the **Medium Impact Threshold**, but less than the **Large Impact Threshold**.

## Large Impact Sound Object - Type: GameObject
- Enables the GameObject under the condition that the velocity on impact with another collider is more than the **Large Impact Threshold**.

## Medium Impact Threshold - Type: Float
- The velocity requirement for a **Medium Impact Sound Object** to trigger instead of a **Small Impact Sound Object**.
  
- **Default Value:** `5.0`

## Large Impact Threshold - Type: Float
- The velocity requirement for a **Large Impact Sound Object** to trigger instead of a **Medium Impact Sound Object**.
  
- **Default Value:** `10.0`

---

### Usage Instructions:
1. Attach this script to a GameObject in Unity.
2. Assign the respective GameObjects for the **Small**, **Medium**, and **Large Impact Sound Objects**.
3. Set the desired thresholds for **Medium Impact Threshold** and **Large Impact Threshold**.
4. Ensure the GameObject has the required components: **Rigidbody**, **VRC Pickup**, and **VRC Object Sync**.

### Script Behavior:
- When a collision occurs, the script evaluates the impact velocity:
  - If the velocity is **less than the Medium Impact Threshold**, the **Small Impact Sound Object** is activated.
  - If the velocity is **between the Medium and Large Impact Thresholds**, the **Medium Impact Sound Object** is activated.
  - If the velocity is **greater than the Large Impact Threshold**, the **Large Impact Sound Object** is activated.
