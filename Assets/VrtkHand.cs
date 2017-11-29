
using UnityEngine;
using VRTK;

public class VrtkHand : MonoBehaviour, IHand
{
  private VrtkHands hands;
  public LiveDebug debug;

  private float grabStrength = 0f;
  

  void Start()
  {
    HookUpEvents();
  }

  public VrtkHand(VrtkHands hands)
  {
    this.hands = hands;
  }

  public Vector3 Centre()
  {
    GameObject controllerRightHand = VRTK_DeviceFinder.GetControllerRightHand(true);

    return ControllerTransform(controllerRightHand).position;
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
      Debug.Log("pressed");
      grabStrength = 1f;
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
      Debug.Log("released");
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




  
}
