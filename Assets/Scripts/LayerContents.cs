using System.Collections.Generic;

public class LayerContents
{
    public List<Letter> letters;
    public List<List<Letter>> suggestions = new List<List<Letter>>();

    public LayerContents(List<Letter> letters)
    {
        this.letters = letters;
    }
}