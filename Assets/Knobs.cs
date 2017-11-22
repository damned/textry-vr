using System;
using UnityEngine;

public class Knobs : MonoBehaviour
{
  public Knob Create(Letter letter, float x, float y, float z)
  {
    return new Knob(Instantiate(letter.gameObject, transform), new Vector3(x, y, z));
  }

  public void FadeOtherKnobs(GameObject referenceLetter, float fadeLevel)
  {
    foreach (Transform letterTransform in transform)
    {
      GameObject letter = letterTransform.gameObject;
      if (letter != referenceLetter)
      {
        letter.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, fadeLevel);
      }
    }
  }

}