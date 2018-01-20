using System;
using System.Collections.Generic;
using System.Linq;

public class ProbableAlphabeticPredictor : IAlphabeticPredictor
{
  private static List<string> vowels = new List<string>()
  {
    "a", "e", "i", "o", "u"
  };

  private static List<string> alphabet = AToZ();

  public List<string> LettersAfter(string previousLetters)
  {
    if (previousLetters == "")
    {
      return FilteredLetters(letter => true);
    }
    var lastLetter = previousLetters[previousLetters.Length - 1].ToString();
    if (vowels.Contains(lastLetter))
    {
      return FilteredLetters(ConsonantsFilter());
    }
    return FilteredLetters(VowelsFilter());
  }

  private Func<string, bool> ConsonantsFilter()
  {
    return letter => !vowels.Contains(letter);
  }

  private static Func<string, bool> VowelsFilter()
  {
    return letter => vowels.Contains(letter);
  }
  private List<string> FilteredLetters(Func<string, bool> filter)
  {
    var layerLetters = new List<string>();
    alphabet.ForEach(letter =>
    {
      if (filter(letter))
      {
        layerLetters.Add(letter);
      };
    });
    return layerLetters;
  }

  private static List<string> AToZ()
  {
    return "abcdefghijklmnopqrstuvwxyz".Select(ch => ch.ToString()).ToList();
  }
}