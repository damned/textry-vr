using System;
using System.Collections.Generic;
using System.Linq;

public class QwertyLogicalLettersPlacer : ILogicalLayoutPlacer
{
    public LogicalLettersLayout PlaceInRows(List<Letter> layerLetters)
    {
        var rows = new List<List<Letter>>();
        rows.Add(RowOf("qwertyuiop", layerLetters));
        rows.Add(RowOf("asdfghjkl", layerLetters));
        rows.Add(RowOf("zxcvbnm", layerLetters));

        var slots = 8;
        return new LogicalLettersLayout(rows, slots);
    }

    private List<Letter> RowOf(string orderedLetters, List<Letter> layerLetters)
    {
        List<Letter> orderedRow = new List<Letter>();
        foreach (var rowLetter in orderedLetters.ToCharArray().Select(c => c.ToString()))
        {
            Letter letterInRow = layerLetters.Find(letter => letter.letter == rowLetter);
            if (letterInRow == null)
            {
                letterInRow = Letter.SPACE;
            }
            orderedRow.Add(letterInRow);            
        }
        return orderedRow;        
    }
}