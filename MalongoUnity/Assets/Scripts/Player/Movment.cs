using UnityEngine;

public class Movement : MonoBehaviour
{
    public enum CorMovStateEnum
    {
        NoForce,
        RunOnGround,
        RunInAir,
        Swim,
        Max
    }
    public enum SpecialMovState
    {
        Slide,
        Jump,
        Dash,
        Grapel,
        WallRun,
        NoneMax // no movment and MaxLenth Of the enum
    }

    public MoveStateMachine<CorMovStateEnum> corMov;
    public MoveStateMachine<SpecialMovState> specialMov;

    public Feet feet = null;

    private void Awake()
    {
        corMov = new MoveStateMachine<CorMovStateEnum>();
        specialMov = new MoveStateMachine<SpecialMovState>();

        feet = GetComponent<Feet>();
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        corMov.Update();
        
    }
}
