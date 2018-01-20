using System;
using UnityEngine;

public delegate void KnobHandler(Knob knob);
public delegate void GestureHandler(Gesture gesture, Knob knob);

public class Gesture
{
  public event GestureHandler OnGrab;
  public event KnobHandler OnRelease;
  public event GestureHandler OnGrabMove;

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
        OnGrabMove(this, grabbed);
      }
    }
    else
    {
      LeaveKnob();
      if (!closest.Deleted)
      {
        TouchKnob(closest);
      }
    }
  }

  private void GrabKnob(Knob knob)
  {
    grabbed = knob;
    approached = null;
    knob.Grab();
    OnGrab(this, knob);
  }

  private void TouchKnob(Knob knob)
  {
    approached = knob;
    knob.Touch();
  }

  private void LeaveKnob()
  {
    LeaveAny(approached);
    approached = null;
    if (grabbed != null)
    {
      grabbed.Leave();
      grabbed = null;
      OnRelease(grabbed);
    }
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