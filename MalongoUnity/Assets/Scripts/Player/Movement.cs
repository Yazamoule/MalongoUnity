using UnityEngine;

public class Movement : MonoBehaviour
{
    public enum CoreMoveEnum
    {
        None,
        RunOnGround,

        Max
    }
    public CoreMoveEnum coreMoveEnum = CoreMoveEnum.None;
    public CoreMoveEnum backCoreEnum = CoreMoveEnum.None;

    public enum SpecialMoveEnum
    {
        None,

        Max
    }
    public SpecialMoveEnum specialMoveEnum = SpecialMoveEnum.None;
    public SpecialMoveEnum backSpecialMoveEnum = SpecialMoveEnum.None;

    public enum FeetStateEnum
    {
        OnGround,
        ToSteepGround,
        OffGround,

        Max
    }
    public FeetStateEnum feetEnum = FeetStateEnum.OffGround;
    public FeetStateEnum backFeetEnum = FeetStateEnum.OffGround;

    [HideInInspector] public Rigidbody rb;

    public CoreMoveStateMachine coreMove;
    public SpecialMoveStateMachine specialMove;

    [HideInInspector] public Feet feet = null;

    private void Awake()
    {
        GetComponent<Player>().move = this; 
        rb = GetComponent<Rigidbody>();

        feet = GetComponent<Feet>();

        coreMove = new CoreMoveStateMachine();
        specialMove = new SpecialMoveStateMachine();
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

    private void FixedUpdate()
    {
        coreMove.Update();
        specialMove.Update();
    }
}
