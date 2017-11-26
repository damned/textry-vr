public class KnobArranger
{
  private Letters letters;
  private Knobs knobs;
  public int layers = 0;

  public KnobArranger(Letters letters, Knobs knobs)
  {
    this.letters = letters;
    this.knobs = knobs;
  }

  // this class managing physical layout, so should probs handle zoffset, just
  // know it's a new logical layer?
  public string Arrange(float zOffset)
  {
    layers += 1;
    var ySpacing = 0.06f;
    var xSpacing = 0.1f;
    var yOffset = -1.45f;
    var xOffset = 0f;
    var slots = 6;
    var index = 0;
    var xStart = -(xSpacing * slots) / 2;
    var yStart = -(ySpacing * slots) / 2;
    var x = xStart + xOffset;
    var y = yStart + yOffset;
    var xIndex = 0;
    var z = -1f + zOffset;

    string placement = "placed: ";
    letters.ForEach((letter) =>
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
      var knob = knobs.Create(letter, x, y, z);
      placement += knob.Name + ", ";
    });
    return placement;
  }
}