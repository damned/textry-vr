using System;
using UnityEngine;

public class Knob
{
  public int id;
  private GameObject gameObject;

  private static int knobId = 0;

  private readonly Letter letter;

  public bool approached;
  public bool grabbed;
  public IHand grabbingHand;


  public Knob(GameObject gameObject, Vector3 where)
  {
    this.id = (++knobId);
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
    Renderer renderer = gameObject.GetComponent<Renderer>();
    Material material = new Material(Shader.Find("Standard"));
    material.color = color;
    renderer.sharedMaterial = material;
  }

  public void Fade(float fadeLevel)
  {
    ChangeColour(new Color(1f, 1f, 1f, fadeLevel));
  }

  public string Text()
  {
    return letter.letter;
  }

  public override string ToString()
  {
    return "" + id + ": '" + Text() + "'@" + Position();
  }
}