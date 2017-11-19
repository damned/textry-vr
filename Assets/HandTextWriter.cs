﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using Leap;

public class HandTextWriter : MonoBehaviour {
	
	public Text displayText;
	public GameObject target;
	public float range = 2f;

	private HandController controller;

	void Start () {
		controller = GetComponent<HandController>();

		PlaceLetters(GameObject.Find("letters"), GameObject.Find("interactables"));
	}
	
	void Update () {
		ResetDisplay();

		var frame = controller.GetFrame();
		var points = frame.Pointables;
		Debug("points count: " + points.Count);
		Debug("hands presence: " + HandsPresence(frame.Hands));
		Debug("hands info: " + HandsInfo(frame.Hands));

		DetectClosestGrabbed(frame.Hands.Rightmost);
	}

  private void DetectClosestGrabbed(Hand hand)
	{
		if (!hand.IsValid)
		{
			return;
		}

		Vector3 localGrabPosition = hand.PalmPosition.ToUnityScaled();
		Vector3 grabPosition = controller.transform.TransformPoint(localGrabPosition);

		Debug("grab pos (rel to hand controller): " + localGrabPosition);
		Debug("grab pos (world space): " + grabPosition);

		Collider[] close_things = Physics.OverlapSphere(grabPosition, range);
    Vector3 distance = new Vector3(1, 0.0f, 0.0f);

		Collider grabbed_ = null;

    for (int j = 0; j < close_things.Length; ++j) {
      Vector3 new_distance = grabPosition - close_things[j].transform.position;
      if (close_things[j].GetComponent<Rigidbody>() != null && new_distance.magnitude < distance.magnitude &&
          !close_things[j].transform.IsChildOf(transform)) {
        grabbed_ = close_things[j];
        distance = new_distance;
      }
    }
    Debug("Grabbed: " + grabbed_);
		if (grabbed_ == null)
		{
	    Debug("Nearly grabbed things: " + string.Join(", ", close_things.ToList().Select(t => t.gameObject.name).ToArray()));
			return;
		}
    Debug("Grabbed: " + grabbed_.gameObject.name);
		Material material = grabbed_.gameObject.GetComponent<Renderer>().material;
		if (hand.GrabStrength > 0.7) {
			material.color = Color.red;
		}
		else if (hand.GrabStrength < 0.2) {
			material.color = Color.black;
		}
	}

	private void PlaceLetters(GameObject letters, GameObject interactables)
	{
		var spacing = 0.12f;
		var slots = 5;
		var index = 0;
		var start = -(spacing * slots) / 2;
		var x = start;
		var y = start + spacing;
		var xIndex = 0;
		var z = 0f;

		string placement = "placed: ";
		foreach (Transform letterTransform in letters.transform)
		{
				var letter = letterTransform.gameObject;
				var interactable = Instantiate(letter, interactables.transform);
				xIndex = index % slots; 
				if (xIndex == 0)
				{ 
					y += spacing;
					x = start;
				}
				else
				{
					x += spacing;
				}
				index += 1;
				interactable.transform.localPosition = new Vector3(x, y, z);
				placement += interactable.name + ", ";
		}
		Debug(placement);
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

	private void Debug(string message)
	{
		displayText.text += message + "\n";
	}

	private void ResetDisplay()
	{
		displayText.text = "";
	}
}
