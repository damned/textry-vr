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
    // HARDCODE to continue working even though both hands updating - logic doesn't cope with right and left yet
    debug.Log("hand side: " + hand.Side());
    if (hand.Side() == HandSide.Left)
    {
      return text;
    }

    if (!hand.IsPresent())
    {
      ReleaseAllKnobs();
      return text;
    }
    // debug.Log("hand position: " + hand.Centre());

    debug.Log("text: " + text);
    var closest = knobs.FindClosestTo(hand);

    // debug.Log("Closest: " + closest);
    if (closest == null)
    {
      HandleAwayFromKnobs();
      // debug.Log("Nearly grabbed things: " + string.Join(", ", close_things.ToList().Select(t => t.name).ToArray()));
      return text;
    }

    HandleCloseToKnob(hand, closest);
    return text;
  }

  public Gesture Gesture()
  {
    return gestures.GestureFor(HandSide.Right);
  }

  public bool IsGrabbing(HandSide side)
  {
    return Gesture().grabbed != null;
  }

  private void HandleCloseToKnob(IHand hand, Knob closest)
  {
    if (hand.IsClosed())
    {
      if (closest != Gesture().grabbed)
      {
        // debug.Log("Closest: " + closest);
        // debug.Log("Grab - approached: " + Gesture().approached);
        if (closest == Gesture().approached)
        {
          Grabbed(closest, hand);
        }
        UnapproachAllKnobs();
      }
    }
    else
    {
      ReleaseAllKnobs();
      Approach(closest);
    }
    MoveKnobToHand(Gesture().grabbed);
  }

  private void HandleAwayFromKnobs()
  {
    ReleaseAllKnobs();
  }

  private void ReleaseAllKnobs()
  {
    knobs.ForEach(knob => { Leave(knob); });
  }

  private void UnapproachAllKnobs()
  {
    Gesture().approached = null;
  }

  private void MoveKnobToHand(Knob knob)
  {
    if (knob != null)
    {
      float tolerance = 0.01f;

      Vector3 handPosition = Gesture().hand.Centre();

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

  private void Grabbed(Knob knob, IHand hand)
  {
    Gesture().hand = hand;
    knob.ChangeColour(Color.red);

    knobs.FadeOtherKnobs(knob);
    layer += 1;
    text += knob.Text();
    string arrangement = knobArranger.Arrange(layer * 0.2f, knob.Text());
    debug.Log(arrangement);

    Gesture().grabbed = knob;
    Gesture().approached = null;
  }

  private void Approach(Knob knob)
  {
    Gesture().approached = knob;
    knob.ChangeColour(Color.black);
  }

  private void Leave(Knob knob)
  {
    knob.ChangeColour(Color.white);
    Gesture().Leave();
  }

  public string Text()
  {
    return text;
  }
}
