using System;

public class KnobArranger
{
    private Letters letters;
    private Knobs knobs;
    public int layers = 0;
    private readonly ILayerCreator layerCreator;

    public KnobArranger(Letters letters, Knobs knobs, ILayerCreator layerCreator)
    {
        this.layerCreator = layerCreator;
        this.letters = letters;
        this.knobs = knobs;
    }

    public string Arrange(float zOffset, string lastLetter = "")
    {
        layers += 1;
        var ySpacing = 0.05f;
        var xSpacing = 0.08f;
        var yOffset = -0.2f;
        var xOffset = 0f;
        var index = 0;
        var xIndex = 0;
        var z = -1f + zOffset;
        LayerContents layerContents = layerCreator.NextLayer(lastLetter);
        var layerLetters = layerContents.letters;
        var suggestions = layerContents.suggestions;

        var slots = (int)Math.Sqrt(layerLetters.Count) + 1;
        var xStart = -(xSpacing * slots) / 2;
        var yStart = 0f;
        var x = xStart + xOffset;
        var y = yStart + yOffset;

        string placement = "placed: ";
        layerLetters.ForEach((letter) =>
        {
            xIndex = index % slots;
            if (xIndex == 0)
            {
                y -= ySpacing;
                x = xStart + xOffset;
            }
            else
            {
                x += xSpacing;
            }
            index += 1;
            var knob = knobs.Create(letter, x, y, z, layers - 1);
            placement += knob.Name + ", ";
        });
        y = yOffset;
        suggestions.ForEach(suggestion => {
          y += ySpacing;
          knobs.CreateSuggestion(suggestion, xStart, y, z);
        });
        return placement;
    }

    public void ResetLayers()
    {
        knobs.Reset();
        layers = 1;
    }
}