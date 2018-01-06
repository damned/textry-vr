using System;
using System.Collections.Generic;
using System.Linq;

// hmm "interesting" breakdown of responsibility between Gestures and GestureStrategy...
// now looking at "if grabbed, touch opens/updates next layer" and i'm not sure which...
// Gestures defo has lower level right/left handling, and GS has higher level text layer
// interaction...
// maybe Gestures should raise OnTouchWhileGrabbed, OnControllingGrabMove....
public class Gestures
{
  private readonly Knobs knobs;
  private readonly List<Gesture> gestures;
  private HandSide latestGrabSide;

  public Gestures(Knobs knobs) : this(new Gesture [] {new Gesture(HandSide.Right, knobs), new Gesture(HandSide.Left, knobs)})
  {
  }

  public Gestures(params Gesture[] gestures)
  {
    this.gestures = new List<Gesture>(gestures);
    foreach (var gesture in gestures)
    {
      gesture.OnGrabMove += GrabGestureMoved;
      gesture.OnGrab += GrabMade;
    }
  }

  public Gesture GestureFor(HandSide side)
  {
    foreach (var gesture in gestures)
    {
      if (gesture.Side() == side)
      {
        return gesture;
      }
    };
    return null;
  }

  public bool AnyGrabs()
  {
    return gestures.Any(g => g.IsGrabbing);
  }

  public void GrabGestureMoved(Gesture gesture, Knob grabbedKnob)
  {
    if (IsControllingGrab(gesture))
    {
      grabbedKnob.GrabbingHandMove(gesture.hand.Centre());
    }
  }

  private bool IsControllingGrab(Gesture gesture)
  {
    return latestGrabSide == gesture.Side();
  }

  public void GrabMade(Gesture gesture, Knob knob)
  {
    latestGrabSide = gesture.hand.Side();
  }

}