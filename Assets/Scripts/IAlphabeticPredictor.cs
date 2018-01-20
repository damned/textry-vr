using System.Collections.Generic;

public interface IAlphabeticPredictor
{
    Prediction PredictionAfter(string previousLetters);
}