using System;
using System.Collections.Generic;
using System.Linq;

public class KnobArranger
{
    private Letters letters;
    private Knobs knobs;
    public int layers = 0;
    public float sideOffset = 0.1f;
    private readonly ILayerCreator layerCreator;
    private readonly ILogicalLayoutPlacer layoutPlacer;

    public KnobArranger(Letters letters, Knobs knobs, ILayerCreator layerCreator, ILogicalLayoutPlacer layoutPlacer)
    {
        this.layerCreator = layerCreator;
        this.layoutPlacer = layoutPlacer;
        this.letters = letters;
        this.knobs = knobs;
    }

    public string Arrange(float zOffset, string lastLetter = "", Nullable<HandSide> grabbedSide = null)
    {
        layers += 1;
        var yOffset = -0.2f;
        var xOffset = 0f;
        var z = -1f + zOffset;
        LayerContents layerContents = layerCreator.NextLayer(lastLetter);
        List<Letter> layerLetters = layerContents.letters;
        var suggestions = layerContents.suggestions;

        var logicalLettersLayout = layoutPlacer.PlaceInRows(layerLetters);

        var ySpacing = 0.05f * logicalLettersLayout.yFactor;
        var xSpacing = 0.08f * logicalLettersLayout.xFactor;
        float sideOffset = CalculateSideOffset(grabbedSide);

        var xStart = sideOffset - (xSpacing * logicalLettersLayout.slots) / 2;
        var yStart = 0f;
        var yLayerOffset = 0.025f;
        var yLayersOffset = yLayerOffset * layers;
        var x = xStart + xOffset;
        var y = yStart + yOffset + yLayersOffset;

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
            xIndex++;
            x = xStart + xOffset;
            y -= ySpacing;
        });
        y = yOffset + yLayersOffset;
        suggestions.ForEach(suggestion =>
        {
            y += ySpacing;
            knobs.CreateSuggestion(suggestion, xStart, y, z, layer);
        });
        return placement;
    }

    private float CalculateSideOffset(HandSide? grabbedSide)
    {
        if (grabbedSide.HasValue)
        {
            if (grabbedSide == HandSide.Left)
            {
                return sideOffset;
            }
            else
            {
                return -sideOffset;
            }
        }
        return 0f;
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