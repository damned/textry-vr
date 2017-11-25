using System;
using UnityEngine;

public class Knob
{
  private GameObject gameObject;

  private readonly Letter letter;

  public bool approached;
  public bool grabbed;
  public IHand grabbingHand;


  public Knob(GameObject gameObject, Vector3 where)
  {
    this.gameObject = gameObject;
    this.gameObject.transform.localPosition = where;
    this.letter = gameObject.GetComponent<Letter>();
  }

  public string Name
  {
    get
    {
      return gameObject.name;
    }
  }


  public float Z()
  {
    return Position().z;
  }

  public Vector3 Position()
  {
    return letter.Position();
  }

  public void ChangeColour(Color color)
  {
    Material material = gameObject.GetComponent<Renderer>().material;
    material.color = color;
  }

  public void Fade(float fadeLevel)
  {
    gameObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, fadeLevel);
  }

  public string Text()
  {
    return letter.letter;
  }
}