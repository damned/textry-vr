using System;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class KnobTest
{

  [Test]
  public void KnobEqualsItself()
  {
    var knob = new Knob(null, new GameObject(), Vector3.up, new KnobLayer(0));
    Assert.That(knob == knob);
    Assert.That(knob.Equals(knob));
  }

}