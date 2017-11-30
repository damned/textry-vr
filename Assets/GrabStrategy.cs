using UnityEngine;

public class GrabStrategy
{
  private readonly Knobs knobs;
  private readonly IDebug debug;
  private readonly KnobArranger knobArranger;
  
  // actually maybe this becomes a Grab object, cos then track which hand for 2 grabs etc. :)
  private int layer = 0;
  private string text = ""; 

  private Grab grab =  new Grab();


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
      ReleaseAllKnobs();
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

  public bool IsGrabbing()
  {
    return grab.grabbed != null;
  }

  private void HandleCloseToKnob(IHand hand, Knob closest)
  {
    if (hand.IsClosed())
    {
      if (closest != grab.grabbed)
      {
        if (closest == grab.approached)
        {
          if (grab.grabbed != closest)
          {
            Grabbed(closest, hand);
          }
        }
        UnapproachAllKnobs();
      }
    }
    else
    {
      ReleaseAllKnobs();
      Approach(closest);
    }
    MoveKnobToHand(grab.grabbed);
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
    grab.approached = null;
  }

  private void MoveKnobToHand(Knob knob)
  {
    if (knob != null)
    {
      float tolerance = 0.01f;

      Vector3 handPosition = grab.hand.Centre();

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
    grab.hand = hand;
    knob.ChangeColour(Color.red);

    knobs.FadeOtherKnobs(knob);
    layer += 1;
    text += knob.Text();
    string arrangement = knobArranger.Arrange(layer * 0.2f);
    debug.Log(arrangement);

    grab.grabbed = knob;
    grab.approached = null;
  }

  private void Approach(Knob knob)
  {
    grab.approached = knob;
    knob.ChangeColour(Color.black);
  }

  private void Leave(Knob knob)
  {
    knob.ChangeColour(Color.white);
    grab.approached = null;
    grab.grabbed = null;
    grab.hand = null;
  }
}
