using System;
using UnityEngine;

public class Knobs : MonoBehaviour
{
  public float fadeLevel = 0.4f;

  public Knob Create(Letter letter, float x, float y, float z)
  {
    return new Knob(Instantiate(letter.gameObject, transform), new Vector3(x, y, z));
  }

  public void MoveAway()
  {
    Debug.Log("happening!!");
    transform.Translate(new Vector3(0f, 0f, 0.01f));
  }

  public void MoveCloser()
  {
    Debug.Log("happening!");
    transform.Translate(new Vector3(0f, 0f, -0.01f));
  }

  public void FadeOtherKnobs(GameObject referenceLetter)
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

  // move to Knob instances
  public void ForEach(Letters.LetterHandler handler)
  {
    foreach (Transform letterTransform in transform)
    {
      handler(letterTransform.gameObject.GetComponent<Letter>());
    }
  }


}