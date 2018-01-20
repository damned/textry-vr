using System;
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

    private static Dictionary<KnobHandlingState, bool> handlingStateFades = new Dictionary<KnobHandlingState, bool>()
    {
        { KnobHandlingState.Unhandled, true },
        { KnobHandlingState.Touched, false },
        { KnobHandlingState.Grabbed, false }
    };

    public Color ColorForState(KnobHandlingState handlingState, bool faded, float fadeLevel)
    {
        var baseColor = handlingStateColors[handlingState];
        if (ShouldFade(handlingState, faded))
        {
            return FadedColorOf(fadeLevel, baseColor);
        }
        return baseColor;
    }

    private static bool ShouldFade(KnobHandlingState handlingState, bool faded)
    {
        return faded && handlingStateFades[handlingState];
    }

    private static Color FadedColorOf(float fadeLevel, Color baseColor)
    {
        return new Color(baseColor.r, baseColor.g, baseColor.b, fadeLevel);
    }
}