using System.Collections;
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
	private Collider lastGrabbed = null;
	private GameObject interactables;

	void Start () {
		controller = GetComponent<HandController>();
		interactables = GameObject.Find("interactables");

		PlaceLetters(GameObject.Find("letters"), interactables);
	}
	
	void Update () {
		ResetDisplay();

		var interactablesRigidbody = interactables.GetComponent<Rigidbody>();
		Debug("interactables rigidbody, sleeping? " + interactablesRigidbody.IsSleeping());
		interactablesRigidbody.WakeUp();
		Debug("interactables rigidbody, now sleeping? " + interactablesRigidbody.IsSleeping());

		var frame = controller.GetFrame();
		Debug("r confidence: " + frame.Hands.Rightmost.Confidence);
		Debug("hands presence: " + HandsPresence(frame.Hands));
		Debug("hands info: " + HandsInfo(frame.Hands));

		DetectClosestGrabbed(frame.Hands.Rightmost);
	}

  private void DetectClosestGrabbed(Hand hand)
	{
		if (!hand.IsValid)
		{
			ClearLastGrabbed(null);
			return;
		}

		Vector3 localGrabPosition = hand.PalmPosition.ToUnityScaled();
		Vector3 grabPosition = controller.transform.TransformPoint(localGrabPosition);

		Debug("grab pos (rel to hand controller): " + localGrabPosition);
		Debug("grab pos (world space): " + grabPosition);

		Collider[] close_things = Physics.OverlapSphere(grabPosition, range);
    Vector3 distance = new Vector3(1, 0.0f, 0.0f);

		Collider grabbed = null;

    for (int j = 0; j < close_things.Length; ++j) {
      Vector3 new_distance = grabPosition - close_things[j].transform.position;
      if (new_distance.magnitude < distance.magnitude &&
          !close_things[j].transform.IsChildOf(controller.transform)) {
        grabbed = close_things[j];
        distance = new_distance;
      }
    }
    Debug("Grabbed: " + grabbed);
		ClearLastGrabbed(grabbed);
		if (grabbed == null)
		{
	    Debug("Nearly grabbed things: " + string.Join(", ", close_things.ToList().Select(t => t.gameObject.name).ToArray()));
			return;
		}
		lastGrabbed = grabbed;
    Debug("Grabbed: " + grabbed.gameObject.name);
		if (hand.GrabStrength > 0.7) {
			LetterOf(grabbed).Grab(controller.rightPhysicsModel.palm.gameObject);
		}
		else if (hand.GrabStrength < 0.4) {
			LetterOf(grabbed).Approach();
		}
	}

  private void ClearLastGrabbed(Collider grabbed)
	{
		if (lastGrabbed != null && lastGrabbed != grabbed)
		{
			LetterOf(lastGrabbed).Leave();
			lastGrabbed = null;
		} 
	}

	private Letter LetterOf(Collider collider)
	{
		return collider.gameObject.GetComponent<Letter>();
	}

	private void PlaceLetters(GameObject letters, GameObject interactables)
	{
		var spacing = 0.08f;
		var yOffset = 0.7f;
		var xOffset = 2f;
		var slots = 6;
		var index = 0;
		var start = -(spacing * slots) / 2;
		var x = start + xOffset;
		var y = start + yOffset;
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
					y -= spacing;
					x = start + xOffset;
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
