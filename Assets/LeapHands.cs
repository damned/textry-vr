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

  void Start()
  {
    controller = GetComponent<HandController>();
    debug = GetComponent<LiveDebug>();
  }

  void Update()
  {
    debug.Log("r confidence: " + Frame().Hands.Rightmost.Confidence);
  }

  internal Frame Frame()
  {
    return controller.GetFrame();
  }

  internal Vector3 HandCentre()
  {
    var hand = GetHand();

    Vector3 localGrabPosition = hand.StabilizedPalmPosition.ToUnityScaled();
    Vector3 grabPosition = controller.transform.TransformPoint(localGrabPosition);

    // debug.Log("grab pos (rel to hand controller): " + localGrabPosition);
    debug.Log("grab pos (world space): " + grabPosition);
    return grabPosition;
  }

  internal bool HandIsPresent()
  {
    return GetHand().IsValid;
  }

  internal double GrabStrength()
  {
    return GetHand().GrabStrength;
  }

  internal Hand GetHand()
  {
    return Frame().Hands.Rightmost;
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