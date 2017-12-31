using System;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class GesturesTest
{
  [Test]
  public void GestureForReturnsAppropriateGestureForSide()
  {
    Gesture leftGesture = new Gesture(HandSide.Left);
    Gesture rightGesture = new Gesture(HandSide.Right);
    var gestures =  new Gestures(leftGesture, rightGesture);

    Assert.AreEqual(rightGesture, gestures.GestureFor(HandSide.Right));
    Assert.AreEqual(leftGesture, gestures.GestureFor(HandSide.Left));
  }

}