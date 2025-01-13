using System.Collections.Generic;
using System;

public class Transition<TEnum> where TEnum : Enum
{
    public HashSet<TEnum> From { get; private set; }
    public TEnum To { get; private set; }
    public int Priority { get; private set; }
    public Func<bool> Condition { get; private set; }

    public Transition() { }
    public Transition(IEnumerable<TEnum> from, TEnum to, int priority, Func<bool> condition)
    {
        From = new HashSet<TEnum>(from);
        To = to;
        Priority = priority;
        Condition = condition;
    }
}