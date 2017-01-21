using UnityEngine;
using System.Collections;

public class CharacterMove : MonoBehaviour
{
    public delegate void ChangeFloat(float newFloat);
    public delegate void GeneralEvent();

    public event ChangeFloat OnChangedDirection;
    private float oldDirection;
    public event GeneralEvent OnJump;

    [Header("Movement")]
    [Tooltip("The horizontal move speed (m/s).")]
    public float moveSpeed = 2f;

    [Tooltip("The rate at which the character accelerates to reach the move speed (m/s^2).")]
    public float acceleration = 1f;

    [Space()]
    [Tooltip("The maximum angle at which a slope is considered walkable.")]
    public float slopeLimit = 50f;

    [HideInInspector]
    public float inputDirection = 0f;

    [HideInInspector]
    public bool canMove = true;

    [Header("Jumping")]
    [Tooltip("Indirectly controls jump height (this force is applied every fram jump is held, but in smaller and smaller fractions.")]
    public float jumpForce = 10f;

    [Tooltip("How long the jump button can be held for before the character starts falling.")]
    public float jumpTime = 1f;
    private float jumpHeldTime = 0;

    [Space()]
    [Tooltip("How long after the character leaves the ground until they can no longer jump (recommended to have this delay for platformers).")]
    public float stopJumpDelay = 0.02f;
    private float stopJumpTime;

    private bool shouldJump = false;
    private bool heldJump = false;

    [HideInInspector]
    public bool isGrounded = false;

    [Header("Physics")]
    [Tooltip("The maximum speed at which the character can fall (otherwise known as terminal velocity).")]
    public float maxFallSpeed = 20f;

    [Space()]
    public LayerMask groundLayer;

    [Space()]
    public Vector2 circleCastOrigin;
    public float circleCastRadius = 1f;
    public float groundedDistance = 0.1f;

    private Collider2D col;
    private Rect box;
    private Rigidbody2D body;

    private CharacterSound characterSound;

    [HideInInspector]
    public Vector2 velocity;

    private void Awake()
    {
        //Get references
        col = GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
        characterSound = GetComponent<CharacterSound>();
    }

    private void Update()
    {
        //Get velocity, capping fall speed
        velocity = body.velocity;
        velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);

        RaycastHit2D hit = Physics2D.CircleCast((Vector2)transform.position + circleCastOrigin, circleCastRadius, Vector2.down, Mathf.Infinity, groundLayer);

        if (hit.collider != null)
        {
            Vector2 start = (Vector2)transform.position + circleCastOrigin;
            start.y -= circleCastRadius;

            float distance = Vector3.Distance(start, hit.point);

            if (distance <= groundedDistance)
                isGrounded = true;
            else
                isGrounded = false;
        }
        else
            isGrounded = false;

        //Jumping
        {
            if (isGrounded)
                stopJumpTime = Time.time + stopJumpDelay;

            //If jump button has been pressed
            if (shouldJump)
            {
                //Consume jump button flag
                shouldJump = false;

                //If jump button was pressed within a small amount of time after leaving the ground (or still on ground)
                if (Time.time <= stopJumpTime)
                {
                    //Call jump events
                    if (OnJump != null)
                        OnJump();

                    //Reset jump held time, allowing jump to be held
                    jumpHeldTime = 0;
                }
            }

            //If jump has been held for less than the max time
            if (heldJump && jumpHeldTime < jumpTime)
            {
                //Set velocity, slowly decreasing to zero over time
                velocity.y = Mathf.Lerp(jumpForce, 0, jumpHeldTime / jumpTime);

                //Count time jump is held
                jumpHeldTime += Time.deltaTime;
            }
            else //If jump has been released, at can not be held again until a new jump is started
                jumpHeldTime = jumpTime;
        }

        //Horizontal movement
        if (canMove)
            velocity.x = Mathf.Lerp(velocity.x, moveSpeed * inputDirection, acceleration * Time.deltaTime);

        if (inputDirection != 0 && isGrounded)
            characterSound.PlayWalkSound();
        else
            characterSound.StopWalkSound();

        //Player can not leave camera from left (camera doesn't move left)
        float leftCameraPoint = Camera.main.transform.position.x - (Camera.main.orthographicSize * Screen.width / Screen.height);

        if (transform.position.x < leftCameraPoint && velocity.x < 0)
            velocity.x = 0;

        //Move character by velocity
        //transform.Translate(velocity * Time.deltaTime);
        body.velocity = velocity;
    }


    public void Move(float direction)
    {
        //Cache old direction for comparison
        oldDirection = inputDirection;

        inputDirection = direction;

        //If direction has changed (and does not equal 0), then call changed direction event
        if (inputDirection != oldDirection && direction != 0 && OnChangedDirection != null)
            OnChangedDirection((direction >= 0 ? 1 : -1));
    }

    public void Jump(bool pressed)
    {
        if (pressed)
            shouldJump = true;

        heldJump = pressed;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + circleCastOrigin, circleCastRadius);
    }
}