using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class HandTextWriter : MonoBehaviour
{
  public float range = 2f;

  private Knob lastClosest = null;
  private LiveDebug debug;
  private Knobs knobs;
  private Letters letters;
  private LeapHands hands;
  private KnobArranger knobArranger;

  private int layer = 0;
  private string text = "";

  void Start()
  {
    hands = GetComponent<LeapHands>();
    debug = GetComponent<LiveDebug>();

    knobs = GameObject.Find("interactables").GetComponent<Knobs>();
    letters = GameObject.Find("letters").GetComponent<Letters>();

    debug.Log("knobs and letters: " + knobs + ", " + letters);

    knobArranger = new KnobArranger(letters, knobs);
    knobArranger.Arrange(0f);

    hands.OnHandUpdate += CheckForGrab;
  }

  void Update()
  {
    debug.Clear();
  }

  private void CheckForGrab(LeapHand hand)
  {
    if (!hand.IsPresent())
    {
      ClearLastClosest(null);
      return;
    }

    debug.Log("text: " + text);
    var closest = FindClosestTo(hand);

    // debug.Log("Closest: " + closest);
    ClearLastClosest(closest);
    if (closest == null)
    {
      // debug.Log("Nearly grabbed things: " + string.Join(", ", close_things.ToList().Select(t => t.name).ToArray()));
      return;
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
  }

  void MoveGrabbed(Knob knob)
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

  public void Grab(Knob knob, LeapHand hand)
  {
    if (knob.approached && !knob.grabbed)
    {
      Grabbed(knob, hand);
    }
    knob.approached = false;
  }

  private void Grabbed(Knob knob, LeapHand hand)
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

  private Knob FindClosestTo(LeapHand hand)
  {
    Vector3 distance = new Vector3(1, 0.0f, 0.0f);

    Knob closest = null;

    knobs.ForEach(knob => {
      Vector3 new_distance = hand.Centre() - knob.Position();
      if (new_distance.magnitude < distance.magnitude)
      {
        closest = knob;
        distance = new_distance;
      }
    });
    return closest;
  }
}
