using System;
using System.Collections.Generic;
using System.Linq;

public class Gestures
{
  private readonly Knobs knobs;
  private readonly List<Gesture> gestures;

  public Gestures(Knobs knobs) : this(new Gesture [] {new Gesture(HandSide.Right, knobs), new Gesture(HandSide.Left, knobs)})
  {
  }

  public Gestures(params Gesture[] gestures)
  {
    this.gestures = new List<Gesture>(gestures);
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
}