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
    CreateKnobs("a");
    var firstAPosition = Knob("a").Position();

    strategy = NewGrabStrategy();

    strategy.OnHandUpdate(hand.At(firstAPosition).WithGrabStrength(0f));
   
    Assert.AreEqual(1, arranger.layers);

    strategy.OnHandUpdate(hand.At(firstAPosition).WithGrabStrength(1f));

    Assert.AreEqual(2, arranger.layers); // layers should be exposed as truth of knobs
                                       // or mock out arranger?
  }

  [Test]
  public void only_creates_one_new_layer_when_knob_grabbed()
  {
    CreateKnobs("a");
    var firstAPosition = Knob("a").Position();

    strategy = NewGrabStrategy();

    strategy.OnHandUpdate(hand.At(firstAPosition).WithGrabStrength(0f));
    strategy.OnHandUpdate(hand.At(firstAPosition).WithGrabStrength(1f));
    strategy.OnHandUpdate(hand.At(firstAPosition).WithGrabStrength(1f));
    strategy.OnHandUpdate(hand.At(firstAPosition).WithGrabStrength(1f));

    Assert.AreEqual(2, arranger.layers);
  }

  [Test]
  public void dont_act_as_if_grab_if_hand_already_closed_as_arrives_at_knob()
  {
    CreateKnobs("a");
    var firstAPosition = Knob("a").Position();

    strategy = NewGrabStrategy();

    Assert.AreEqual(1, arranger.layers);

    strategy.OnHandUpdate(hand.At(firstAPosition).WithGrabStrength(1f));

    Assert.AreEqual(1, arranger.layers);
  }

  public void need_to_let_go()
  {
    // todo
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