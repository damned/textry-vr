using System;
using System.Collections.Generic;
using UnityEngine;

public class GesturesStrategy
{
    private readonly Knobs knobs;
    private readonly IDebug debug;
    private readonly KnobArranger knobArranger;

    private int layer = 0;
    private string text = "";

    private readonly Gestures gestures;
    private readonly List<string> words = new List<string>();
    private int grabbedLayer;
    private Knob layerCreatingKnob;

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
        gestures.GestureFor(HandSide.Right).OnTouch += OnTouch;
        gestures.GestureFor(HandSide.Left).OnTouch += OnTouch;
    }

    public string OnHandUpdate(IHand hand)
    {
        // move up and out to text writer
        var side = hand.Side();
        var gesture = gestures.GestureFor(side);


        // debug.Log("hand side: " + hand.Side());

        // yes this is a bit odd, around construction-time access to vrtk
        // instances - have a look sometime when on vive setup 
        gesture.hand = hand;

        gesture.OnHandUpdate(hand);
        debug.Log("text: " + text + ", words: " + String.Join(", ", words.ToArray()));
        return text;
    }

    public bool IsGrabbing(HandSide side)
    {
        return gestures.GestureFor(side).IsGrabbing;
    }

    public void OnGrab(Gesture gesture, Knob knob)
    {
        grabbedLayer = layer;
        AddLayer(knob);
    }

    private void AddLayer(Knob knob)
    {
        Debug.Log($"adding layer, pre-text: {text}");
        layer += 1;
        layerCreatingKnob = knob;
        text += knob.Text();
        string arrangement = knobArranger.Arrange(layer * 0.08f, text);
        debug.Log(arrangement);
        Debug.Log($"adding layer, post-text: {text}");
    }


    private void RemoveLayer()
    {
        layer -= 1;
        Debug.Log($"removing layer, pre-text: {text}");
        text = text.Substring(0, text.Length - 1);
        Debug.Log($"removing layer, post-text: {text}");
        knobArranger.RemoveLayer();
    }

    public void OnTouch(Gesture gesture, Knob knob)
    {
        debug.Log($"touch: knob layer {knob.Layer}, layer {layer}");
        if (gestures.AnyGrabs())
        {
            if (AtLastLayer(knob))
            {
                AddLayer(knob);
            }
            else if (BeforeTouchSelectedLayer(knob))
            {
                RemoveLayer();
            }
        }
    }

    private bool BeforeTouchSelectedLayer(Knob knob)
    {
        return knob.Layer < layer - 1;
    }

    private bool AtLastLayer(Knob knob)
    {
        return knob.Layer == layer;
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
        words.Add(text);
        text = "";
        layer = 0;
        knobArranger.ResetLayers();
    }

    public string Text()
    {
        return text;
    }

    public List<string> Words()
    {
        return words;
    }
}
