using System;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class GesturesTest
{
  [Test]
  public void GestureForReturnsAppropriateGestureForSide()
  {
    var gestures =  new Gestures((Knobs) null);

    Assert.AreEqual(HandSide.Right, gestures.GestureFor(HandSide.Right).Side());
    Assert.AreEqual(HandSide.Left, gestures.GestureFor(HandSide.Left).Side());
  }

  [Test]
  public void AnyGrabsIsTrueIfAnyGesturesIsGrabbing()
  {
    var grabbingGesture = new Gesture(HandSide.Left, null);
    grabbingGesture.grabbed = new Knob(null, new GameObject(), Vector3.up, 0);
    var nonGrabbingGesture = new Gesture(HandSide.Right, null);
    var gestures =  new Gestures(grabbingGesture, nonGrabbingGesture);

    Assert.True(gestures.AnyGrabs());
  }

  [Test]
  public void AnyGrabsIsFalseIfNoGesturesAreGrabbing()
  {
    var nonGrabbingGesture = new Gesture(HandSide.Right, null);
    var anotherNonGrabbingGesture = new Gesture(HandSide.Left, null);
    var gestures =  new Gestures(nonGrabbingGesture, anotherNonGrabbingGesture);

    Assert.False(gestures.AnyGrabs());
  }

}