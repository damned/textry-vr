using UnityEngine;

public class GrabStrategy
{
  private readonly Knobs knobs;
  private readonly LiveDebug debug;
  private readonly KnobArranger knobArranger;

  private Knob lastClosest = null;
  private int layer = 0;
  private string text = "";


  public GrabStrategy(Knobs knobs, KnobArranger knobArranger, LiveDebug debug)
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

    debug.Log("text: " + text);
    var closest = knobs.FindClosestTo(hand);

    // debug.Log("Closest: " + closest);
    ClearLastClosest(closest);
    if (closest == null)
    {
      // debug.Log("Nearly grabbed things: " + string.Join(", ", close_things.ToList().Select(t => t.name).ToArray()));
      return text;
    }

    // debug.Log("Closest: " + closest.name);
    if (hand.GrabStrength() > 0.7)
    {
      if (closest != lastClosest)
      {
        Grab(closest, hand);
        knobs.FadeOtherKnobs(closest);
        layer += 1;
        text += closest.Text();
        string arrangement = knobArranger.Arrange(layer * 0.2f);
        debug.Log(arrangement);
        lastClosest = closest;
      }
    }
    else if (hand.GrabStrength() < 0.4)
    {
      Approach(closest);
    }
    knobs.ForEach(MoveGrabbed);
    return text;
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

  private void Grab(Knob knob, IHand hand)
  {
    if (knob.approached && !knob.grabbed)
    {
      Grabbed(knob, hand);
    }
    knob.approached = false;
  }

  private void Grabbed(Knob knob, IHand hand)
  {
    knob.grabbingHand = hand;
    knob.ChangeColour(Color.red);

    knob.grabbed = true;
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
