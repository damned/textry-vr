using System;
using UnityEngine;

public delegate void KnobHandler(Knob knob);
public delegate void GestureHandler(Gesture gesture, Knob knob);

public class Gesture
{
  public event GestureHandler OnGrab;
  public event GestureHandler OnTouch;
  public event KnobHandler OnRelease;
  public event GestureHandler OnGrabMove;

  public Knob grabbed = null;
  private Knob touched;
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
        // debug.Log("Grab - touched: " + touched);
        if (closest == touched)
        {
          GrabKnob(closest);
        }
        touched = null;
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
    touched = null; // rename to touched
    knob.Grab();
    OnGrab(this, knob);
  }

  private void TouchKnob(Knob knob)
  {
    touched = knob;
    knob.Touch();
    OnTouch(this, knob);
  }

  private void LeaveKnob()
  {
    LeaveAny(touched);
    touched = null;
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