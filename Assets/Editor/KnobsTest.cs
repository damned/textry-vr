using System;
using NUnit.Framework;
using UnityEngine;
using Builders;

[TestFixture]
public class KnobsTest
{
  private Knobs knobs;
  private Knob knobA;
  private Knob knobB;
  private Knob knobC;

  [SetUp]
  public void SetUp()
  {
    var knobsGo = new GameObject();
    knobs = knobsGo.AddComponent<Knobs>();

    knobA = knobs.Create(A.Letter("a"), 0, 0, 0, 0);
    knobB = knobs.Create(A.Letter("b"), 1, 0, 0, 0);
    knobC = knobs.Create(A.Letter("c"), 2, 0, 0, 0);
  }

  [Test]
  public void FadesKnobsThatAreNotGrabbed()
  {    
    knobA.Grab();

    knobs.OnKnobStateChange();

    Assert.True(knobB.Faded);
    Assert.True(knobC.Faded);
    Assert.False(knobA.Faded);
  }

  [Test]
  public void FadesNoKnobsIfNoneGrabbed()
  {    
    knobs.OnKnobStateChange();

    Assert.False(knobA.Faded);
    Assert.False(knobB.Faded);
    Assert.False(knobC.Faded);
  }

}