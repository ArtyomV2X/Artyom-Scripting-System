## Interact Script

**Important:** This script is marked as **DEPRECATED** and has been replaced with the **Linked Object System**. It is advised to use the newer system for future development.

## Requires:
- **VRC Pickup** (for ownership management)
- **VRC Object Sync**
- **Animator** component attached to GameObjects

## Animators - Type: Array of Animators
- Holds the list of animators to manipulate based on the interaction type.

## Parameter Names - Type: Array of Strings
- Defines the parameter names in the Animators corresponding to interaction states.

## Interaction Type - Type: Enum
- Specifies the type of interaction:
  - **Temporary:** Activates state for a fixed duration.
  - **Permanent:** Sets the state permanently.
  - **Toggle:** Toggles between active and inactive states.

## Temporary Duration - Type: Float
- The duration (in seconds) for which the state is active in a **Temporary** interaction.
  - **Default Value:** `2.0`

## Audio Object True - Type: GameObject
- The GameObject to activate when the state changes to true.

## Audio Object False - Type: GameObject
- The GameObject to activate when the state changes to false.

---

### Usage Instructions:
1. Attach this script to a GameObject in Unity.
2. Assign the desired **Animators** and corresponding **Parameter Names**.
3. Set the **Interaction Type** according to the desired behavior.
4. Specify additional parameters such as **Temporary Duration**, **Audio Object True**, and **Audio Object False**.

### Script Behavior:
- Upon interaction:
  - Depending on the **Interaction Type**, the state of the GameObject is updated:
    - **Temporary:** Activates the state and reverts it after the specified duration.
    - **Permanent:** Activates the state indefinitely.
    - **Toggle:** Switches between active and inactive states.
  - Synchronizes the state across the network via **VRC Udon Networking**.

**Note:** As this script is deprecated, transitioning to the **Linked Object System** is strongly recommended to ensure compatibility with future updates.
