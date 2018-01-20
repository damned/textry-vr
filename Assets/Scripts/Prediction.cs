using System.Collections.Generic;

public class Prediction
{
    public List<string> letters;
    public List<string> suggestions;

    public Prediction(List<string> letters)
    {
        this.letters = letters;
    }
}