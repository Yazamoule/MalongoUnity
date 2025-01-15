using System.Collections.Generic;
using System;
using static Movement;

public class Transition<TEnum> where TEnum : Enum
{
    Movement move;

    public HashSet<CoreMoveEnum> fromCor = null;
    public HashSet<SpecialMoveEnum> fromSpecial = null;
    //public HashSet<SpecialMoveStateEnum> fromFeet = null;
    public TEnum to;
    public int priority;
    private Func<bool> OriginalCondition;

    public Transition(TEnum _to, int _priority, Func<bool> _condition, IEnumerable<CoreMoveEnum> _fromCore = null, IEnumerable<SpecialMoveEnum> _fromSpecial = null)
    {
        //ref of movement to get currentstates at runtime
        move = LevelManager.Instance.player.move;


        if (_fromCore != null)
        fromCor = new HashSet<CoreMoveEnum>(_fromCore);

        if (_fromSpecial != null)
        fromSpecial = new HashSet<SpecialMoveEnum>(_fromSpecial);

        to = _to;

        priority = _priority;

        OriginalCondition = _condition;
    }

    public bool Condition()
    {
        if (!(fromCor == null || fromCor.Contains(move.coreMoveEnum)))
            return false;
        
        if (!(fromSpecial == null || fromSpecial.Contains(move.specialMoveEnum)))
            return false;

        return OriginalCondition();
    }
}