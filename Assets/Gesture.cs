using System;
using UnityEngine;

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

  public void Leave(Knob knob)
  {
    approached = null;
    grabbed = null;
    hand = null;
    knob.Leave();
  }

  public void Grab(Knob knob)
  {
    grabbed = knob;
    approached = null;
  }

  public void Touch(Knob knob)
  {
    approached = knob;
    knob.Touch();
  }

  public void NotTouching()
  {
    approached = null;
  }
}