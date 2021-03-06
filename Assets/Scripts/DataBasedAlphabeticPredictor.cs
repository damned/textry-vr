using System.Collections.Generic;
using System.Linq;

public class DataBasedAlphabeticPredictor : IAlphabeticPredictor
{
    private readonly List<string> words;

    public DataBasedAlphabeticPredictor(List<string> words)
    {
        this.words = words;
    }

    public Prediction PredictionAfter(string previousLetters)
    {
        if (previousLetters == "")
        {
            return new Prediction(AToZ());
        }
        var probableWords = ProbableWords(previousLetters);
        Prediction prediction = new Prediction(LettersAt(previousLetters.Length, probableWords).Distinct().ToList());
        return AddPredictions(prediction, probableWords);
    }

    private static Prediction AddPredictions(Prediction prediction, List<string> probableWords)
    {
        if (probableWords.Count < 5)
        {
            prediction.suggestions.AddRange(probableWords);
        }
        return prediction;
    }

    private static IEnumerable<string> LettersAt(int index, IEnumerable<string> probableWords)
    {
        return probableWords.Select(word => word[index].ToString());
    }

    private List<string> ProbableWords(string previousLetters)
    {
        return words.Where(w => w.StartsWith(previousLetters)).ToList();
    }

    private static List<string> AToZ()
    {
        return "abcdefghijklmnopqrstuvwxyz".Select(ch => ch.ToString()).ToList();
    }
}