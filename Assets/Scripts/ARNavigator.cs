using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARNavigator : MonoBehaviour {

    enum MoveState {
        Grounded,
        Jumping
    }

    //public static ARNavigator activeNavigator;
    private static Vector3 destination;
    public static Vector3 Destination {
        get {
            return destination;
        }
        set {
            destination = value;
            movingToDestination = true;
        }
    }
    public static bool exiting = false;

    public float speed = 0.01f;
    public Transform gfx;
    [Tooltip("The Raycast Position should be parented to this object and offset on the Z axis")]
    public Transform raycastOrigin;
    [Tooltip("The position from which a ray will be cast downward to check for the ground")]
    public Transform raycastPosition;
    public float raycastLength = 0.1f;
    public ARRaycastManager raycaster;

    [HideInInspector] public PokemonSpawner mySpawner;
    [HideInInspector] public Animator anim;

    public TMPro.TextMeshPro debugText;

    private static bool movingToDestination = false;
    private MoveState state;
    private Vector3[] jumpPoints = new Vector3[3];
    private float jumpTime = 1f;
    private float jumpTimeElapsed = 0f;

    protected void Reset() {
        raycaster = FindObjectOfType<ARRaycastManager>();
    }

    protected void Awake() {
        if(raycaster == null) {
            raycaster = FindObjectOfType<ARRaycastManager>();
        }
        anim = gfx.GetComponentInChildren<Animator>();
    }

    private void OnEnable() {
        state = MoveState.Grounded;
        movingToDestination = false;
        anim.SetBool("Active", true);
        exiting = false;
        //Vector3 eulers = transform.localEulerAngles;
        //eulers.x = 0;
        //eulers.z = 0;
        //transform.localEulerAngles = eulers;
    }

    protected void Update() {

        //debugText.text = state.ToString();
        //lr1.SetPositions(new Vector3[] { transform.position, Destination });
        //lr2.SetPositions(jumpPoints);

        // Store position at beginning of frame to get speed later
        Vector3 pos1 = transform.localPosition;

        if(movingToDestination) {
            switch(state) {
                case MoveState.Grounded:
                    MoveGrounded();
                    break;
                case MoveState.Jumping:
                    MoveJumping();
                    break;
            }
        }

        // Calculate speed, set animation
        Vector3 pos2 = transform.localPosition;
        Vector3 diff = pos2 - pos1;
        float hDiff = new Vector2(diff.x, diff.z).sqrMagnitude;
        float vDiff = diff.y * diff.y;
        anim.SetFloat("hSpeed", hDiff);
        anim.SetFloat("vSpeed", vDiff);
        if(diff.y < 0) anim.SetBool("Jumping Up", false);
    }

    private void OnDrawGizmos() {
        if(raycastPosition == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(raycastPosition.position, Mathf.Min(raycastLength / 5, 0.025f));
        Gizmos.DrawLine(raycastPosition.position, raycastPosition.position + Vector3.down * raycastLength);
    }

    private void MoveGrounded() {

        // Check if we're at the destination
        Vector2 tPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 dPos = new Vector2(Destination.x, Destination.z);
        if(Vector2.Distance(tPos, dPos) < speed) {
            if(Mathf.Abs(Destination.y - transform.position.y) < speed * 3) {
                transform.position = Destination;
                movingToDestination = false;
                if(exiting) {
                    anim.SetBool("Active", false);
                }
            } else {
                // If we've gotten to the XZ coords but are 
                //some vertical distance off, just jump because idk
                StartJump();
                Debug.Log("JUMP REASON: Move finished");
            }
            return;
        }

        // Position our raycast point
        Vector3 toDestination = Destination - transform.position;
        Vector3 newForward = toDestination;
        newForward.y = 0;
        newForward.Normalize();
        raycastOrigin.forward = newForward;
        Vector3 newPos = transform.position;

        // Test for gaps, but only if we're not right next to the destination
        if(toDestination.sqrMagnitude > (raycastPosition.position - transform.position).sqrMagnitude) {
            Ray ray = new Ray(raycastPosition.position, Vector3.down);
            List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
            if(raycaster.Raycast(ray, hitResults)) {
                //Debug.Log("Distance to raycast: " + hitResults[0].distance);
                if(hitResults[0].distance > raycastLength) {
                    StartJump();
                    Debug.Log("JUMP REASON: Next plane is far below");
                    return;
                } else {
                    newPos.y = hitResults[0].pose.position.y;
                }
            } else {
                StartJump();
                Debug.Log("JUMP REASON: No plane below");
                return;
            }
        }

        // Move towards destination, rotate
        gfx.forward = Vector3.Slerp(gfx.forward, newForward, .2f);
        newPos += newForward * speed;
        transform.position = newPos;
    }

    private void MoveJumping() {
        jumpTimeElapsed = Mathf.Min(jumpTime, jumpTimeElapsed + Time.deltaTime);
        float t = jumpTimeElapsed / jumpTime;
        Debug.Log("Jumping... " + t);
        transform.position = Vector3.Lerp(
            Vector3.Lerp(jumpPoints[0], jumpPoints[1], t),
            Vector3.Lerp(jumpPoints[1], jumpPoints[2], t),
            t);
        if(jumpTimeElapsed == jumpTime) {
            Debug.Log("FINISHED JUMP");
            anim.SetTrigger("Land");
            state = MoveState.Grounded;
        }
    }

    private void StartJump() {
        anim.SetBool("Jumping Up", true);
        state = MoveState.Jumping;
        jumpTimeElapsed = 0f;
        // Create the jump curve
        jumpPoints[0] = transform.position;
        jumpPoints[2] = Destination;
        jumpPoints[1] = (jumpPoints[0] + jumpPoints[2]) / 2f;
        float extraHeight = 0.1f;
        jumpPoints[1].y = (jumpPoints[0].y > jumpPoints[2].y ? jumpPoints[0].y: jumpPoints[2].y) + extraHeight;
        Vector3 newForward = jumpPoints[2] - jumpPoints[0];
        newForward.y = 0;
        gfx.transform.forward = newForward.normalized;
    }

    public static void Exit(Vector3 cardPosition) {
        Destination = cardPosition;
        exiting = true;
    }

    public void OnExitAnimFinish() {
        Debug.Log("ARNavigator - ONEXITANIMFINISH CALLED");
        exiting = false;
        mySpawner.OnPokemonRecalled();
        gameObject.SetActive(false);
    }

}
