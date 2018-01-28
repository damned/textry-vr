using System;
using System.Collections.Generic;
using System.Linq;

public class KnobLayer
{
    public int index;
    private List<Knob> knobs = new List<Knob>();

    public KnobLayer(int layerIndex)
    {
        this.index = layerIndex;
    }

    public void Add(Knob knob)
    {
        knobs.Add(knob);
    }

    public void ForEach(KnobHandler handler)
    {
        foreach (var knob in knobs)
        {
            handler(knob);
        }
    }

    public bool Any(KnobFilter filter)
    {
        return knobs.Any(knob => filter(knob));
    }

    public void Delete()
    {
        foreach (var knob in knobs)
        {
            knob.Delete();
        };
        knobs.Clear();
    }
}