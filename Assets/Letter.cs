using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{

  public string letter;

  public bool approached;
  public bool grabbed;
  public LeapHand grabbingHand;

  private GameObject parent;
  private Rigidbody parentRigidbody;

  void Start()
  {
    parent = transform.parent.gameObject;
  }

  void Update()
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

  // move state manage back up
  // expose change colour & pos info

  public void ChangeColour(Color color)
  {
    Material material = gameObject.GetComponent<Renderer>().material;
    material.color = color;
  }

}
