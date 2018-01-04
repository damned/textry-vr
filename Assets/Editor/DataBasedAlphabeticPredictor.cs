using System.Collections.Generic;
using System.Linq;

public class DataBasedAlphabeticPredictor : IAlphabeticPredictor
{
  private readonly List<string> words;

  public DataBasedAlphabeticPredictor(List<string> words)
  {
    this.words = words;
  }

  public List<string> LettersAfter(string previousLetters)
  {
    if (previousLetters == "")
    {
      return AToZ();
    }
    var next = new List<string>();
    next.Add(ProbableWord(previousLetters)[previousLetters.Length].ToString());
    return next;
  }

  private string ProbableWord(string previousLetters)
  {
    return words.Find(w => w.StartsWith(previousLetters));
  }

  private static List<string> AToZ()
  {
    return "abcdefghijklmnopqrstuvwxyz".Select(ch => ch.ToString()).ToList();
  }
}