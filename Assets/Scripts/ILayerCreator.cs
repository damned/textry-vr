using System.Collections.Generic;

public interface ILayerCreator
{
  List<Letter> LayerLetters(string lastLetter);
}