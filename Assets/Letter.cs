using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{

  public string letter;

  public bool approached;
  public bool grabbed;
  public LeapHand grabbingHand;

  public float Z()
  {
    return transform.position.z;
  }

  public Vector3 Position()
  {
    return transform.position;
  }

  public void ChangeColour(Color color)
  {
    Material material = gameObject.GetComponent<Renderer>().material;
    material.color = color;
  }

}
