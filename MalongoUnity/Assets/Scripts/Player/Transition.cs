using System.Collections.Generic;
using System;
using static Movement;
using System.Linq;

public class Transition<TEnum> where TEnum : Enum
{
    Movement move;

    public HashSet<CorMoveStateEnum> fromCorMove;
    public HashSet<SpecialMoveStateEnum> fromSpecialMove;
    //public HashSet<SpecialMoveStateEnum> fromSpecialMove;
    public TEnum to;
    public int priority;
    private Func<bool> OriginalCondition;

    public Transition(IEnumerable<CorMoveStateEnum> fromCore, IEnumerable<SpecialMoveStateEnum> fromSpecialMove, TEnum to, int priority, Func<bool> condition)
    {
        move = LevelManager.Instance.player.move;

        fromCorMove = new HashSet<CorMoveStateEnum>(fromCore);
        this.fromSpecialMove = new HashSet<SpecialMoveStateEnum>(fromSpecialMove);
        this.to = to;
        this.priority = priority;
        OriginalCondition = condition;
    }

    public bool Condition()
    {
        if (!(fromCorMove == null || fromCorMove.Contains(move.corMove.currentStateEnum)))
            return false;
        
        if (!(fromSpecialMove == null || fromSpecialMove.Contains(move.specialMove.currentStateEnum)))
            return false;

        return OriginalCondition();
    }
}