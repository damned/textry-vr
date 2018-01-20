using System;
using System.Collections.Generic;
using UnityEngine;

public enum LetterType { Vowel, Consonant };

public class LetterBasedLayerCreator : ILayerCreator
{
  public readonly Letters lettersObject;
  public readonly List<string> letters = new List<string>();
  private readonly Dictionary<string, Letter> lookup;
  private readonly IAlphabeticPredictor predictor;

  public LetterBasedLayerCreator(Letters lettersObject, IAlphabeticPredictor predictor)
  {
    this.predictor = predictor;
    this.lettersObject = lettersObject;
    this.lookup = new Dictionary<string, Letter>();
    lettersObject.ForEach(letter =>
    {
      lookup[letter.letter] = letter;
      letters.Add(letter.letter);
    });
  }

  public LayerContents NextLayer(string previousLetters)
  {
    return new LayerContents(LetterObjectsOf(predictor.PredictionAfter(previousLetters).letters));
  }

  private List<Letter> LetterObjectsOf(List<string> letters)
  {
    var layerLetterObjects = new List<Letter>();
    letters.ForEach(letter =>
    {
      if (lookup.ContainsKey(letter))
      {
        layerLetterObjects.Add(lookup[letter]);
      }
      else {
        Debug.Log($"missing required Letter '{letter}'");
      }
    });
    return layerLetterObjects;
  }
}