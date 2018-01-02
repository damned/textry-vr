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

  public void OnHandUpdate(IHand hand)
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

  private void MoveGrabbedKnobToHand()
  {
    grabbed.GrabbingHandMove(hand.Centre());
  }

  private void HandNotNearKnob()
  {
    LeaveKnob();
  }

  private void HandNearKnob(Knob closest)
  {
    if (hand.IsClosed())
    {
      if (closest != grabbed)
      {
        // debug.Log("Closest: " + closest);
        // debug.Log("Grab - approached: " + Gesture().approached);
        if (closest == approached)
        {
          GrabKnob(closest);
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
      LeaveKnob();
      TouchKnob(closest);
    }
  }
  private void GrabKnob(Knob knob)
  {
    grabbed = knob;
    approached = null;
    knob.Grab();
    OnGrab(knob);
  }

  private void TouchKnob(Knob knob)
  {
    approached = knob;
    knob.Touch();
  }

  private void LeaveKnob()
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

}