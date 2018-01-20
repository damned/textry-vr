using System;
using System.Collections.Generic;
using System.Linq;
using Builders;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class ProbableAlphabeticPredictorTest
{
  [Test]
  public void WithNoPreviousLetterNextLayerHasWholeAlphabet()
  {
    var predictor = new ProbableAlphabeticPredictor();
    var next = predictor.PredictionAfter("");

    Assert.AreEqual("abcdefghijklmnopqrstuvwxyz", LettersToString(next));
  }


  [Test]
  public void AfterAVowelIsProbablyAConsonant()
  {
    var predictor = new ProbableAlphabeticPredictor();
    var next = predictor.PredictionAfter("i");

    Assert.AreEqual("bcdfghjklmnpqrstvwxyz", LettersToString(next));
  }

  [Test]
  public void AfterAConsonantIsProbablyAVowel()
  {
    var predictor = new ProbableAlphabeticPredictor();
    var next = predictor.PredictionAfter("t");

    Assert.AreEqual("aeiou", LettersToString(next));
  }

  [Test]
  public void AfterSeveralLettersPredictionIsBasedOnLastConsonant()
  {
    var predictor = new ProbableAlphabeticPredictor();
    var next = predictor.PredictionAfter("aaaaoooooooooooh");

    Assert.AreEqual("aeiou", LettersToString(next));
  }

  [Test]
  public void AfterSeveralLettersPredictionIsBasedOnLastVowel()
  {
    var predictor = new ProbableAlphabeticPredictor();
    var next = predictor.PredictionAfter("frnngghhha");

    Assert.AreEqual("bcdfghjklmnpqrstvwxyz", LettersToString(next));
  }

  private List<string> AToZ()
  {
    return "abcdefghijklmnopqrstuvwxyz".Select(ch => ch.ToString()).ToList();
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