using UnityEngine;

public class Knob
{
  private GameObject gameObject;

  public Knob(GameObject gameObject, Vector3 where)
  {
    this.gameObject = gameObject;
    this.gameObject.transform.localPosition = where;
  }

  public string Name { get {
    return gameObject.name;
  } }
}