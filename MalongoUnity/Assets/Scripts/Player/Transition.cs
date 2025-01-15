using System.Collections.Generic;
using System;
using static Movement;
using System.Linq;

public class Transition<TEnum> where TEnum : Enum
{
    Movement move;

    public HashSet<CorMoveStateEnum> fromCor = null;
    public HashSet<SpecialMoveStateEnum> fromSpecial = null;
    //public HashSet<SpecialMoveStateEnum> fromSpecialMove;
    public TEnum to;
    public int priority;
    private Func<bool> OriginalCondition;

    public Transition(IEnumerable<CorMoveStateEnum> _fromCore, IEnumerable<SpecialMoveStateEnum> _fromSpecial, TEnum _to, int _priority, Func<bool> _condition)
    {
        //ref of movement to get currentstates at runtime
        move = LevelManager.Instance.player.move;


        if (_fromCore != null)
        fromCor = new HashSet<CorMoveStateEnum>(_fromCore);

        if (_fromSpecial != null)
        fromSpecial = new HashSet<SpecialMoveStateEnum>(_fromSpecial);

        to = _to;

        priority = _priority;

        OriginalCondition = _condition;
    }

    public bool Condition()
    {
        if (!(fromCor == null || fromCor.Contains(move.corMove.currentStateEnum)))
            return false;
        
        if (!(fromSpecial == null || fromSpecial.Contains(move.specialMove.currentStateEnum)))
            return false;

        return OriginalCondition();
    }
}