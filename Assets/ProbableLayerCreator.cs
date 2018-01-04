using System;
using System.Collections.Generic;

public enum LetterType { Vowel, Consonant };

public class ProbableLayerCreator : ILayerCreator
{
  public readonly Letters letters;
  private readonly Dictionary<string, Letter> lookup;
  private static List<string> vowels = new List<string>() 
  {
    "a", "e", "i", "o", "u"
  };

  public ProbableLayerCreator(Letters letters)
  {
    this.letters = letters;
    this.lookup = new Dictionary<string, Letter>();
    letters.ForEach(letter =>
    {
      lookup[letter.letter] = letter;
    });
  }

  public List<Letter> LayerLetters(string lastLetter)
  {
    var layerLetters = new List<Letter>();
    if (lastLetter == "")
    {
      return AllLetters(layerLetters);
    }
    if (vowels.Contains(lastLetter))
    {
      return Consonants(layerLetters);
    }
    return Vowels(layerLetters);
  }

  private List<Letter> AllLetters(List<Letter> layerLetters)
  {
    letters.ForEach(letter =>
    {
      layerLetters.Add(letter);
    });
    return layerLetters;
  }

  private List<Letter> Consonants(List<Letter> layerLetters)
  {
    letters.ForEach(letter =>
    {
      if (!vowels.Contains(letter.letter))
      {
        layerLetters.Add(letter);
      };
    });
    return layerLetters;
  }

  private List<Letter> Vowels(List<Letter> layerLetters)
  {
    letters.ForEach(letter =>
    {
      if (vowels.Contains(letter.letter))
      {
        layerLetters.Add(letter);
      };
    });
    return layerLetters;
  }
}