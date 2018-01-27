using System;
using System.Collections.Generic;
using System.Linq;

public class ConsistentAlphabeticLogicalLettersPlacer : ILogicalLayoutPlacer
{
    public LogicalLettersLayout PlaceInRows(List<Letter> layerLetters)
    {
        var rows = new List<List<Letter>>();
        rows.Add(RowOf("abcdef", layerLetters));
        rows.Add(RowOf("ghijkl", layerLetters));
        rows.Add(RowOf("mnopqr", layerLetters));
        rows.Add(RowOf("stuvwx", layerLetters));
        rows.Add(RowOf("yz", layerLetters));

        var slots = 6;
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
            else {
                orderedRow.Add(Letter.SPACE);
            }
        }
        return orderedRow;        
    }
}