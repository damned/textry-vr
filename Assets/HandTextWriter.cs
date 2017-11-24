﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class HandTextWriter : MonoBehaviour
{
  public float range = 2f;
  public float fadeLevel = 0.4f;

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
        closest.Grab(hand);
        knobs.FadeOtherKnobs(closest.gameObject, fadeLevel);
        layer += 1;
        text += closest.letter;
        string arrangement = knobArranger.Arrange(layer * 0.2f);
        debug.Log(arrangement);
        lastClosest = closest;
      }
    }
    else if (hand.GrabStrength() < 0.4)
    {
      closest.Approach();
    }
  }

  private void ClearLastClosest(Letter closest)
  {
    if (lastClosest != null && lastClosest != closest)
    {
      lastClosest.Leave();
      lastClosest = null;
    }
  }

  private Letter FindClosestTo(LeapHand hand)
  {
    Collider[] close_things = Physics.OverlapSphere(hand.Centre(), range);
    Vector3 distance = new Vector3(1, 0.0f, 0.0f);

    Collider closest = null;

    for (int j = 0; j < close_things.Length; ++j)
    {
      Vector3 new_distance = hand.Centre() - close_things[j].transform.position;
      if (new_distance.magnitude < distance.magnitude && !hands.IsHandPart(close_things[j]))
      {
        closest = close_things[j];
        distance = new_distance;
      }
    }

    return LetterOf(closest.gameObject);
  }

  private Letter LetterOf(GameObject go)
  {
    return go.GetComponent<Letter>();
  }
}
