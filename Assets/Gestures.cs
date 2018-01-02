public class Gestures
{
  private readonly Knobs knobs;
  private readonly Gesture[] gestures;

  public Gestures(Knobs knobs)
  {
    this.gestures = new Gesture [] {new Gesture(HandSide.Right, knobs), new Gesture(HandSide.Left, knobs)};
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
}