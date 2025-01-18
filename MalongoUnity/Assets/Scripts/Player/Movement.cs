using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    #region Enums
    public enum CoreEnum
    {
        None,
        RunOnGround,

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
    InputAction lookAction = null;
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

        lookAction = gm.input.actions.FindAction("Look");
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
        //read the Input
        inputTranslation = moveAction.ReadValue<Vector2>().normalized;
        inputRotation = lookAction.ReadValue<Vector2>();
        
        //RotatteCamera
        Look();
    }

    private void FixedUpdate()
    {
        wishDir = feet.RotateToLocalWorld(new Vector3(inputTranslation.x, 0, inputTranslation.y), false);
        wishDir = feet.RotateToLocalWorld(new Vector3(inputTranslation.x, 0, inputTranslation.y), true);

        //gm.debug.DrawRay("wishDir", Color.red, playerFoward.position, wishDir);
        gm.debug.DrawRay("Velocity", Color.blue, playerFoward.position, rb.linearVelocity);

        feet.CustomUpdate();
        coreMove.Update();
        specialMove.Update();
    }

    void Look()
    {
        // Get input and scale it by sensitivity and frame time
        float mouseX = inputRotation.x * lookSensitivity * Time.deltaTime; // Yaw
        float mouseY = inputRotation.y * lookSensitivity * Time.deltaTime; // Pitch

        // Rotate the yaw (horizontal) using Transform.forward
        transform.Rotate(Vector3.up * mouseX); // Rotate the player (or main object) horizontally

        // Adjust pitch (vertical rotation) on the cameraAnchor
        float currentPitch = cameraAnchor.localEulerAngles.x;
        if (currentPitch > 180f) currentPitch -= 360f; // Normalize pitch to [-180, 180]

        float newPitch = Mathf.Clamp(currentPitch - mouseY, -yawLimit, yawLimit); // Clamp pitch
        cameraAnchor.localRotation = Quaternion.Euler(newPitch, 0f, 0f); // Apply clamped pitch
    }

    public void OnInputLook(InputAction.CallbackContext _context)
    {
        inputRotation = _context.ReadValue<Vector2>();

    }

}
