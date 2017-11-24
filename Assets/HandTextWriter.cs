using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class HandTextWriter : MonoBehaviour
{
  public float range = 2f;

  // actually this should probably be a Knob not a Letter, cos that's what we're dealing with
  private Letter lastClosest = null;
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
      ClearLastClosest((Letter)null);
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
        knobs.FadeOtherKnobs(closest.gameObject);
        layer += 1;
        text += closest.letter;
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

  void MoveGrabbed(Letter letter)
  {
    if (letter.grabbed)
    {
      float tolerance = 0.01f;

      Vector3 grabPosition = letter.grabbingHand.Centre();

      Debug.Log("hand z: " + grabPosition.z);
      Debug.Log("letter z: " + letter.Z());

      if (grabPosition.z < (letter.Z() - tolerance))
      {
        letter.MoveCloser();
      }
      else if (grabPosition.z > (letter.Z() + tolerance))
      {
        letter.MoveAway();
      }
    }
  }

  public void Grab(Letter letter, LeapHand hand)
  {
    if (letter.approached && !letter.grabbed)
    {
      Grabbed(letter, hand);
    }
    letter.approached = false;
  }

  private void Grabbed(Letter letter, LeapHand hand)
  {
    letter.grabbingHand = hand;
    letter.ChangeColour(Color.red);

    letter.grabbed = true;
  }

  private void Approach(Letter letter)
  {
    letter.approached = true;
    letter.ChangeColour(Color.black);
  }

  private void ClearLastClosest(Letter closest)
  {
    if (lastClosest != null && lastClosest != closest)
    {
      Leave(lastClosest);
      lastClosest = null;
    }
  }

  private void Leave(Letter letter)
  {
    letter.ChangeColour(Color.white);
    letter.approached = false;
    letter.grabbed = false;
    letter.grabbingHand = null;
  }

  private Letter FindClosestTo(LeapHand hand)
  {
    Vector3 distance = new Vector3(1, 0.0f, 0.0f);

    Letter closest = null;

    knobs.ForEach(letter => {
      Vector3 new_distance = hand.Centre() - letter.Position();
      if (new_distance.magnitude < distance.magnitude)
      {
        closest = letter;
        distance = new_distance;
      }
    });
    return closest;
  }

  private Letter LetterOf(GameObject go)
  {
    return go.GetComponent<Letter>();
  }
}
