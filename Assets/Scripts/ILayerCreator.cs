using System.Collections.Generic;

public interface ILayerCreator
{
  LayerContents NextLayer(string lastLetter);
}