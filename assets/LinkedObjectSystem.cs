using UdonSharp;
using UnityEngine;
using UnityEngine.Rendering;
using VRC.SDKBase;
using VRC.Udon;

[DisallowMultipleComponent]
[AddComponentMenu("Artyom Scripting System/Linked Object System")]
public class LinkedObjectSystem : UdonSharpBehaviour
{
    [Header("Script Settings\n")]
    [Tooltip("Print all variables to console for debug purposes")]
    public bool debugMode;
    [Tooltip("Delay for Temporarty interact, ignore if not needed")]
    public float delayedInteractTime = 0f;

    public LayerMask excludeLayers;
    [Header("Interactions Settings\n")]
    [Tooltip("Trigger behavior type")]
    

    public TriggerType triggerType = TriggerType.None;
    [Tooltip("Interact via select type")]
    public InteractionType interactionType = InteractionType.None;

    [Header("Animator Links (bool only)\n")]
    public Animator[] outputAnimators;
    public string[] outputParameterNames; 
    [Space]
    public GameObject audioObjectTrue;
    public GameObject audioObjectFalse;
    // PRIVATE VARS

    private bool playerTriggerEnabled = false;
    private bool pickupTriggerEnabled = false;
    
    [UdonSynced]
    private int isOccupied;
    private float timer;

    void Start()
    {
        isOccupied = 0;
        if (triggerType == TriggerType.BothPlayerAndObject)
        {
            playerTriggerEnabled = true;
            pickupTriggerEnabled = true;
        }
        else if (triggerType == TriggerType.Player)
        {
            playerTriggerEnabled = true;
            pickupTriggerEnabled = false;
        }
        else if (triggerType == TriggerType.Object)
        {
            playerTriggerEnabled = false;
            pickupTriggerEnabled = true;
        }
        if (interactionType == InteractionType.None)
        {
            DisableInteractive = true;
        }
    }
    private void Update()
    {
        if (interactionType == InteractionType.Temporary && isOccupied == 1)
        {
            if (debugMode) Debug.Log("Linked Object System: Interaction Type Checked As Temprary");
            if (timer < delayedInteractTime)
            {
                isOccupied = 1;
                if (debugMode) Debug.LogWarning("LOS: Logged isOccupied as " + isOccupied.ToString());
                timer += Time.deltaTime;
                if (debugMode) Debug.Log("Linked Object System: DeltaTime Update, New Value -> " + timer.ToString());
            }
            else if (timer >= delayedInteractTime)
            {
                isOccupied = 0;
                if (debugMode) Debug.LogWarning("LOS: Logged isOccupied as " + isOccupied.ToString());
                UpdateAnims(false);
                timer = 0f;
            }
        }
    }
      
    private void UpdateAnims(bool state)
    {
        for (int i = 0; i < outputAnimators.Length; i++)
        {
            outputAnimators[i].SetBool(outputParameterNames[i], state);
        }

        // Enable or disable GameObjects based on the state
        if (audioObjectFalse != null && audioObjectTrue != null)
        {
            if (!state)
            {
                if (debugMode) Debug.Log("Linked Object System: GameObject set audio to false");
                audioObjectFalse.SetActive(true);
                audioObjectTrue.SetActive(false);
            }
            else
            {
                if (debugMode) Debug.Log("Linked Object System: GameObject set audio to true");
                audioObjectTrue.SetActive(true);
                audioObjectFalse.SetActive(false);
            }
        }
    } 
    private void OnTriggerEnter(Collider other)
    {
        if (pickupTriggerEnabled && other.GetComponent<VRC_Pickup>())
        {
            isOccupied++;
            if (debugMode) Debug.Log("Linked Object System: Pickup set trigger to " + isOccupied.ToString());
            if (isOccupied > 0 && isOccupied <=2)
            {
                UpdateAnims(true);
            }
        }
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (playerTriggerEnabled)
        {
            isOccupied++;
            if (debugMode) Debug.Log("Linked Object System: Player set trigger to " + isOccupied.ToString());
            if (isOccupied > 0 && isOccupied <=2)
            {
                UpdateAnims(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (pickupTriggerEnabled)
        {
            VRC_Pickup pickup = other.GetComponent<VRC_Pickup>();
            if (pickup != null)
            {
                isOccupied--;
                if (debugMode) Debug.Log("Linked Object System: Pickup set trigger to " + isOccupied.ToString());
                if (isOccupied == 0)
                {
                    UpdateAnims(false);
                }
            }
        }
    }
    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (playerTriggerEnabled)
        {
            isOccupied--;
            if (debugMode) Debug.Log("Linked Object System: Player set trigger to " + isOccupied.ToString());
            if (isOccupied == 0)
            {
                UpdateAnims(false);
            }
        }
    }
    public override void Interact()
    {
        if (interactionType.Equals(InteractionType.Permanent))
        {
            isOccupied++;
            if (isOccupied == 1)
            {
                UpdateAnims(true);
            }
            
        }
        if (interactionType.Equals(InteractionType.Temporary))
        {
            if (isOccupied == 0)
            {
                isOccupied = 1;
                if (debugMode) Debug.LogWarning("LOS: Logged isOccupied as " + isOccupied.ToString());
                UpdateAnims(true);
                UdonBehaviour behaviour = (UdonBehaviour)GetComponent(typeof(UdonBehaviour));
                behaviour.DisableInteractive = true;
                SendCustomEventDelayedSeconds(nameof(DisableTemp), delayedInteractTime+0.2f);
            }
        }
        if (interactionType.Equals(InteractionType.Toggle))
        {
            if (isOccupied == 1) { 
                isOccupied = 0;
                UpdateAnims(false);
            }
            else {
                isOccupied = 1;
                UpdateAnims(true);
            }
        }
    }
    public void DisableTemp()
    {
        UdonBehaviour behaviour = (UdonBehaviour)GetComponent(typeof(UdonBehaviour));
        behaviour.DisableInteractive = false;
        isOccupied = 0;
        if (debugMode) Debug.LogWarning("LOS: Logged isOccupied as " + isOccupied.ToString());
    }
}

public enum InteractionType
{
    None,
    Permanent,
    Temporary,
    Toggle
}
public enum TriggerType
{
    None,
    Player,
    Object,
    BothPlayerAndObject
}