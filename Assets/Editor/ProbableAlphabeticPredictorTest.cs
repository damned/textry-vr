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
    var next = predictor.LettersAfter("");

    Assert.AreEqual("abcdefghijklmnopqrstuvwxyz", LettersToString(next));
  }


  [Test]
  public void AfterAVowelIsProbablyAConsonant()
  {
    var predictor = new ProbableAlphabeticPredictor();
    var next = predictor.LettersAfter("i");

    Assert.AreEqual("bcdfghjklmnpqrstvwxyz", LettersToString(next));
  }

  [Test]
  public void AfterAConsonantIsProbablyAVowel()
  {
    var predictor = new ProbableAlphabeticPredictor();
    var next = predictor.LettersAfter("t");

    Assert.AreEqual("aeiou", LettersToString(next));
  }

  [Test]
  public void AfterSeveralLettersPredictionIsBasedOnLastConsonant()
  {
    var predictor = new ProbableAlphabeticPredictor();
    var next = predictor.LettersAfter("aaaaoooooooooooh");

    Assert.AreEqual("aeiou", LettersToString(next));
  }

  [Test]
  public void AfterSeveralLettersPredictionIsBasedOnLastVowel()
  {
    var predictor = new ProbableAlphabeticPredictor();
    var next = predictor.LettersAfter("frnngghhha");

    Assert.AreEqual("bcdfghjklmnpqrstvwxyz", LettersToString(next));
  }

  private List<string> AToZ()
  {
    return "abcdefghijklmnopqrstuvwxyz".Select(ch => ch.ToString()).ToList();
  }

  private static string LettersToString(List<string> next)
  {
    var nextLetters = "";
    foreach (var letter in next)
    {
      nextLetters += letter;
    }
    return nextLetters;
  }
}