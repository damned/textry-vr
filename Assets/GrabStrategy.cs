using System;
using UnityEngine;

public class GrabStrategy
{
  private readonly Knobs knobs;
  private readonly IDebug debug;
  private readonly KnobArranger knobArranger;
  
  private int layer = 0;
  private string text = ""; 


  private Gestures gestures = new Gestures(new Gesture(HandSide.Right), new Gesture(HandSide.Left)); 


  public GrabStrategy(Knobs knobs, KnobArranger knobArranger, IDebug debug)
  {
    this.knobArranger = knobArranger;
    this.debug = debug;
    this.knobs = knobs;
  }

  public string OnHandUpdate(IHand hand)
  {
    EnsureHandAndGestureWired(hand);

    // HARDCODE to continue working even though both hands updating - logic doesn't cope with right and left yet
    var side = HandSide.Right;
    debug.Log("hand side: " + hand.Side());
    if (hand.Side() != side)
    {
      return text;
    }

    if (!hand.IsPresent())
    {
      Gesture(side).Leave();
      return text;
    }
    // debug.Log("hand position: " + hand.Centre());

    debug.Log("text: " + text);
    var closest = knobs.FindClosestTo(hand);

    // debug.Log("Closest: " + closest);
    if (closest == null)
    {
      Gesture(side).Leave();
      // debug.Log("Nearly grabbed things: " + string.Join(", ", close_things.ToList().Select(t => t.name).ToArray()));
      return text;
    }

    HandleCloseToKnob(hand, closest, side);
    return text;
  }

  // yes this is a bit odd, around construction-time access to vrtk
  // instances - have a look sometime when on vive setup 
  private void EnsureHandAndGestureWired(IHand hand)
  {
    Gesture(hand.Side()).hand = hand;
  }

  public Gesture Gesture(HandSide side)
  {
    return gestures.GestureFor(side);
  }

  public bool IsGrabbing(HandSide side)
  {
    return gestures.GestureFor(side).IsGrabbing;
  }

  private void HandleCloseToKnob(IHand hand, Knob closest, HandSide side)
  {
    if (hand.IsClosed())
    {
      if (closest != Gesture(side).grabbed)
      {
        // debug.Log("Closest: " + closest);
        // debug.Log("Grab - approached: " + Gesture().approached);
        if (closest == Gesture(side).approached)
        {
          Grabbed(closest, hand, side);
        }
        Gesture(side).NotTouching();
      }
    }
    else
    {
      Gesture(side).Leave();
      Gesture(side).Touch(closest);
    }
    MoveKnobToHand(Gesture(side).grabbed, side);
  }

  // move to Knobs
  private void MoveKnobToHand(Knob knob, HandSide side)
  {
    if (knob != null)
    {
      float tolerance = 0.01f;

      Vector3 handPosition = Gesture(side).hand.Centre();

      Debug.Log("hand z: " + handPosition.z);
      Debug.Log("knob z: " + knob.Z());

      if (handPosition.z < (knob.Z() - tolerance))
      {
        knobs.MoveCloser();
      }
      else if (handPosition.z > (knob.Z() + tolerance))
      {
        knobs.MoveAway();
      }
    }
  }

  private void Grabbed(Knob knob, IHand hand, HandSide side)
  {
    // should probs get knobs to fade all unhandled knobs only, probs only within grab layer?
    knobs.FadeOtherKnobs(knob);
    layer += 1;
    text += knob.Text();
    string arrangement = knobArranger.Arrange(layer * 0.2f, knob.Text());
    debug.Log(arrangement);

    Gesture(side).Grab(knob);
  }

  public string Text()
  {
    return text;
  }
}
