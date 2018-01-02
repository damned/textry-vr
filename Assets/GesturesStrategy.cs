using System;
using UnityEngine;

public class GesturesStrategy
{
  private readonly Knobs knobs;
  private readonly IDebug debug;
  private readonly KnobArranger knobArranger;
  
  private int layer = 0;
  private string text = ""; 

  private readonly Gestures gestures; 

  public GesturesStrategy(Knobs knobs, KnobArranger knobArranger, IDebug debug)
  {
    this.knobArranger = knobArranger;
    this.debug = debug;
    this.knobs = knobs;
    this.gestures = new Gestures(knobs);

    gestures.GestureFor(HandSide.Right).OnGrab += OnGrab;
    gestures.GestureFor(HandSide.Left).OnGrab += OnGrab;
  }

  public string OnHandUpdate(IHand hand)
  {
    // HARDCODE to continue working even though both hands updating - logic doesn't cope with right and left yet
    var side = HandSide.Right; // hand.Side()
    var gesture = Gesture(side);
    

    debug.Log("hand side: " + hand.Side());
    if (hand.Side() != side)
    {
      return text;
    }

    // yes this is a bit odd, around construction-time access to vrtk
    // instances - have a look sometime when on vive setup 
    gesture.hand = hand;

    gesture.OnHandUpdate(hand, side);
    debug.Log("text: " + text);
    return text;
  }

  public Gesture Gesture(HandSide side)
  {
    return gestures.GestureFor(side);
  }

  public bool IsGrabbing(HandSide side)
  {
    return gestures.GestureFor(side).IsGrabbing;
  }

  public void OnGrab(Knob knob)
  {
    knobs.FadeOtherKnobs(knob); // move out to knob events: allows knobs dep to move from here
    layer += 1;
    text += knob.Text();
    string arrangement = knobArranger.Arrange(layer * 0.2f, knob.Text());
    debug.Log(arrangement);
  }

  public string Text()
  {
    return text;
  }
}