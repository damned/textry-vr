using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
public class HandTextWriter : MonoBehaviour
{
  public float range = 2f;

  private LiveDebug debug;
  private Knobs knobs;
  private Letters letters;
  private LeapHands hands;
  private KnobArranger knobArranger;

  private GrabStrategy grabStrategy;

  void Start()
  {
    hands = GetComponent<LeapHands>();
    debug = GetComponent<LiveDebug>();

    knobs = GameObject.Find("interactables").GetComponent<Knobs>();
    letters = GameObject.Find("letters").GetComponent<Letters>();

    debug.Log("knobs and letters: " + knobs + ", " + letters);

    knobArranger = new KnobArranger(letters, knobs);
    knobArranger.Arrange(0f);

    grabStrategy = new GrabStrategy(knobs, knobArranger, debug);

    hands.OnHandUpdate += OnHandUpdate;
  }

  private void OnHandUpdate(LeapHand hand)
  {
    grabStrategy.OnHandUpdate(hand);
  }

  void Update()
  {
    debug.Clear();
  }

}
