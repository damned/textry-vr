using UnityEngine;

public class GrabStrategy
{
  private readonly Knobs knobs;
  private readonly IDebug debug;
  private readonly KnobArranger knobArranger;

  private Knob lastClosest = null;
  private int layer = 0;
  private string text = "";


  public GrabStrategy(Knobs knobs, KnobArranger knobArranger, IDebug debug)
  {
    this.knobArranger = knobArranger;
    this.debug = debug;
    this.knobs = knobs;
  }

  public string OnHandUpdate(IHand hand)
  {
    if (!hand.IsPresent())
    {
      ClearLastClosest(null);
      return text;
    }
    debug.Log("hand position: " + hand.Centre());

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

  private void HandleCloseToKnob(IHand hand, Knob closest)
  {
    if (HandIsClosed(hand))
    {
      if (closest != lastClosest)
      {
        debug.Log("Closest: " + closest);
        if (lastClosest != null)
        {
          debug.Log("Last closest: " + lastClosest);
        }
        debug.Log("Grab - approached: " + closest.approached);
        if (closest.approached)
        {
          if (!closest.grabbed)
          {
            Grabbed(closest, hand);
          }
        }
        UnapproachAllKnobs();
      }
    }
    else if (HandIsOpen(hand))
    {
      ReleaseAllKnobs();
      Approach(closest);
    }
    knobs.ForEach(MoveGrabbed);
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
    knobs.ForEach(knob => { knob.approached = false; });    
  }
  private static bool HandIsClosed(IHand hand)
  {
    return hand.GrabStrength() >= 0.5;
  }

  private static bool HandIsOpen(IHand hand)
  {
    return hand.GrabStrength() < 0.5;
  }

  private void MoveGrabbed(Knob knob)
  {
    if (knob.grabbed)
    {
      float tolerance = 0.01f;

      Vector3 grabPosition = knob.grabbingHand.Centre();

      Debug.Log("hand z: " + grabPosition.z);
      Debug.Log("knob z: " + knob.Z());

      if (grabPosition.z < (knob.Z() - tolerance))
      {
        knobs.MoveCloser();
      }
      else if (grabPosition.z > (knob.Z() + tolerance))
      {
        knobs.MoveAway();
      }
    }
  }

  private void Grabbed(Knob knob, IHand hand)
  {
    knob.grabbingHand = hand;
    knob.ChangeColour(Color.red);

    knobs.FadeOtherKnobs(knob);
    layer += 1;
    text += knob.Text();
    string arrangement = knobArranger.Arrange(layer * 0.2f);
    debug.Log(arrangement);
    lastClosest = knob;

    knob.grabbed = true;
    knob.approached = false;
  }

  private void Approach(Knob knob)
  {
    knob.approached = true;
    knob.ChangeColour(Color.black);
  }

  private void ClearLastClosest(Knob closest)
  {
    if (lastClosest != null && lastClosest != closest)
    {
      Leave(lastClosest);
      lastClosest = null;
    }
  }

  private void Leave(Knob knob)
  {
    knob.ChangeColour(Color.white);
    knob.approached = false;
    knob.grabbed = false;
    knob.grabbingHand = null;
  }
}
