using System;
using UnityEngine;

public class Knob
{
  public int id;
  private GameObject gameObject;

  private static int knobId = 0;

  private readonly Letter letter;

  public bool approached;
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
}