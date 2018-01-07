using System;

public class KnobArranger
{
  private Letters letters;
  private Knobs knobs;
  public int layers = 0;
  private readonly ILayerCreator layerCreator;

  public KnobArranger(Letters letters, Knobs knobs, ILayerCreator layerCreator)
  {
    this.layerCreator = layerCreator;
    this.letters = letters;
    this.knobs = knobs;
  }

  // this class managing physical layout, so should probs handle zoffset, just
  // know it's a new logical layer?
  public string Arrange(float zOffset, string lastLetter = "")
  {
    layers += 1;
    var ySpacing = 0.05f;
    var xSpacing = 0.08f;
    var yOffset = -0.2f;
    var xOffset = 0f;
    var slots = 6;
    var index = 0;
    var xStart = -(xSpacing * slots) / 2;
    var yStart = -(ySpacing * slots) / 2;
    var x = xStart + xOffset;
    var y = yStart + yOffset;
    var xIndex = 0;
    var z = -1f + zOffset;

    var layerLetters = layerCreator.LayerLetters(lastLetter);

    string placement = "placed: ";
    layerLetters.ForEach((letter) =>
    {
      xIndex = index % slots;
      if (xIndex == 0)
      {
        y -= ySpacing;
        x = xStart + xOffset;
      }
      else
      {
        x += xSpacing;
      }
      index += 1;
      var knob = knobs.Create(letter, x, y, z, layers - 1);
      placement += knob.Name + ", ";
    });
    return placement;
  }

  public void ResetLayers()
  {
    knobs.Reset();
    layers = 1;
  }
}