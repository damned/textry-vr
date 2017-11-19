using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap;

public class HandTextWriter : MonoBehaviour {
	
	public Text displayText;

	void Start () {
		
	}
	
	void Update () {
		ResetDisplay();
		var controller = GetComponent<HandController>();
		var frame = controller.GetFrame();
		var points = frame.Pointables;
		Debug("points count: " + points.Count);
		Debug("hands: " + HandsInfo(frame.Hands));
	}

  private string HandsInfo(HandList hands)
	{
		return "l: " + HandInfo(hands.Leftmost) + "; r: " + HandInfo(hands.Rightmost);
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
