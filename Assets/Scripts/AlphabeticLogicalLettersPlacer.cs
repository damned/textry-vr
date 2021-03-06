using System;
using System.Collections.Generic;
using System.Linq;

public class AlphabeticLogicalLettersPlacer : ILogicalLayoutPlacer
{
    public int maxColumns = 6;
    public LogicalLettersLayout PlaceInRows(List<Letter> layerLetters)
    {
        var slots = Math.Min(maxColumns, layerLetters.Count);
        var index = 0;
        var xIndex = 0;
        var rows = new List<List<Letter>>();
        layerLetters.ForEach((letter) =>
        {
            xIndex = index % slots;
            if (xIndex == 0)
            {
                rows.Add(new List<Letter>());
            }
            rows.Last().Add(letter);
            index += 1;
        });
        return new LogicalLettersLayout(rows, slots, 1f, 1f);
    }

}