using UnityEngine;

public class Movement : MonoBehaviour
{
    public enum CorMoveStateEnum
    {
        None,
        RunOnGround,
        Max
    }
    public enum SpecialMoveStateEnum
    {
        None,
        Max
    }

    public MoveStateMachine<CorMoveStateEnum> corMove;
    public MoveStateMachine<SpecialMoveStateEnum> specialMove;

    [HideInInspector] public Feet feet = null;

    private void Awake()
    {
        corMove = new MoveStateMachine<CorMoveStateEnum>();
        specialMove = new MoveStateMachine<SpecialMoveStateEnum>();

        GetComponent<Player>().move = this;

        feet = GetComponent<Feet>();
    }

    private void Start()
    {
        //init all states
        foreach (var moveState in GetComponents<MoveState<CorMoveStateEnum>>())
        {
            moveState.Init();
        }
        corMove.LateInit();

        foreach (var moveState in GetComponents<MoveState<SpecialMoveStateEnum>>())
        {
            moveState.Init();
        }
        specialMove.LateInit();
    }

    private void FixedUpdate()
    {
        corMove.Update();
    }
}
