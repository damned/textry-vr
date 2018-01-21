using System;
using System.Collections.Generic;
using System.Linq;

public class ConsistentAlphabeticLogicalLettersPlacer : ILogicalLayoutPlacer
{
    public LogicalLettersLayout PlaceInRows(List<Letter> layerLetters)
    {
        var rows = new List<List<Letter>>();
        rows.Add(RowOf("abcde", layerLetters));
        rows.Add(RowOf("fghij", layerLetters));
        rows.Add(RowOf("klmno", layerLetters));
        rows.Add(RowOf("pqrst", layerLetters));
        rows.Add(RowOf("uvwxyz", layerLetters));

        var slots = 8;
        return new LogicalLettersLayout(rows, slots, 0.8f, 1f);
    }

    private List<Letter> RowOf(string orderedLetters, List<Letter> layerLetters)
    {
        List<Letter> orderedRow = new List<Letter>();
        foreach (var rowLetter in orderedLetters.ToCharArray().Select(c => c.ToString()))
        {
            Letter letterInRow = layerLetters.Find(letter => letter.letter == rowLetter);
            if (letterInRow != null)
            {
                orderedRow.Add(letterInRow);            
            }
        }
        return orderedRow;        
    }
}