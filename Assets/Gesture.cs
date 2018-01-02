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
  private readonly Knobs knobs;

  public Gesture(HandSide side, Knobs knobs)
  {
    this.knobs = knobs;
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
    if (knob != null)
    {
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

  public void MoveGrabbedKnobToHand()
  {
    grabbed.GrabbingHandMove(hand.Centre());
  }

  public void OnHandUpdate(IHand hand, HandSide side)
  {
    if (!hand.IsPresent())
    {
      HandNotNearKnob();
      return;
    }

    var closest = knobs.FindClosestTo(hand.Centre());

    if (closest == null)
    {
      HandNotNearKnob();
      return;
    }

    HandNearKnob(closest);
  }


  public void HandNotNearKnob()
  {
    Leave();
  }

  public void HandNearKnob(Knob closest)
  {
    if (hand.IsClosed())
    {
      if (closest != grabbed)
      {
        // debug.Log("Closest: " + closest);
        // debug.Log("Grab - approached: " + Gesture().approached);
        if (closest == approached)
        {
          Grab(closest);
        }
        approached = null;
      }
      if (IsGrabbing)
      {
        MoveGrabbedKnobToHand();
      }
    }
    else
    {
      Leave();
      Touch(closest);
    }
  }
}