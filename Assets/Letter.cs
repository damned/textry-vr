﻿using System.Collections;
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

  public void Leave()
  {
    ChangeColour(Color.white);
    approached = false;
    grabbed = false;
    grabbingHand = null;
  }

  private void Grabbed(LeapHand hand)
  {
    grabbingHand = hand;
    ChangeColour(Color.red);

    // parent.GetComponent<SpringJoint>().connectedBody = hand.GetComponent<Rigidbody>();
    // gameObject.GetComponent<SpringJoint>().connectedBody = handRigidbody;

    // turns out even directly adding force to parent object didn't work:
    // parentRigidbody.AddForce(new Vector3(0f, 0f, 0.1f));

    // ...but suddenly by deleting all Rigidbody components in all parent's children, then things start moving...
    // ...but quite unstable
    // parent.GetComponent<SpringJoint>().connectedBody = handRigidbody;

    // instability - maybe interactables-follower causing lag?
    // not enough spring force?
    // looks like fairly consistent which objects grabbed....
    // i'm thinking palm position not v stable - why else have stabilizedPalmPosition()?
    // so maybe need to calc forces of off the stable value

    // now i think stability down to hand bashing around interactables rigidbody on collision with letter
    // turn off colliders i get nothing
    // still means that interactables parent object not responding to physical impetus directly
    // yes, proved this by setting colliders to non-trigger and turning off all forces

    // parent.GetComponent<FixedJoint>().connectedBody = hand.GetComponent<Rigidbody>();

    // yeah so basically, could NOT get joint connection to interactables to work,
    // what did work was setting interactables rigidbody to kinematic and moving it
    // with translate...

    // ..but one last try - what if the lack of collider on interactables is causing issue?
    // no, adding that did not make joints work...
    // nor did applying force, just not moving...
    // didn't help trying to move the object away from floor... but maybe it is because it is somehow overlapping
    // and therefore locked to another object??

    // actually late on i found that the hand palm position was off and weird - and maybe not changing...
    // so that would explain a lot!

    // ah well, in the end just gonna have to go kinematic...

    grabbed = true;
  }

  private void ChangeColour(Color color)
  {
    Material material = gameObject.GetComponent<Renderer>().material;
    material.color = color;
  }

}
