using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Knobs : MonoBehaviour
{
  public delegate void KnobHandler(Knob knob);

  public float fadeLevel = 0.05f;

  private List<Knob> knobs = new List<Knob>();
  private Nullable<Vector3> initialPosition = new Nullable<Vector3>();

  public int LayerCount { get; private set; }

  // extract Layer
  public Knob Create(Letter letter, float x, float y, float z, int layer)
  {
    Knob knob = new Knob(this, Instantiate(letter.gameObject, transform), new Vector3(x, y, z), layer);
    knobs.Add(knob);
    knob.UpdateColor();
    if (layer > LayerCount - 1)
    {
      LayerCount = layer + 1;
    }
    return knob;
  }

  public void MoveInZ(float z)
  {
    transform.Translate(new Vector3(0f, 0f, z));
  }


  public void OnKnobStateChange()
  {
    if (AnyGrabbed())
    {
      Fade(UnhandledKnobs());
    }
  }

  private void Fade(List<Knob> unhandledKnobs)
  {
    foreach (var knob in unhandledKnobs)
    {
      knob.Fade(fadeLevel);
    }
  }

  private List<Knob> UnhandledKnobs()
  {
    var unhandledKnobs = new List<Knob>();
    foreach (var knob in knobs)
    {
      // maybe better to distribute context event info and let knob fade itself
      if (knob.HandlingState == KnobHandlingState.Unhandled)
      {
        unhandledKnobs.Add(knob);
      }
    }

    return unhandledKnobs;
  }

  private bool AnyGrabbed()
  {
    return knobs.Any(k => k.HandlingState == KnobHandlingState.Grabbed);
  }

  public void ForEach(KnobHandler handler)
  {
    foreach (var knob in knobs)
    {
      handler(knob);
    }
  }

  public Knob FindClosestTo(Vector3 handPosition)
  {
    Vector3 distance = new Vector3(1, 0.0f, 0.0f);

    Knob closest = null;

    knobs.ForEach(knob => {
      Vector3 new_distance = handPosition - knob.Position();
      if (new_distance.magnitude < distance.magnitude)
      {
        closest = knob;
        distance = new_distance;
      }
    });
    return closest;
  }

  public void Reset()
  {
    RemoveKnobs(knobs.Where(knob =>
    {
      return knob.Layer != 0;
    }));
    LayerCount = 1;
    ResetToInitialPosition();
  }

  public void RemoveLayer()
  {
    RemoveKnobs(knobs.Where(knob =>
    {
      return knob.Layer == LayerCount;
    }));
    LayerCount -= 1;
  }

  private void RemoveKnobs(IEnumerable<Knob> knobsToRemove)
  {
    foreach (var knob in knobsToRemove)
    {
      knob.Delete();
    };
    knobs.RemoveAll(knob => knobsToRemove.Contains(knob));
  }
  private void CaptureInitialPosition()
  {
    if (!initialPosition.HasValue)
    {
      initialPosition = transform.localPosition;
    }
  }

  private void ResetToInitialPosition()
  {
    if (initialPosition.HasValue)
    {
      transform.localPosition = initialPosition.Value;
    }
  }
}