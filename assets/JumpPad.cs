
using System.Numerics;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;
using Vector3 = UnityEngine.Vector3;

[DisallowMultipleComponent]

[AddComponentMenu("Artyom Scripting System/Jump Pad Controller")]
public class JumpPad : UdonSharpBehaviour
{
    public bool debug;
    public Animator launchAnim;
    public string animName = "isLaunch";
    public Transform jumpTarget;
    public Transform self;
    public float arcTime = 1.2f;
    public float walkLimit = 0.2f;
    public float runLimit = 0.4f;
    public float strafeLimit = 0.2f;
    private Vector3 appliedVelocity;

    private float defaultWalk;
    private float defaultStrafe;
    private float defaultRun;

    private VRCPlayerApi player;


    void Start()
    {
        launchAnim = GetComponent<Animator>();
        appliedVelocity = CalculateInitialVelocity(self.position, jumpTarget.position, -9.8f, arcTime);
        if (debug) Debug.Log("Jump Pad Controller: Velocity Calculated to:"+ appliedVelocity.ToString()); 
    }
    
    public static Vector3 CalculateInitialVelocity(Vector3 origin, Vector3 target, float gravity, float time)
    {
        // Compute the differences in position
        Vector3 displacement = target - origin;

        // Calculate the initial velocity components
        Vector3 initialVelocity = new Vector3(
            displacement.x / time,                              // Horizontal X velocity
            (displacement.y - 0.5f * gravity * time * time) / time, // Vertical Y velocity
            displacement.z / time                               // Horizontal Z velocity
        );

        return initialVelocity;
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi colidedPlayer) {
        appliedVelocity = CalculateInitialVelocity(self.position, jumpTarget.position, -9.8f, arcTime);
        launchAnim.SetBool(animName, true);
        if (debug) Debug.Log("Jump Pad Controller: Animator State attempted to be set to True");
        player = colidedPlayer;
        // Get speeds to reinstate after jump imobilization
        defaultRun = player.GetRunSpeed();
        defaultStrafe = player.GetStrafeSpeed();
        defaultWalk = player.GetWalkSpeed();
        // Set current velocity to triggered player and limit their movement
        LimitMobility();
        player.SetVelocity(appliedVelocity);
        
        if (debug) Debug.Log("Jump Pad Controller: Player Velocity Set to:"+ appliedVelocity.ToString());
        SendCustomEventDelayedSeconds(nameof(Mobilize), arcTime-0.2f);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        appliedVelocity = CalculateInitialVelocity(self.position, jumpTarget.position, -9.8f, arcTime);
        VRC_Pickup pickup = other.GetComponent<VRC_Pickup>();
        Rigidbody body = other.GetComponent<Rigidbody>();
        if (pickup && body)
        {
            pickup.Drop();
            body.velocity = appliedVelocity;
        }
    }

    public void LimitMobility()
    {
        player.SetWalkSpeed(walkLimit);
        player.SetRunSpeed(runLimit);
        player.SetStrafeSpeed(strafeLimit);
        if (debug) Debug.Log("Jump Pad Controller: Limited Player Mobility for " + arcTime.ToString() + " seconds");
    }
    public void Mobilize()
    {
        player.SetWalkSpeed(defaultWalk);
        player.SetRunSpeed(defaultRun);
        player.SetStrafeSpeed(defaultStrafe);
        if (debug) Debug.Log("Jump Pad Controller: De-Limited Player Mobility after" + arcTime.ToString() + " seconds");
        launchAnim.SetBool(animName,false);
        if (debug) Debug.Log("Jump Pad Controller: Animator State attempted to be set to False");
    }
}
