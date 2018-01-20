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
    var probableWords = ProbableWords(previousLetters);
    return LettersAt(previousLetters.Length, probableWords).Distinct().ToList();
  }

  private static IEnumerable<string> LettersAt(int index, IEnumerable<string> probableWords)
  {
    return probableWords.Select(word => word[index].ToString());
  }

  private IEnumerable<string> ProbableWords(string previousLetters)
  {
    return words.Where(w => w.StartsWith(previousLetters));
  }

  private static List<string> AToZ()
  {
    return "abcdefghijklmnopqrstuvwxyz".Select(ch => ch.ToString()).ToList();
  }
}