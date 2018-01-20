using System.Collections.Generic;
using UnityEngine;

public class KnobColorArbiter
{
    private static Dictionary<KnobHandlingState, Color> handlingStateColors = new Dictionary<KnobHandlingState, Color>()
    {
        { KnobHandlingState.Unhandled, Color.yellow },
        { KnobHandlingState.Touched, Color.black },
        { KnobHandlingState.Grabbed, Color.red }
    };

    public KnobColorArbiter()
    {
    }

    public Color ColorForState(KnobHandlingState handlingState)
    {
        return handlingStateColors[handlingState];
    }

}