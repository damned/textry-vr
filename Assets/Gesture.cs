using System;
using UnityEngine;

public delegate void KnobHandler(Knob knob);


public class Gesture
{
  public event KnobHandler OnGrab;

  public Knob grabbed = null;
  public Knob approached;
  public IHand hand;
  
  private readonly HandSide side;

  public bool IsGrabbing { get { return grabbed != null; } }

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
    LeaveAny(grabbed);
    grabbed = null;
    LeaveAny(approached);
    approached = null;
    hand = null;
  }

  private void LeaveAny(Knob knob)
  {
    if (knob != null) {
      knob.Leave();
    }    
  }

  public void Grab(Knob knob)
  {
    grabbed = knob;
    approached = null;
    knob.Grab();
    OnGrab(knob);
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

  public void MoveGrabbedKnobToHand()
  {
    grabbed.GrabbingHandMove(hand.Centre());
  }
}