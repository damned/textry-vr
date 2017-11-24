using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{

  public string letter;

  private bool approached;
  private bool grabbed;
  private LeapHand grabbingHand;
  private HandController controller;

  private GameObject parent;
  private Rigidbody parentRigidbody;

  // Use this for initialization
  void Start()
  {
    parent = transform.parent.gameObject;
  }

  // Update is called once per frame
  void Update()
  {

  }

  void FixedUpdate()
  {
    if (grabbed)
    {
      float tolerance = 0.01f;

      Vector3 grabPosition = grabbingHand.Centre();

      Debug.Log("hand z: " + grabPosition.z);
      Debug.Log("letter z: " + transform.position.z);
      if (grabPosition.z < (transform.position.z - tolerance))
      {
        Debug.Log("happening!");
        parent.transform.Translate(new Vector3(0f, 0f, -0.01f));
      }
      else if (grabPosition.z > (transform.position.z + tolerance))
      {
        Debug.Log("happening!!");
        parent.transform.Translate(new Vector3(0f, 0f, 0.01f));
      }
    }
  }

  public void Approach()
  {
    approached = true;
    ChangeColour(Color.black);
  }

  public void Grab(LeapHand hand)
  {
    this.controller = controller;
    if (approached && !grabbed)
    {
      Grabbed(hand);
    }
    approached = false;
  }

  // move state manage back up
  // expose change colour & pos info

  private void Grabbed(LeapHand hand)
  {
    grabbingHand = hand;
    ChangeColour(Color.red);

    grabbed = true;
  }

  public void Leave()
  {
    ChangeColour(Color.white);
    approached = false;
    grabbed = false;
    grabbingHand = null;
  }


  public void ChangeColour(Color color)
  {
    Material material = gameObject.GetComponent<Renderer>().material;
    material.color = color;
  }

}
