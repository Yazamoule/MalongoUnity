using System.Collections.Generic;
using System;
using static Movement;

public class Transition<TEnum> where TEnum : Enum
{
    Movement move;

    public HashSet<CoreEnum> fromCor = null;
    public HashSet<SpecialEnum> fromSpecial = null;
    public HashSet<FeetEnum> fromFeet = null;
    public TEnum to;
    public int priority;
    private Func<bool> OriginalCondition;

    public Transition(TEnum _to, int _priority, Func<bool> _condition, IEnumerable<FeetEnum> _fromFeet = null, IEnumerable<CoreEnum> _fromCore = null, IEnumerable<SpecialEnum> _fromSpecial = null)
    {
        //ref of movement to get currentstates at runtime
        move = LevelManager.Instance.player.move;


        if (_fromCore != null)
        fromCor = new HashSet<CoreEnum>(_fromCore);

        if (_fromSpecial != null)
        fromSpecial = new HashSet<SpecialEnum>(_fromSpecial);

        if (_fromFeet != null)
            fromFeet = new HashSet<FeetEnum>(_fromFeet);


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

        if (!(fromFeet == null || fromFeet.Contains(move.feetEnum)))
            return false;

        return OriginalCondition();
    }
}