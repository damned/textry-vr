public class Gestures
{
  private Gesture[] gestures;

  public Gestures(params Gesture [] gestures)
  {
    this.gestures = gestures;
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