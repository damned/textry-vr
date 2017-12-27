
using System;
using UnityEngine;
using VRTK;

public class VrtkHand : MonoBehaviour, IHand
{
  private VrtkHands hands;
  public LiveDebug debug;

  private float grabStrength = 0f;
  
  private HandSide side;

  void Start()
  {
    HookUpEvents();
    RecognizeSide();
  }

  private void RecognizeSide()
  {
    if (gameObject.name.StartsWith("Left")) {
      side = HandSide.Left;
    }
    else {
      side = HandSide.Right;
    }
  }

    public VrtkHand(VrtkHands hands)
  {
    this.hands = hands;
  }

  public Vector3 Centre()
  {
    GameObject controllerHand;
    if (side == HandSide.Right) {
      controllerHand = VRTK_DeviceFinder.GetControllerRightHand(true);
    }
    else {
      controllerHand = VRTK_DeviceFinder.GetControllerLeftHand(true);      
    }

    return ControllerTransform(controllerHand).position;
  }

  public bool IsPresent()
  {
    return true;
  }

  public double GrabStrength()
  {
    return grabStrength;
  }
  

    private void HookUpEvents()
    {
        GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
      // Debug.Log("pressed");
      grabStrength = 1f;
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
      // Debug.Log("released");
      grabStrength = 0f;
    }

    private Transform ControllerTransform(GameObject controller)
    {
        GameObject controllerModel = controller;
        GameObject controllerParent = controllerModel.transform.parent.gameObject;

        if (controllerModel.name.StartsWith("Controller") && controllerParent.name.Equals("[CameraRig]")) {
            // Debug.Log("We appear to have controller reference within sensed play area");
        }
        else {
            Debug.LogError("Hmm controller event hierarchy wasn't as we expected :/  was trying to find moving controller whose local position reflected un-transformed sensed location");
            Debug.LogError("expected controller: " + controllerModel.name);
            Debug.LogError("expected play area object (camera rig): " + controllerParent.name);
        }

        Transform controllerTransform = controllerModel.transform;

        // Debug.Log("clicked! at " + controllerTransform.position + " (local " + controllerTransform.localPosition + ")");

        return controllerTransform;
    }

    public HandSide Side()
    {
        return side;
    }

    public bool IsClosed()
    {
        return !IsOpen();
    }

    private bool IsOpen()
    {
      return grabStrength == 0f;
    }
}
