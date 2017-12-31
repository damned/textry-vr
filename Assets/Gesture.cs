using System;

public class Gesture
{

  public Knob grabbed = null;
  public Knob approached;
  public IHand hand;
  
  private readonly HandSide side;

  public Gesture(HandSide side)
  {
    this.side = side;
  }

  public HandSide Side()
  {
    return side;
  }

  public void Leave()
  {
    approached = null;
    grabbed = null;
    hand = null;
  }
}