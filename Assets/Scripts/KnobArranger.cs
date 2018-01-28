using System;

public class KnobArranger
{
    private Knobs knobs;
    private readonly ILayerCreator layerCreator;

    public KnobArranger(Knobs knobs, ILayerCreator layerCreator)
    {
        this.layerCreator = layerCreator;
        this.knobs = knobs;
    }

    public void Arrange(string lastLetter = "")
    {
        var ySpacing = 0.05f;
        var xSpacing = 0.08f;
        var yOffset = -0.2f;
        var xOffset = 0f;
        var index = 0;
        var xIndex = 0;

        var layer = knobs.CreateLayer();
        var zOffset = 0.08f * knobs.LayerCount;
        
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
            var knob = knobs.Create(letter, x, y, z);
            placement += knob.Name + ", ";
        });
        y = yOffset;
        suggestions.ForEach(suggestion => {
          y += ySpacing;
          knobs.CreateSuggestion(suggestion, xStart, y, z);
        });
    }

    public void ResetLayers()
    {
        knobs.Reset();
    }
}