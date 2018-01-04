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
    var next = predictor.LettersAfter("");

    Assert.AreEqual("abcdefghijklmnopqrstuvwxyz", LettersToString(next));
  }

  [Test]
  public void PredictsSecondLetterForOneWord()
  {
    var predictor = new DataBasedAlphabeticPredictor(Words("light"));
    var next = predictor.LettersAfter("l");

    Assert.AreEqual("i", LettersToString(next));
  }

  [Test]
  public void PredictsSecondLetterForThePossibleWord()
  {
    var predictor = new DataBasedAlphabeticPredictor(Words("jack", "doe"));
    var next = predictor.LettersAfter("d");

    Assert.AreEqual("o", LettersToString(next));
  }

  [Test]
  public void PredictsThirdLetterForThePossibleWord()
  {
    var predictor = new DataBasedAlphabeticPredictor(Words("jack", "doe"));

    Assert.AreEqual("e", LettersToString(predictor.LettersAfter("do")));
  }

  [Test]
  public void PredictsSubsequentLettersForThePossibleWord()
  {
    var predictor = new DataBasedAlphabeticPredictor(Words("jackie", "doe"));

    Assert.AreEqual("i", LettersToString(predictor.LettersAfter("jack")));
  }

  [Test]
  public void PredictsSecondLettersForAllWords()
  {
    var predictor = new DataBasedAlphabeticPredictor(Words("john", "jack"));

    Assert.AreEqual("oa", LettersToString(predictor.LettersAfter("j")));
  }

  private List<string> Words(params string [] words)
  {
    return words.ToList();
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