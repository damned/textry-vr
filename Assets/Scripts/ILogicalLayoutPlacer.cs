using System.Collections.Generic;

public interface ILogicalLayoutPlacer
{
    LogicalLettersLayout PlaceInRows(List<Letter> layerLetters);
}