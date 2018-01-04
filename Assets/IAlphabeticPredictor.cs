using System.Collections.Generic;

public interface IAlphabeticPredictor
{
    List<string> LettersAfter(string lastLetter);
}