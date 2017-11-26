using System;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class GrabStrategyTest
{
  private Knobs knobs;
  private Letters letters;
  private KnobArranger arranger;
  private GrabStrategy strategy;
  private StubHand hand;

  // todo: thoughts from typing tests:
  // already thinking grab strategy should do knobs.add_layer([knobs]); then
  // grab strategy should hand off "knob grabbed" to whatever's going to manage
  //     what the next layer of knobs should actually be:  a text predicter?

  [SetUp]
  public void SetUp()
  {
    var knobsObject = new GameObject("knobs");
    var lettersObject = new GameObject("letters");
    knobs = knobsObject.AddComponent<Knobs>();
    letters = lettersObject.AddComponent<Letters>();
    arranger = new KnobArranger(letters, knobs);
    hand = new StubHand();
  }

  [Test]
  public void creates_new_knobs_layer_when_knob_grabbed()
  {
    CreateKnobs("a", "b", "c");

    strategy = NewGrabStrategy();

    strategy.OnHandUpdate(hand.At(Knob("a").Position()).WithGrabStrength(0f));
   
    Assert.That(arranger.layers == 1);

    strategy.OnHandUpdate(hand.At(Knob("a").Position()).WithGrabStrength(1f));

    Assert.That(arranger.layers == 2); // layers should be exposed as truth of knobs
                                       // or mock out arranger?
  }

  private Knob Knob(string letter)
  {
    Knob found = null;
    knobs.ForEach(knob => {
      if (knob.Text() == letter)
      {
        found = knob;
      }
    });
    if (found == null)
    {
      throw new InvalidOperationException("not a named knob: " + letter);
    }
    return found;
  }

  private GrabStrategy NewGrabStrategy()
  {
    return new GrabStrategy(knobs, arranger, new StubDebug());
  }

  private void CreateKnobs(params string[] allLetters)
  {
    foreach (var letter in allLetters)
    {
      CreateLetter(letter).transform.parent = letters.transform;
    }
    arranger.Arrange(0f);
  }

  private static GameObject CreateLetter(string name)
  {
    var letter = new GameObject(name);
    letter.AddComponent<Letter>().letter = name;
    letter.AddComponent<MeshRenderer>();
    return letter;
  }
}