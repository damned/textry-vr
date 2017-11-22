using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class HandTextWriter : MonoBehaviour
{
  public float range = 2f;
  public float fadeLevel = 0.4f;

  private Collider lastGrabbed = null;
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

    hands.OnHandUpdate += DetectClosestGrabbed;
  }

  void Update()
  {
    debug.Clear();
  }

  private void DetectClosestGrabbed(LeapHand hand)
  {
    if (!hand.IsPresent())
    {
      ClearLastGrabbed(null);
      return;
    }

    debug.Log("text: " + text);
    Collider[] close_things = Physics.OverlapSphere(hand.Centre(), range);
    Vector3 distance = new Vector3(1, 0.0f, 0.0f);

    Collider grabbed = null;

    for (int j = 0; j < close_things.Length; ++j)
    {
      Vector3 new_distance = hand.Centre() - close_things[j].transform.position;
      if (new_distance.magnitude < distance.magnitude && !hands.IsHandPart(close_things[j]))
      {
        grabbed = close_things[j];
        distance = new_distance;
      }
    }

    // debug.Log("Grabbed: " + grabbed);
    ClearLastGrabbed(grabbed);
    if (grabbed == null)
    {
      // debug.Log("Nearly grabbed things: " + string.Join(", ", close_things.ToList().Select(t => t.gameObject.name).ToArray()));
      return;
    }

    // debug.Log("Grabbed: " + grabbed.gameObject.name);
    if (hand.GrabStrength() > 0.7)
    {
      if (grabbed != lastGrabbed)
      {
        LetterOf(grabbed).Grab(hand);
        knobs.FadeOtherKnobs(grabbed.gameObject, fadeLevel);
        layer += 1;
        text += LetterOf(grabbed).letter;
        debug.Log(knobArranger.Arrange(layer * 0.2f));
        lastGrabbed = grabbed;
      }
    }
    else if (hand.GrabStrength() < 0.4)
    {
      LetterOf(grabbed).Approach();
    }
  }


  private void ClearLastGrabbed(Collider grabbed)
  {
    if (lastGrabbed != null && lastGrabbed != grabbed)
    {
      LetterOf(lastGrabbed).Leave();
      lastGrabbed = null;
    }
  }

  private Letter LetterOf(Collider collider)
  {
    return collider.gameObject.GetComponent<Letter>();
  }
}
