using Scripts.Player_Scripts.Player_States;
using UnityEngine;

public class PlayerStateMachine : StateManager<PlayerStateMachine.PlayerState>
{
    public bool isGrounded;
    public float PlayerSpeed = 50f;
    public float groundDrag = 1f;
    public float airDrag = 0f;
    public float airDragSideways = 0.1f;
    public float playerJumpForce = 5f;
    public float jumpCooldown = 0.5f;
    public float jumpTimer = 0f;
    public ConstantForce constantForce;
    public Transform cameraTransform;
    public Vector2 moveDirection;
    public Vector3 Velocity;
    public Rigidbody rb;
    public PlayerState currentPlayerState;
    public InputManager inputManager;
    [SerializeField] private LayerMask groundMask;
    
    
    
    public enum PlayerState
    {
        Idle,
        Walking,
        Falling,
        Jumping,
        Locked
    }
    void Awake()
    {        
        rb = GetComponentInParent<Rigidbody>();
        constantForce = GetComponentInParent<ConstantForce>();
        Velocity = Vector3.zero;
        rb.drag = groundDrag;
        
        
        States[PlayerState.Idle] = new IdleState(PlayerState.Idle, this);
        States[PlayerState.Walking] = new WalkingState(PlayerState.Walking, this);
        States[PlayerState.Falling] = new FallState(PlayerState.Falling, this);
        States[PlayerState.Jumping] = new JumpState(PlayerState.Jumping, this);
        States[PlayerState.Locked] = new LockedState(PlayerState.Locked, this);
        CurrentState = States[PlayerState.Idle];
    }

    
    /*
    void Start()
    {

    }
*/
    void FixedUpdate()
    {
        
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundMask);

        
        PlayerState nextStateKey = CurrentState.GetNextState();
        
        if (!IsTransitioningState&&nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else if (!IsTransitioningState)
        {
            TransitionToState(nextStateKey);
        }
        
        jumpTimer += Time.deltaTime;
        
        ApplyDrag();
        //CheckSaveInputs();
    }

    private void ApplyDrag()
    {
        

        
        if (isGrounded)
        {
            ApplyGroundDrag();
            rb.AddForce(0,-2f,0);
        }
        else
        {
            ApplyAirDragHorizontal();
            ApplyAirDragVertical();
        }

    }

    private void ApplyAirDragVertical()
    {
        Vector3 velocity = rb.velocity;
        Vector3 dragForce = -velocity * airDrag;
        
        dragForce.x = 0;
        dragForce.z = 0;
        
        rb.AddForce(dragForce);
    }

    private void ApplyGroundDrag()
    {
        Vector3 velocity = rb.velocity;
        Vector3 dragForce = -velocity * groundDrag;
        
        dragForce.y = 0;
        
        rb.AddForce(dragForce);
    }

    private void ApplyAirDragHorizontal()
    {
        Vector3 velocity = rb.velocity;
        Vector3 dragForce = -velocity * airDragSideways;

        dragForce.y = 0;
        
        rb.AddForce(dragForce);
    }

    public Vector3 GetMovementDirection()
    {
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0; // Ignore the vertical component
        cameraForward.Normalize();

        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0; // Ignore the vertical component
        cameraRight.Normalize();

        Vector3 targetDirection = (cameraForward * moveDirection.y + cameraRight * moveDirection.x).normalized;

        return targetDirection;
    }


}
