using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
public class HandTextWriter : MonoBehaviour
{
  public float range = 2f;

  private LiveDebug debug;
  public Knobs knobs;
  public Letters letters;
  public WordSuggester wordSuggester;
  private IHands hands;
  private KnobArranger knobArranger;

  private GesturesStrategy gesturesStrategy;

  void Start()
  {
    hands = GetComponent<IHands>();
    debug = GetComponent<LiveDebug>();

    var predictor = new DataBasedAlphabeticPredictor(EnglishWords.AsList());
    var layerCreator = new LetterBasedLayerCreator(letters, predictor);
    knobArranger = new KnobArranger(letters, knobs, layerCreator, new ConsistentAlphabeticLogicalLettersPlacer());
    knobArranger.Arrange(0f);

    var gestures = new Gestures(knobs);
    
    gesturesStrategy = new GesturesStrategy(gestures, knobArranger, debug);

    hands.OnHandUpdate += OnHandUpdate;
    gesturesStrategy.OnWord += wordSuggester.Suggest;
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
