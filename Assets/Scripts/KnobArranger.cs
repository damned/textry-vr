using System;
using System.Collections.Generic;
using System.Linq;

public class KnobArranger
{
    private Letters letters;
    private Knobs knobs;
    public int layers = 0;
    private readonly ILayerCreator layerCreator;
    private readonly ILogicalLayoutPlacer layoutPlacer;

    public KnobArranger(Letters letters, Knobs knobs, ILayerCreator layerCreator, ILogicalLayoutPlacer layoutPlacer)
    {
        this.layerCreator = layerCreator;
        this.layoutPlacer = layoutPlacer;
        this.letters = letters;
        this.knobs = knobs;
    }

    public string Arrange(float zOffset, string lastLetter = "")
    {
        layers += 1;
        var yOffset = -0.2f;
        var xOffset = 0f;
        var z = -1f + zOffset;
        LayerContents layerContents = layerCreator.NextLayer(lastLetter);
        List<Letter> layerLetters = layerContents.letters;
        var suggestions = layerContents.suggestions;

        var logicalLettersLayout = layoutPlacer.PlaceInRows(layerLetters);

        var xSpacing = 0.08f * logicalLettersLayout.xFactor;
        var ySpacing = 0.05f * logicalLettersLayout.yFactor;

        var xStart = -(xSpacing * logicalLettersLayout.slots) / 2;
        var yStart = 0f;
        var x = xStart + xOffset;
        var y = yStart + yOffset;

        string placement = "placed: ";
        int layer = layers - 1;

        var index = 0;
        var xIndex = 0;
        logicalLettersLayout.rows.ForEach((row) =>
        {
            row.ForEach((letter) =>
            {
                if (letter != Letter.SPACE)
                {
                    var knob = knobs.Create(letter, x, y, z, layer);
                    placement += knob.Name + ", ";
                }
                x += xSpacing;
            });
            xIndex ++;
            x = xStart + xOffset;
            y -= ySpacing;
        });
        y = yOffset;
        suggestions.ForEach(suggestion =>
        {
            y += ySpacing;
            knobs.CreateSuggestion(suggestion, xStart, y, z, layer);
        });
        return placement;
    }

    public void ResetLayers()
    {
        knobs.Reset();
        layers = 1;
    }

    public void RemoveLayer()
    {
        layers -= 1;
        knobs.RemoveLayer(layers);
    }
}