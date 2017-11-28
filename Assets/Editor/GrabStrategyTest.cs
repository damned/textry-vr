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

    strategy.OnHandUpdate(hand.At(firstAPosition).Open());
   
    Assert.AreEqual(1, arranger.layers);

    strategy.OnHandUpdate(hand.At(firstAPosition).Closed());

    Assert.AreEqual(2, arranger.layers); // layers should be exposed as truth of knobs
                                       // or mock out arranger?
  }

  [Test]
  public void only_creates_one_new_layer_when_knob_grabbed()
  {
    CreateKnobs("a");
    var firstAPosition = Knob("a").Position();

    strategy = NewGrabStrategy();

    strategy.OnHandUpdate(hand.At(firstAPosition).Open());
    strategy.OnHandUpdate(hand.Closed());
    strategy.OnHandUpdate(hand);
    strategy.OnHandUpdate(hand);

    Assert.AreEqual(2, arranger.layers);
  }

  [Test]
  public void dont_act_as_if_grab_if_hand_already_closed_as_arrives_at_knob()
  {
    CreateKnobs("a");
    var firstAPosition = Knob("a").Position();

    strategy = NewGrabStrategy();

    Assert.AreEqual(1, arranger.layers);

    strategy.OnHandUpdate(hand.At(firstAPosition).Closed());

    Assert.AreEqual(1, arranger.layers);
  }

  [Test]
  public void need_to_stay_grabbed_as_move_away_then_release_once_hand_opened()
  {
    CreateKnobs("a", "b", "c");
    var firstAPosition = Knob("a").Position();
    var firstCPosition = Knob("c").Position();

    strategy = NewGrabStrategy();

    Assert.AreEqual(0, knobs.GrabCount());

    strategy.OnHandUpdate(hand.At(firstAPosition).Open());
    strategy.OnHandUpdate(hand.Closed());

    Assert.AreEqual(1, knobs.GrabCount());

    strategy.OnHandUpdate(hand.At(firstCPosition));

    Assert.AreEqual(1, knobs.GrabCount());

    strategy.OnHandUpdate(hand.Open());

    Assert.AreEqual(0, knobs.GrabCount());
  }

  [Test]
  public void need_to_release_grabbed_knob_when_hand_moved_away_from_knobs()
  {
    CreateKnobs("a");

    Vector3 somewhereElse = new Vector3(9999, 6543, 7399);

    strategy = NewGrabStrategy();

    strategy.OnHandUpdate(hand.At(Knob("a").Position()).Open());
    strategy.OnHandUpdate(hand.Closed());

    Assert.AreEqual(1, knobs.GrabCount());

    strategy.OnHandUpdate(hand.At(somewhereElse));

    Assert.AreEqual(0, knobs.GrabCount());
  }

  [Test]
  public void need_to_release_grabbed_knob_when_hand_no_longer_present()
  {
    CreateKnobs("a");

    strategy = NewGrabStrategy();

    strategy.OnHandUpdate(hand.At(Knob("a").Position()).Open());
    strategy.OnHandUpdate(hand.Closed());

    Assert.AreEqual(1, knobs.GrabCount());

    strategy.OnHandUpdate(hand.ThatIsNotPresent());

    Assert.AreEqual(0, knobs.GrabCount());
  }

  [Test]
  public void dont_go_around_approaching_loads_of_different_knobs_then_grabbing_all_and_sundry_when_arrive_again_this_time_closed()
  {
    CreateKnobs("a", "b", "c");
    
    var firstAPosition = Knob("a").Position();
    var firstBPosition = Knob("b").Position();
    var firstCPosition = Knob("c").Position();

    strategy = NewGrabStrategy();

    hand.Open();
    strategy.OnHandUpdate(hand.At(firstAPosition));
    strategy.OnHandUpdate(hand.At(firstBPosition));
    strategy.OnHandUpdate(hand.At(firstCPosition));

    hand.Closed();
    strategy.OnHandUpdate(hand.At(firstAPosition));
    strategy.OnHandUpdate(hand.At(firstBPosition));
    strategy.OnHandUpdate(hand.At(firstCPosition));

    Assert.AreEqual(0, knobs.GrabCount());    
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