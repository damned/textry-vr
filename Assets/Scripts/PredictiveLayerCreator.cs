using System;
using System.Collections.Generic;

public class PredictiveLayerCreator : ILayerCreator
{
  private readonly Letters letters;

  public PredictiveLayerCreator(Letters letters)
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