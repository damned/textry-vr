using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class HandTextWriter : MonoBehaviour
{

  public float range = 2f;
  public float fadeLevel = 0.4f;

  private Collider lastGrabbed = null;
  private GameObject interactables;
  private LiveDebug debug;
  private LeapHands hands;

  private int layer = 0;
  private string text = "";

  void Start()
  {
    hands = GetComponent<LeapHands>();
    interactables = GameObject.Find("interactables");
    debug = GetComponent<LiveDebug>();

    PlaceLetters(GameObject.Find("letters"), interactables, 0f);
  }

  void Update()
  {
    debug.Clear();

    // debug.Log("hands presence: " + HandsPresence(frame.Hands));
    // debug.Log("hands info: " + HandsInfo(frame.Hands));

    DetectClosestGrabbed();
  }

  private void DetectClosestGrabbed()
  {
    if (!hands.HandIsPresent())
    {
      ClearLastGrabbed(null);
      return;
    }

    debug.Log("text: " + text);

    var grabPosition = hands.Hand().Centre();

    Collider[] close_things = Physics.OverlapSphere(grabPosition, range);
    Vector3 distance = new Vector3(1, 0.0f, 0.0f);

    Collider grabbed = null;

    for (int j = 0; j < close_things.Length; ++j)
    {
      Vector3 new_distance = grabPosition - close_things[j].transform.position;
      if (new_distance.magnitude < distance.magnitude &&
          !close_things[j].transform.IsChildOf(hands.controller.transform))
      {
        grabbed = close_things[j];
        distance = new_distance;
      }
    }
    // debug.Log("Grabbed: " + grabbed);
    ClearLastGrabbed(grabbed);
    if (grabbed == null)
    {
      // debug.Log("Nearly grabbed things: " + string.Join(", ", close_things.ToList().Select(t => t.gameObject.name).ToArray()));
      return;
    }

    // debug.Log("Grabbed: " + grabbed.gameObject.name);
    if (hands.GrabStrength() > 0.7)
    {
      if (grabbed != lastGrabbed)
      {
        LetterOf(grabbed).Grab(hands.Hand());
        FadeOtherLetters(grabbed.gameObject);
        layer += 1;
        text += LetterOf(grabbed).letter;
        PlaceLetters(GameObject.Find("letters"), interactables, layer * 0.2f);
        lastGrabbed = grabbed;
      }
    }
    else if (hands.GrabStrength() < 0.4)
    {
      LetterOf(grabbed).Approach();
    }
  }

  private void FadeOtherLetters(GameObject referenceLetter)
  {
    foreach (Transform letterTransform in interactables.transform)
    {
      GameObject letter = letterTransform.gameObject;
      if (letter != referenceLetter)
      {
        letter.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, fadeLevel);
      }
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

  private void PlaceLetters(GameObject letters, GameObject interactables, float zOffset)
  {
    var ySpacing = 0.06f;
    var xSpacing = 0.1f;
    var yOffset = -1.45f;
    var xOffset = 0f;
    var slots = 6;
    var index = 0;
    var xStart = -(xSpacing * slots) / 2;
    var yStart = -(ySpacing * slots) / 2;
    var x = xStart + xOffset;
    var y = yStart + yOffset;
    var xIndex = 0;
    var z = -1f + zOffset;

    string placement = "placed: ";
    foreach (Transform letterTransform in letters.transform)
    {
      var letter = letterTransform.gameObject;
      var interactable = Instantiate(letter, interactables.transform);
      xIndex = index % slots;
      if (xIndex == 0)
      {
        y -= ySpacing;
        x = xStart + xOffset;
      }
      else
      {
        x += xSpacing;
      }
      index += 1;
      interactable.transform.localPosition = new Vector3(x, y, z);
      placement += interactable.name + ", ";
    }
    debug.Log(placement);
  }
}
