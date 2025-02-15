using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    #region Enums
    public enum CoreEnum
    {
        None,
        RunOnGround,
        RunInAir,

        Max
    }
    public CoreEnum coreMoveEnum = CoreEnum.None;
    public CoreEnum backCoreEnum = CoreEnum.None;

    public enum SpecialEnum
    {
        None,
        Jump,

        Max
    }
    public SpecialEnum specialMoveEnum = SpecialEnum.None;
    public SpecialEnum backSpecialMoveEnum = SpecialEnum.None;

    public enum FeetEnum
    {
        OnGround,
        ToSteepGround,
        OffGround,

        Max
    }
    public FeetEnum feetEnum = FeetEnum.OffGround;
    public FeetEnum backFeetEnum = FeetEnum.OffGround;
    #endregion

    GameManager gm = null;
    LevelManager lm = null;

    [HideInInspector] public Rigidbody rb;

    [SerializeField] public Transform playerFoward;
    [SerializeField] public Transform cameraAnchor;
    [SerializeField] float lookSensitivity = 0.6f;
    [SerializeField] float yawLimit = 85;

    public CoreMoveStateMachine coreMove;
    public SpecialMoveStateMachine specialMove;

    [HideInInspector] public Feet feet = null;

    InputAction moveAction = null;
    [HideInInspector] public Vector2 inputTranslation = Vector2.zero;
    [HideInInspector] public Vector2 inputRotation = Vector2.zero;

    [HideInInspector] public Vector3 wishDir = Vector3.zero;

    private void Awake()
    {
        gm = GameManager.Instance;
        lm = LevelManager.Instance;

        GetComponent<Player>().move = this;
        rb = GetComponent<Rigidbody>();

        feet = GetComponent<Feet>();

        coreMove = new CoreMoveStateMachine();
        specialMove = new SpecialMoveStateMachine();

        gm.input.actions.FindAction("Look").performed += OnInputLook;
        moveAction = gm.input.actions.FindAction("Move");
    }

    private void Start()
    {
        //init all states
        foreach (var moveState in GetComponents<CoreMoveState>())
        {
            moveState.Init();
        }
        coreMove.LateInit();

        foreach (var moveState in GetComponents<SpecialMoveState>())
        {
            moveState.Init();
        }
        specialMove.LateInit();
    }

    private void Update()
    {
        gm.DebugLine(false, "OverrideVerticalAxis Velocity", Color.magenta, feet.OverrideVerticalAxis(rb.linearVelocity, true) * 0.5f);

        gm.DebugLine(false, "spring Velocity", Color.red, feet.verticalSpringSpeed * Vector3.up * 0.5f);
        gm.DebugLine(false, "Velocity - OverrideVerticalAxisVelocity ", Color.yellow, (rb.linearVelocity - feet.OverrideVerticalAxis(rb.linearVelocity, true)) * 0.5f);

        gm.DebugLine(false, "spring Velocity + OverrideVerticalAxis", Color.blue, ((feet.verticalSpringSpeed * Vector3.up) + feet.OverrideVerticalAxis(rb.linearVelocity, true)) * 0.5f);
        gm.DebugLine(false, "Velocity", Color.green, rb.linearVelocity * 0.5f);

        gm.DebugLine(false, "wishDir", Color.magenta, wishDir);
        gm.DebugLine(false, "Velocity", Color.blue, rb.linearVelocity * 0.3f);

        //read the Input
        inputTranslation = moveAction.ReadValue<Vector2>().normalized;
        
        //RotatteCamera
        Look();
    }

    private void FixedUpdate()
    {

        wishDir = feet.RotateToLocalWorld(new Vector3(inputTranslation.x, 0, inputTranslation.y), false);



        feet.CustomUpdate();
        coreMove.CustomUpdate();
        specialMove.CustomUpdate();

    }

    void Look()
    {
        // Get input and scale it by sensitivity and frame time
        float mouseX = inputRotation.x * lookSensitivity; // Yaw
        float mouseY = inputRotation.y * lookSensitivity; // Pitch
        //reset after use
        inputRotation = Vector2.zero;

        // Rotate the yaw (horizontal) using Transform.forward
        playerFoward.Rotate(Vector3.up * mouseX); // Rotate the player (or main object) horizontally

        // Adjust pitch (vertical rotation) on the cameraAnchor
        float currentPitch = cameraAnchor.localEulerAngles.x;
        if (currentPitch > 180f) currentPitch -= 360f; // Normalize pitch to [-180, 180]

        float newPitch = Mathf.Clamp(currentPitch - mouseY, -yawLimit, yawLimit); // Clamp pitch
        cameraAnchor.localRotation = Quaternion.Euler(newPitch, 0f, 0f); // Apply clamped pitch
    }

    public void OnInputLook(InputAction.CallbackContext _context)
    {
        inputRotation += _context.ReadValue<Vector2>();
    }

}
