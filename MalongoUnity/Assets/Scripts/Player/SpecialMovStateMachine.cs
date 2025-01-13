using UnityEngine;

public class SpecialMovStateMachine
{


    public enum SpecialMovState
    {
        Slide,
        Jump,
        Dash,
        Grapel,
        WallRun,
        NoneMax // no movment and MaxLenth Of the enum
    }
    public SpecialMovState specialMovState;
}
