using System;
using System.Collections.Generic;
using UnityEngine;

public enum KnobHandlingState { Unhandled, Touched, Grabbed }

public class Knob
{

  private static Dictionary<KnobHandlingState, Color> handlingStateColors = new Dictionary<KnobHandlingState, Color>()
  {
    { KnobHandlingState.Unhandled, Color.yellow },
    { KnobHandlingState.Touched, Color.black },
    { KnobHandlingState.Grabbed, Color.red }
  };

  public int id;
  private GameObject gameObject;

  private static int knobId = 0;

  private readonly Letter letter;

  public KnobHandlingState HandlingState { get; private set; }
  private readonly Knobs knobs;

  public int Layer { get; }

  public Knob(Knobs knobs, GameObject gameObject, Vector3 where, int layer)
  {
    this.Layer = layer;
    this.knobs = knobs;
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

  public bool Deleted { get; internal set; }
  public bool Faded { get; private set; }

  public void GrabbingHandMove(Vector3 handPosition)
  {
    Debug.Log("hand pos: " + handPosition);
    Debug.Log("knob pos: " + Position());

    float tolerance = 0.001f;
    Vector3 offset = handPosition - Position();
    if (offset.magnitude > tolerance)
    {
      knobs.Move(offset);
    }
  }

  public void Grab()
  {
    HandlingState = KnobHandlingState.Grabbed;
    UpdateColor();
    knobs.OnKnobStateChange();
  }

  public void Delete()
  {
    if (Application.isEditor)
    {
      UnityEngine.Object.DestroyImmediate(gameObject);
    }
    else
    {
      UnityEngine.Object.Destroy(gameObject);
    }
    Deleted = true;
  }

  public void Touch()
  {
    HandlingState = KnobHandlingState.Touched;
    UpdateColor();
  }

  public void Leave()
  {
    HandlingState = KnobHandlingState.Unhandled;
    UpdateColor();
  }

  public Vector3 Position()
  {
    return letter.Position();
  }

  public void ChangeColour(Color color)
  {
    Renderer renderer = gameObject.GetComponent<Renderer>();
    if (renderer == null)
    {
      return; // test mode
    }
    Material material = new Material(Shader.Find("Standard"));
    TryToSetTransparentRenderMode(material);
    material.color = color;
    renderer.sharedMaterial = material;
  }

  private static void TryToSetTransparentRenderMode(Material material)
  {
    // https://forum.unity.com/threads/access-rendering-mode-var-on-standard-shader-via-scripting.287002/#post-1961025
    material.SetFloat("_Mode", 2);
    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
    material.SetInt("_ZWrite", 0);
    material.DisableKeyword("_ALPHATEST_ON");
    material.EnableKeyword("_ALPHABLEND_ON");
    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    material.renderQueue = 3000;
  }

  public void Fade(float fadeLevel)
  {
    ChangeColour(new Color(1f, 1f, 1f, fadeLevel));
    Faded = true;
  }

  public string Text()
  {
    return letter.letter;
  }

  public override string ToString()
  {
    return "" + id + ": '" + Text() + "'@" + Position();
  }

  public void UpdateColor()
  {
    ChangeColour(handlingStateColors[HandlingState]);
  }
}