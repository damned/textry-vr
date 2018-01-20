using System;
using System.Collections.Generic;
using System.Linq;
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
        Prediction prediction = predictor.PredictionAfter(previousLetters);
        LayerContents layerContents = new LayerContents(LetterObjectsOf(prediction.letters));
        layerContents.suggestion = Suggestion(prediction);
        return layerContents;
    }

    private List<Letter> Suggestion(Prediction prediction)
    {
        if (prediction.suggestions.Count == 0)
        {
            return new List<Letter>();
        }
        var firstSuggestion = prediction.suggestions[0];
        return LetterObjectsOf(firstSuggestion.ToCharArray().Select(c => c.ToString()).ToList());
    }

    private List<Letter> LetterObjectsOf(List<string> letters)
    {
        var layerLetterObjects = new List<Letter>();
        letters.ForEach(letter =>
        {
            Letter item = LookupLetterObject(letter);
            if (item != null)
            {
                layerLetterObjects.Add(item);
            }
        });
        return layerLetterObjects;
    }

    private Letter LookupLetterObject(string letter)
    {
        Letter item = null;
        if (lookup.ContainsKey(letter))
        {
            item = lookup[letter];
        }
        else
        {
            Debug.Log($"missing required Letter '{letter}'");
        }

        return item;
    }
}