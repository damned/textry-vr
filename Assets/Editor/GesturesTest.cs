using System;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class GesturesTest
{
  [Test]
  public void GestureForReturnsAppropriateGestureForSide()
  {
    var gestures =  new Gestures(null);

    Assert.AreEqual(HandSide.Right, gestures.GestureFor(HandSide.Right).Side());
    Assert.AreEqual(HandSide.Left, gestures.GestureFor(HandSide.Left).Side());
  }

}