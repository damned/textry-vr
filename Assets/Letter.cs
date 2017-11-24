using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{

  public string letter;

  public bool approached;
  public bool grabbed;
  public LeapHand grabbingHand;

  public  GameObject parent;

  void Start()
  {
    parent = transform.parent.gameObject;
  }

  public float Z()
  {
    return transform.position.z;
  }

  public Vector3 Position()
  {
    return transform.position;
  }

  public void MoveAway()
  {
    Debug.Log("happening!!");
    parent.transform.Translate(new Vector3(0f, 0f, 0.01f));
  }

  public void MoveCloser()
  {
    Debug.Log("happening!");
    parent.transform.Translate(new Vector3(0f, 0f, -0.01f));
  }

  public void ChangeColour(Color color)
  {
    Material material = gameObject.GetComponent<Renderer>().material;
    material.color = color;
  }

}
