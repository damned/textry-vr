using System;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class GesturesStrategyTest
{
    private Knobs knobs;
    private Letters letters;
    private KnobArranger arranger;
    private GesturesStrategy strategy;
    private StubHand leftHand;
    private StubHand rightHand;

    // todo: thoughts from typing tests:
    // already thinking gesture strategy should do knobs.add_layer([knobs]); then
    // gesture strategy should hand off "knob grabbed" to whatever's going to manage
    //     what the next layer of knobs should actually be:  a text predicter?

    [SetUp]
    public void SetUp()
    {
        var knobsObject = new GameObject("knobs");
        var lettersObject = new GameObject("letters");
        knobs = knobsObject.AddComponent<Knobs>();
        letters = lettersObject.AddComponent<Letters>();
        arranger = new KnobArranger(knobs, new PredictiveLayerCreator(letters));
        leftHand = new StubHand(HandSide.Left);
        rightHand = new StubHand(HandSide.Right);
    }

    [Test]
    public void creates_new_knobs_layer_when_knob_grabbed()
    {
        CreateKnobs("a");
        var firstAPosition = Knob("a").Position();
        Assert.AreEqual(KnobHandlingState.Unhandled, Knob("a").HandlingState);

        strategy = NewGesturesStrategy();

        strategy.OnHandUpdate(rightHand.At(firstAPosition).Open());

        Assert.AreEqual(1, knobs.LayerCount);
        Assert.AreEqual(KnobHandlingState.Touched, Knob("a").HandlingState);

        strategy.OnHandUpdate(rightHand.At(firstAPosition).Closed());

        Assert.AreEqual(2, knobs.LayerCount);
        Assert.AreEqual(KnobHandlingState.Grabbed, Knob("a").HandlingState);
    }

    [Test]
    public void knobs_become_unhandled_once_nearer_to_another()
    {
        CreateKnobs("a", "b");
        Assert.AreEqual(KnobHandlingState.Unhandled, Knob("a").HandlingState);
        Assert.AreEqual(KnobHandlingState.Unhandled, Knob("b").HandlingState);

        strategy = NewGesturesStrategy();

        rightHand.Open();
        strategy.OnHandUpdate(rightHand.At(Knob("a").Position()));

        Assert.AreEqual(KnobHandlingState.Touched, Knob("a").HandlingState);
        Assert.AreEqual(KnobHandlingState.Unhandled, Knob("b").HandlingState);

        strategy.OnHandUpdate(rightHand.At(Knob("b").Position()));

        Assert.AreEqual(KnobHandlingState.Unhandled, Knob("a").HandlingState);
        Assert.AreEqual(KnobHandlingState.Touched, Knob("b").HandlingState);
    }

    [Test]
    public void only_creates_one_new_layer_when_knob_grabbed()
    {
        CreateKnobs("a");
        var firstAPosition = Knob("a").Position();

        strategy = NewGesturesStrategy();

        strategy.OnHandUpdate(rightHand.At(firstAPosition).Open());
        strategy.OnHandUpdate(rightHand.Closed());
        strategy.OnHandUpdate(rightHand);
        strategy.OnHandUpdate(rightHand);

        Assert.AreEqual(2, knobs.LayerCount);
    }

    [Test]
    public void dont_act_as_if_grab_if_hand_already_closed_as_arrives_at_knob()
    {
        CreateKnobs("a");
        var firstAPosition = Knob("a").Position();

        strategy = NewGesturesStrategy();

        Assert.AreEqual(1, knobs.LayerCount);

        strategy.OnHandUpdate(rightHand.At(firstAPosition).Closed());

        Assert.AreEqual(1, knobs.LayerCount);
    }

    [Test]
    public void need_to_release_grabbed_knob_when_hand_moved_away_from_knobs()
    {
        CreateKnobs("a");

        Vector3 somewhereElse = new Vector3(9999, 6543, 7399);

        strategy = NewGesturesStrategy();

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

        strategy = NewGesturesStrategy();

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

        strategy = NewGesturesStrategy();

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

        strategy = NewGesturesStrategy();

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

        strategy = NewGesturesStrategy();

        strategy.OnHandUpdate(rightHand.At(Knob("a", 0).Position()).Open());
        strategy.OnHandUpdate(rightHand.Closed());
        strategy.OnHandUpdate(leftHand.At(Knob("b", 0).Position()).Open());

        Assert.IsTrue(strategy.IsGrabbing(HandSide.Right));
        Assert.IsFalse(strategy.IsGrabbing(HandSide.Left));
    }

    [Test]
    public void grabbing_and_releasing_single_letter_generates_one_char_word_and_resets_layers()
    {
        CreateKnobs("a");

        strategy = NewGesturesStrategy();

        strategy.OnHandUpdate(rightHand.At(Knob("a").Position()).Open());
        strategy.OnHandUpdate(rightHand.Closed());
        strategy.OnHandUpdate(rightHand.Open());

        Assert.AreEqual(1, strategy.Words().Count);
        Assert.AreEqual("a", strategy.Words()[0]);
        Assert.AreEqual("", strategy.Text());
        Assert.AreEqual(1, knobs.LayerCount);
    }

    [Test]
    public void word_creation_continues_while_one_of_the_hands_retains_a_grab()
    {
        CreateKnobs("a", "b", "c");

        strategy = NewGesturesStrategy();

        strategy.OnHandUpdate(rightHand.At(Knob("c", 0).Position()).Open());
        strategy.OnHandUpdate(leftHand.Open());

        strategy.OnHandUpdate(rightHand.Closed());
        strategy.OnHandUpdate(leftHand.At(Knob("a", 1).Position()).Open());

        strategy.OnHandUpdate(rightHand.Closed());
        strategy.OnHandUpdate(leftHand.Closed());

        strategy.OnHandUpdate(rightHand.Open().At(Knob("b", 2).Position()));
        strategy.OnHandUpdate(leftHand.Closed());

        strategy.OnHandUpdate(rightHand.Closed());
        strategy.OnHandUpdate(leftHand.Open());

        Assert.AreEqual(0, strategy.Words().Count);
        Assert.AreEqual("cab", strategy.Text());

        strategy.OnHandUpdate(rightHand.Open());

        Assert.AreEqual("cab", strategy.Words()[0]);
        Assert.AreEqual("", strategy.Text());
    }

    [Test]
    public void knob_remains_grabbed_while_second_grab_made_by_other_hand()
    {
        CreateKnobs("a", "b");

        strategy = NewGesturesStrategy();

        strategy.OnHandUpdate(rightHand.At(Knob("a", 0).Position()).Open());
        strategy.OnHandUpdate(rightHand.Closed());
        strategy.OnHandUpdate(leftHand.At(Knob("b", 1).Position()).Open());

        Assert.AreEqual(KnobHandlingState.Grabbed, Knob("a", 0).HandlingState);

        strategy.OnHandUpdate(leftHand.Closed());

        Assert.AreEqual(KnobHandlingState.Grabbed, Knob("a", 0).HandlingState);
    }

    [Test]
    public void moving_grabbing_hand_drags_knobs_in_z_to_match_location()
    {
        CreateKnobs("a");

        strategy = NewGesturesStrategy();

        var initialAPosition = Knob("a", 0).Position();

        strategy.OnHandUpdate(rightHand.At(initialAPosition).Open());
        strategy.OnHandUpdate(rightHand.Closed());

        Assert.AreEqual(initialAPosition, Knob("a", 0).Position());

        var movedHandPosition = new Vector3(initialAPosition.x, initialAPosition.y, initialAPosition.z + 1);

        strategy.OnHandUpdate(rightHand.At(movedHandPosition));

        AssertPositionsEqual(movedHandPosition, Knob("a", 0).Position());
    }

    private static void AssertPositionsEqual(Vector3 expected, Vector3 actual)
    {
        Assert.AreEqual(expected.x, actual.x, 0.0001f, "in x");
        Assert.AreEqual(expected.y, actual.y, 0.0001f, "in y");
        Assert.AreEqual(expected.z, actual.z, 0.0001f, "in z");
    }

    [Test]
    public void knobs_move_to_latest_grab_only()
    {
        CreateKnobs("a", "b");

        strategy = NewGesturesStrategy();

        var initialAPosition = Knob("a", 0).Position();

        strategy.OnHandUpdate(rightHand.At(initialAPosition).Open());
        strategy.OnHandUpdate(rightHand.Closed());

        var initialBPosition = Knob("b", 1).Position();

        strategy.OnHandUpdate(leftHand.At(initialBPosition).Open());
        strategy.OnHandUpdate(leftHand.Closed());

        Vector3 movedRightPosition = MovedInZ(initialAPosition, 0.5f);
        Vector3 movedLeftPosition = MovedInZ(initialBPosition, 0.7f);

        strategy.OnHandUpdate(leftHand.At(movedLeftPosition));
        strategy.OnHandUpdate(rightHand.At(movedRightPosition));

        AssertPositionsEqual(movedLeftPosition, Knob("b", 1).Position());
    }

    private static Vector3 MovedInZ(Vector3 position, float offset)
    {
        return new Vector3(position.x, position.y, position.z + offset);
    }

    private Knob Knob(string letter, int index = 0)
    {
        Knob found = null;
        int count = 0;
        knobs.ForEach(knob =>
        {
            if (knob.Text() == letter)
            {
                if (count == index)
                {
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

    private GesturesStrategy NewGesturesStrategy()
    {
        var gestures = new Gestures(knobs);
        return new GesturesStrategy(gestures, arranger, new StubDebug());
    }

    private void CreateKnobs(params string[] allLetters)
    {
        foreach (var letter in allLetters)
        {
            CreateLetter(letter).transform.parent = letters.transform;
        }
        arranger.Arrange();
    }

    private static GameObject CreateLetter(string name)
    {
        var letter = new GameObject(name);
        letter.AddComponent<Letter>().letter = name;
        letter.AddComponent<MeshRenderer>();
        return letter;
    }
}