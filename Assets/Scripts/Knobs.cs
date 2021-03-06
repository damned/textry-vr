using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Knobs : MonoBehaviour
{
    public delegate void KnobHandler(Knob knob);

    public float fadeLevel = 0.1f;

    private List<Knob> knobs = new List<Knob>();
    private Nullable<Vector3> initialPosition = new Nullable<Vector3>();
    public float wordOffset = 0.05f;
    public float knobTouchRadius = 0.05f;
    public int lastLayer = 1;

    // extract Layer
    public Knob Create(Letter letter, float x, float y, float z, int layer)
    {
        Knob knob = new Knob(this, Instantiate(letter.gameObject, transform), new Vector3(x, y, z), layer);
        knobs.Add(knob);
        knob.UpdateColor();

        lastLayer = Math.Max(lastLayer, layer);
        return knob;
    }

    // split letter and suggestion knob?
    public Knob CreateSuggestion(List<Letter> suggestionLetters, float x, float y, float z, int layer)
    {
        var suggestionParent = new GameObject();
        suggestionParent.transform.parent = transform;
        suggestionParent.transform.position = new Vector3(x, y, z);
        var letterIndex = -suggestionLetters.Count / 2;
        suggestionLetters.ForEach(letter =>
        {
            var letterObject = Instantiate(letter.gameObject, suggestionParent.transform);
            letterObject.transform.Translate(new Vector3(wordOffset - (0.03f * letterIndex), 0, 0));
            letterIndex++;
        });
        Debug.Log("suggestion parent: " + suggestionParent);
        Knob suggestionKnob = new Knob(this, suggestionParent, new Vector3(x, y, z), layer);
        knobs.Add(suggestionKnob);
        lastLayer = Math.Max(lastLayer, layer);
        return suggestionKnob;
    }

    public void Move(Vector3 translation)
    {
        CaptureInitialPosition();
        transform.Translate(translation);
    }


    public void OnKnobStateChange()
    {
        if (AnyGrabbed())
        {
            Fade(UnhandledKnobsNotInLastLayer());
        }
        else
        {
            Unfade(UnhandledKnobsNotInLastLayer());
        }
    }

    public void RemoveLayer(int layer)
    {
        Debug.Log($"layer to remove: {layer}, layers: " + String.Join(", ", knobs.Select(k => k.Layer.ToString()).Distinct().ToArray()));
        var knobsInLastLayer = knobs.Where(knob =>
        {
            return knob.Layer == layer;
        });
        foreach (var knob in knobsInLastLayer)
        {
            knob.Delete();
        };
        knobs.RemoveAll(knob => knobsInLastLayer.Contains(knob));
        lastLayer = layer - 1;
        OnKnobStateChange();
    }

    private void Fade(List<Knob> unhandledKnobs)
    {
        foreach (var knob in unhandledKnobs)
        {
            knob.Fade(fadeLevel);
        }
    }

    private void Unfade(List<Knob> unhandledKnobs)
    {
        foreach (var knob in unhandledKnobs)
        {
            knob.Unfade();
        }
    }

    private List<Knob> UnhandledKnobsNotInLastLayer()
    {
        var unhandledKnobs = new List<Knob>();
        foreach (var knob in knobs)
        {
            // maybe better to distribute context event info and let knob fade itself
            if (knob.HandlingState == KnobHandlingState.Unhandled && knob.Layer != lastLayer)
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

        knobs.ForEach(knob =>
        {
            Vector3 new_distance = handPosition - knob.Position();
            if (new_distance.magnitude < distance.magnitude)
            {
                closest = knob;
                distance = new_distance;
            }
        });
        if (distance.magnitude > knobTouchRadius)
        {
            return null;
        }
        return closest;
    }

    public void Reset()
    {
        var knobsNotInFirstLayer = knobs.Where(knob =>
        {
            return knob.Layer != 0;
        });
        foreach (var knob in knobsNotInFirstLayer)
        {
            knob.Delete();
        };
        knobs.RemoveAll(knob => knobsNotInFirstLayer.Contains(knob));
        lastLayer = 1;
        OnKnobStateChange();
        ResetToInitialPosition();
    }

    private void CaptureInitialPosition()
    {
        if (!initialPosition.HasValue)
        {
            initialPosition = transform.localPosition;
            Debug.Log("Initial knobs position: " + initialPosition.Value);
        }
    }

    private void ResetToInitialPosition()
    {
        Debug.Log("resetting");
        if (initialPosition.HasValue)
        {
            Debug.Log("Before reset knobs position: " + transform.localPosition);
            transform.localPosition = initialPosition.Value;
            Debug.Log("Reset knobs position: " + transform.localPosition);
        }
    }
}