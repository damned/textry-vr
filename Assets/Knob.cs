using System;
using System.Collections.Generic;
using UnityEngine;

public enum KnobHandlingState { Unhandled, Touched, Grabbed }

public class Knob
{

  private static Dictionary<KnobHandlingState, Color> handlingStateColors = new Dictionary<KnobHandlingState, Color>()
  {
    { KnobHandlingState.Unhandled, Color.white },
    { KnobHandlingState.Touched, Color.black },
    { KnobHandlingState.Grabbed, Color.red }
  };

  public int id;
  private GameObject gameObject;

  private static int knobId = 0;

  private readonly Letter letter;

  public KnobHandlingState HandlingState { get; private set; }
  private readonly Knobs knobs;

  public Knob(Knobs knobs, GameObject gameObject, Vector3 where)
  {
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

  public void GrabbingHandMove(Vector3 handPosition)
  {
    Debug.Log("hand z: " + handPosition.z);
    Debug.Log("knob z: " + Z());

    float tolerance = 0.01f;
    if (handPosition.z < (Z() - tolerance))
    {
      knobs.MoveCloser();
    }
    else if (handPosition.z > (Z() + tolerance))
    {
      knobs.MoveAway();
    }
  }

  private float Z()
  {
    return Position().z;
  }

  public void Grab()
  {
    HandlingState = KnobHandlingState.Grabbed;
    UpdateColor();
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
  }

  public string Text()
  {
    return letter.letter;
  }

  public override string ToString()
  {
    return "" + id + ": '" + Text() + "'@" + Position();
  }

  private void UpdateColor()
  {
    ChangeColour(handlingStateColors[HandlingState]);
  }
}