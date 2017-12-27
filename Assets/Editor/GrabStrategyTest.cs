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
  private StubHand rightHand;

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
    rightHand = new StubHand(HandSide.Left);
  }

  [Test]
  public void creates_new_knobs_layer_when_knob_grabbed()
  {
    CreateKnobs("a");
    var firstAPosition = Knob("a").Position();

    strategy = NewGrabStrategy();

    strategy.OnHandUpdate(rightHand.At(firstAPosition).Open());
   
    Assert.AreEqual(1, arranger.layers);

    strategy.OnHandUpdate(rightHand.At(firstAPosition).Closed());

    Assert.AreEqual(2, arranger.layers); // layers should be exposed as truth of knobs
                                       // or mock out arranger?
  }

  [Test]
  public void only_creates_one_new_layer_when_knob_grabbed()
  {
    CreateKnobs("a");
    var firstAPosition = Knob("a").Position();

    strategy = NewGrabStrategy();

    strategy.OnHandUpdate(rightHand.At(firstAPosition).Open());
    strategy.OnHandUpdate(rightHand.Closed());
    strategy.OnHandUpdate(rightHand);
    strategy.OnHandUpdate(rightHand);

    Assert.AreEqual(2, arranger.layers);
  }

  [Test]
  public void dont_act_as_if_grab_if_hand_already_closed_as_arrives_at_knob()
  {
    CreateKnobs("a");
    var firstAPosition = Knob("a").Position();

    strategy = NewGrabStrategy();

    Assert.AreEqual(1, arranger.layers);

    strategy.OnHandUpdate(rightHand.At(firstAPosition).Closed());

    Assert.AreEqual(1, arranger.layers);
  }

  [Test]
  public void need_to_stay_grabbed_as_move_away_then_release_once_hand_opened()
  {
    CreateKnobs("a", "b", "c");
    var firstAPosition = Knob("a").Position();
    var firstCPosition = Knob("c").Position();

    strategy = NewGrabStrategy();

    Assert.IsFalse(strategy.IsGrabbing(HandSide.Right));

    strategy.OnHandUpdate(rightHand.At(firstAPosition).Open());
    strategy.OnHandUpdate(rightHand.Closed());

    Assert.IsTrue(strategy.IsGrabbing(HandSide.Right));

    strategy.OnHandUpdate(rightHand.At(firstCPosition));

    Assert.IsTrue(strategy.IsGrabbing(HandSide.Right));

    strategy.OnHandUpdate(rightHand.Open());

    Assert.IsFalse(strategy.IsGrabbing(HandSide.Right));
  }

  [Test]
  public void need_to_release_grabbed_knob_when_hand_moved_away_from_knobs()
  {
    CreateKnobs("a");

    Vector3 somewhereElse = new Vector3(9999, 6543, 7399);

    strategy = NewGrabStrategy();

    strategy.OnHandUpdate(rightHand.At(Knob("a").Position()).Open());
    strategy.OnHandUpdate(rightHand.Closed());

    Assert.IsTrue(strategy.IsGrabbing(HandSide.Right));

    strategy.OnHandUpdate(rightHand.At(somewhereElse));

    Assert.IsFalse(strategy.IsGrabbing(HandSide.Right));
  }

  [Test]
  public void need_to_release_grabbed_knob_when_hand_no_longer_present()
  {
    CreateKnobs("a");

    strategy = NewGrabStrategy();

    strategy.OnHandUpdate(rightHand.At(Knob("a").Position()).Open());
    strategy.OnHandUpdate(rightHand.Closed());

    Assert.IsTrue(strategy.IsGrabbing(HandSide.Right));

    strategy.OnHandUpdate(rightHand.ThatIsNotPresent());

    Assert.IsFalse(strategy.IsGrabbing(HandSide.Right));
  }

  [Test]
  public void dont_go_around_approaching_loads_of_different_knobs_then_grabbing_all_and_sundry_when_arrive_again_this_time_closed()
  {
    CreateKnobs("a", "b", "c");
    
    var firstAPosition = Knob("a").Position();
    var firstBPosition = Knob("b").Position();
    var firstCPosition = Knob("c").Position();

    strategy = NewGrabStrategy();

    rightHand.Open();
    strategy.OnHandUpdate(rightHand.At(firstAPosition));
    strategy.OnHandUpdate(rightHand.At(firstBPosition));
    strategy.OnHandUpdate(rightHand.At(firstCPosition));

    rightHand.Closed();
    strategy.OnHandUpdate(rightHand.At(firstAPosition));
    strategy.OnHandUpdate(rightHand.At(firstBPosition));
    strategy.OnHandUpdate(rightHand.At(firstCPosition));

    Assert.IsFalse(strategy.IsGrabbing(HandSide.Right));
  }

  [Test]
  public void second_grab_with_other_hand_adds_to_text()
  {
    CreateKnobs("a", "b");

    var leftHand = new StubHand(HandSide.Left);

    strategy = NewGrabStrategy();

    strategy.OnHandUpdate(rightHand.At(Knob("a", 0).Position()).Open());
    strategy.OnHandUpdate(rightHand.Closed());
    strategy.OnHandUpdate(leftHand.At(Knob("b", 1).Position()).Open());
    strategy.OnHandUpdate(leftHand.Closed());
   
    Assert.AreEqual("ab", strategy.Text());
  }

  [Test]
  public void second_hand_presence_does_not_release_first_grab()
  {
    CreateKnobs("a", "b");

    var leftHand = new StubHand(HandSide.Left);

    strategy = NewGrabStrategy();

    strategy.OnHandUpdate(rightHand.At(Knob("a", 0).Position()).Open());
    strategy.OnHandUpdate(rightHand.Closed());
    strategy.OnHandUpdate(leftHand.At(Knob("b", 0).Position()).Open());
   
    Assert.IsTrue(strategy.IsGrabbing(HandSide.Right));
    Assert.IsTrue(strategy.IsGrabbing(HandSide.Left));
  }

  private Knob Knob(string letter, int index = 0)
  {
    Knob found = null;
    int count = 0;
    knobs.ForEach(knob => {
      if (knob.Text() == letter)
      {
        if (count == index) {
          found = knob;
        }
        count++;
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