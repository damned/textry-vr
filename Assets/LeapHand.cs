
using UnityEngine;

using Leap;
public class LeapHand : IHand
{
  private readonly LeapHands hands;
  private readonly LiveDebug debug;

  public LeapHand(LeapHands hands, LiveDebug debug)
  {
    this.debug = debug;
    this.hands = hands;
  }

  public Vector3 Centre()
  {

    Vector palmPosition = GetHand().PalmPosition;
    Vector3 grabPosition = hands.ToUnityWorldSpace(palmPosition);

    // debug.Log("grab pos (rel to hand controller): " + localGrabPosition);
    debug.Log("grab pos (world space): " + grabPosition);
    return grabPosition;
  }

  public bool IsPresent()
  {
    return GetHand().IsValid;
  }

  public double GrabStrength()
  {
    return GetHand().GrabStrength;
  }

  private Hand GetHand()
  {
    return hands.GetHand();
  }

  private bool IsOpen()
  {
    return GrabStrength() < 0.5;
  }
  
  public bool IsClosed()
  {
    return !IsOpen();
  }

}
