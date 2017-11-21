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

  void Start () {
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
}

