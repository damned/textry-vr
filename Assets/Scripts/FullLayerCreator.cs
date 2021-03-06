using System;
using System.Collections.Generic;

public class FullLayerCreator : ILayerCreator
{
  private readonly Letters letters;

  public FullLayerCreator(Letters letters)
  {
    this.letters = letters;
  }

  public LayerContents NextLayer(string lastLetter)
  {
    var layerLetters = new List<Letter>();
    letters.ForEach(letter => layerLetters.Add(letter));
    return new LayerContents(layerLetters);
  }
}