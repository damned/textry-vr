using System;
using System.Collections.Generic;
using UnityEngine;

public class Knobs : MonoBehaviour
{
  public delegate void KnobHandler(Knob knob);

  public float fadeLevel = 0.4f;

  private List<Knob> knobs = new List<Knob>();

  public Knob Create(Letter letter, float x, float y, float z)
  {
    Knob knob = new Knob(Instantiate(letter.gameObject, transform), new Vector3(x, y, z));
    knobs.Add(knob);
    return knob;
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

  public void FadeOtherKnobs(Knob referenceKnob)
  {
    foreach (var knob in knobs)
    {
      if (knob != referenceKnob)
      {
        knob.Fade(fadeLevel);
      }
    }
  }

  public void ForEach(KnobHandler handler)
  {
    foreach (var knob in knobs)
    {
      handler(knob);
    }
  }
}