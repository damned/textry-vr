using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void KnobHandler(Knob knob);
public delegate void GestureHandler(Gesture gesture, Knob knob);

public class Gesture
{
    public event GestureHandler OnGrab;
    public event GestureHandler OnTouch;
    public event KnobHandler OnRelease;
    public event GestureHandler OnGrabMove;
    public GameObject trailObject;

    public Knob grabbed = null;
    public Knob approached;
    public IHand hand;

    private readonly HandSide side;

    public bool IsGrabbing { get { return grabbed != null; } }
    private readonly Knobs knobs;
    private List<GameObject> trail = new List<GameObject>();
    private Vector3 lastTrailPosition = Vector3.up;
    private bool useTrail = false;

    public Gesture(HandSide side, Knobs knobs)
    {
        this.knobs = knobs;
        this.side = side;
        trailObject = GameObject.FindWithTag("TrailPoint");
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

        AddTrail(hand);

        var closest = knobs.FindClosestTo(hand.Centre());

        if (closest == null)
        {
            HandNotNearKnob();
            return;
        }

        HandNearKnob(closest);
    }

    private void AddTrail(IHand hand)
    {
        if (useTrail && (hand.Centre() - lastTrailPosition).magnitude > 0.01f)
        {
            lastTrailPosition = hand.Centre();
            trail.Add(GameObject.Instantiate(trailObject, lastTrailPosition, Quaternion.identity));
        }
    }

    public void ClearTrail()
    {
        foreach (GameObject point in trail)
        {
          if (Application.isEditor)
          {
              UnityEngine.Object.DestroyImmediate(point);
          }
          else
          {
              UnityEngine.Object.Destroy(point);
          }
        }
        trail.Clear();
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
        OnTouch(this, knob);
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