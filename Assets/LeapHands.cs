using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using Leap;
using System;

public class LeapHands : MonoBehaviour
{
  public HandController controller;

  private LiveDebug debug;
  private LeapHand hand;

  public delegate void HandHandler(LeapHand hand);
  public event HandHandler OnHandUpdate;

  void Start()
  {
    controller = GetComponent<HandController>();
    debug = GetComponent<LiveDebug>();
    hand = new LeapHand(this, debug);
  }

  void Update()
  {
    debug.Log("r confidence: " + Frame().Hands.Rightmost.Confidence);
    OnHandUpdate(hand);
  }

  public Vector3 ToUnityWorldSpace(Vector leapSpacePosition)
  {
    Vector3 unityLocalPosition = leapSpacePosition.ToUnityScaled();
    return ToWorldSpace(unityLocalPosition);
  }

  private Vector3 ToWorldSpace(Vector3 localUnityScaledPosition)
  {
    return controller.transform.TransformPoint(localUnityScaledPosition);
  }

  internal Hand GetHand()
  {
    return Frame().Hands.Rightmost;
  }

  private Frame Frame()
  {
    return controller.GetFrame();
  }

  private string HandsInfo(HandList hands)
  {
    return "l: " + HandInfo(hands.Leftmost) + "; r: " + HandInfo(hands.Rightmost);
  }

  private string HandsPresence(HandList hands)
  {
    return "l: " + hands.Leftmost.IsValid + "; r: " + hands.Rightmost.IsValid;
  }

  private string HandInfo(Hand hand)
  {
    return string.Format("grab: {0:F1}, pinch: {0:F1}", hand.GrabStrength, hand.PinchStrength);
  }

}