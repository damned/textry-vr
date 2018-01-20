using System.Collections.Generic;

public class LayerContents
{
    public List<Letter> letters;
    public List<Letter> suggestion = new List<Letter>();

    public LayerContents(List<Letter> letters)
    {
        this.letters = letters;
    }
}