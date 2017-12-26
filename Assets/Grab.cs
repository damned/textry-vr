public class Grab
{

  public Knob grabbed = null;
  public Knob approached;
  public IHand hand;

  public Grab()
  {
  }

  public HandSide Side()
  {
    return HandSide.Right;
  }
}