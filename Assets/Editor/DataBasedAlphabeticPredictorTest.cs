using System;
using System.Collections.Generic;
using System.Linq;
using Builders;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class DataBasedAlphabeticPredictorTest
{
  [Test]
  public void WithNoPreviousLetterNextLayerHasWholeAlphabet()
  {
    var predictor = new DataBasedAlphabeticPredictor(Words());
    var next = predictor.PredictionAfter("");

    Assert.AreEqual("abcdefghijklmnopqrstuvwxyz", LettersToString(next));
  }

  [Test]
  public void PredictsSecondLetterForOneWord()
  {
    var predictor = new DataBasedAlphabeticPredictor(Words("light"));
    var next = predictor.PredictionAfter("l");

    Assert.AreEqual("i", LettersToString(next));
  }

  [Test]
  public void PredictsSecondLetterForThePossibleWord()
  {
    var predictor = new DataBasedAlphabeticPredictor(Words("jack", "doe"));
    var next = predictor.PredictionAfter("d");

    Assert.AreEqual("o", LettersToString(next));
  }

  [Test]
  public void PredictsThirdLetterForThePossibleWord()
  {
    var predictor = new DataBasedAlphabeticPredictor(Words("jack", "doe"));

    Assert.AreEqual("e", LettersToString(predictor.PredictionAfter("do")));
  }

  [Test]
  public void PredictsSubsequentLettersForThePossibleWord()
  {
    var predictor = new DataBasedAlphabeticPredictor(Words("jackie", "doe"));

    Assert.AreEqual("i", LettersToString(predictor.PredictionAfter("jack")));
  }

  [Test]
  public void SuggestedNextLettersAreLimitedToOnePerLetter()
  {
    var predictor = new DataBasedAlphabeticPredictor(Words("jackie", "jaffie", "jazzy"));

    Assert.AreEqual("a", LettersToString(predictor.PredictionAfter("j")));
  }

  [Test]
  public void PredictsSecondLettersForAllWords()
  {
    var predictor = new DataBasedAlphabeticPredictor(Words("john", "jack"));

    Assert.AreEqual("oa", LettersToString(predictor.PredictionAfter("j")));
  }

  private List<string> Words(params string [] words)
  {
    return words.ToList();
  }

  private static string LettersToString(Prediction next)
  {
    var nextLetters = "";
    foreach (var letter in next.letters)
    {
      nextLetters += letter;
    }
    return nextLetters;
  }
}