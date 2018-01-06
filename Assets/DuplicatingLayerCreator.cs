using System;
using System.Collections.Generic;

public class DuplicatingLayerCreator : ILayerCreator
{
  private readonly Letters letters;

  public DuplicatingLayerCreator(Letters letters)
  {
    this.letters = letters;
  }

  public List<Letter> LayerLetters(string lastLetter)
  {
    var layerLetters = new List<Letter>();
    letters.ForEach(letter => layerLetters.Add(letter));
    return layerLetters;
  }
}