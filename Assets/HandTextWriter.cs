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
  private IHands hands;
  private KnobArranger knobArranger;

  private GesturesStrategy gesturesStrategy;

  void Start()
  {
    hands = GetComponent<IHands>();
    debug = GetComponent<LiveDebug>();

    knobs = GameObject.Find("interactables").GetComponent<Knobs>();
    letters = GameObject.Find("letters").GetComponent<Letters>();

    debug.Log("knobs and letters: " + knobs + ", " + letters);

    knobArranger = new KnobArranger(letters, knobs);
    knobArranger.Arrange(0f);

    var gestures = new Gestures(knobs);
    
    gesturesStrategy = new GesturesStrategy(gestures, knobArranger, debug);

    hands.OnHandUpdate += OnHandUpdate;
  }

  private void OnHandUpdate(IHand hand)
  {
    gesturesStrategy.OnHandUpdate(hand);
  }

  void Update()
  {
    debug.Clear();
  }

}
