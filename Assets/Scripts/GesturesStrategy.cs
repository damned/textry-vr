using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GesturesStrategy
{
    private readonly Knobs knobs;
    private readonly IDebug debug;
    private readonly KnobArranger knobArranger;

    private List<Knob> wordKnobs = new List<Knob>();

    private readonly Gestures gestures;
    private readonly List<string> words = new List<string>();

    public GesturesStrategy(Gestures gestures, KnobArranger knobArranger, IDebug debug)
    {
        this.knobArranger = knobArranger;
        this.debug = debug;
        this.gestures = gestures;

        // move up and out to text writer, use lister interface
        gestures.GestureFor(HandSide.Right).OnGrab += OnGrab;
        gestures.GestureFor(HandSide.Left).OnGrab += OnGrab;
        gestures.GestureFor(HandSide.Right).OnRelease += OnRelease;
        gestures.GestureFor(HandSide.Left).OnRelease += OnRelease;
    }

    public string OnHandUpdate(IHand hand)
    {
        // move up and out to text writer
        var side = hand.Side();
        var gesture = gestures.GestureFor(side);


        debug.Log("hand side: " + hand.Side());

        // yes this is a bit odd, around construction-time access to vrtk
        // instances - have a look sometime when on vive setup 
        gesture.hand = hand;

        gesture.OnHandUpdate(hand);
        debug.Log("words: " + String.Join(", ", words.ToArray()));
        debug.Log("text: " + Text());
        return Text();
    }

    public bool IsGrabbing(HandSide side)
    {
        return gestures.GestureFor(side).IsGrabbing;
    }

    public void OnGrab(Gesture gesture, Knob knob)
    {
        wordKnobs.Add(knob);
        knobArranger.Arrange(Text());
    }

    public void OnRelease(Knob knob)
    {
        if (!gestures.AnyGrabs())
        {
            CompleteWord();
        }
    }

    private void CompleteWord()
    {
        words.Add(Text());
        wordKnobs.Clear();
        knobArranger.ResetLayers();
    }

    public string Text()
    {
        return String.Join("", wordKnobs.Select(knob => knob.Text()).ToArray());
    }

    public List<string> Words()
    {
        return words;
    }
}
