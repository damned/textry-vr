using System;
using System.Collections.Generic;
using System.Linq;
using Builders;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class LetterBasedLayerCreatorTest
{

  [Test]
  public void AfterAVowelIsProbablyAConsonant()
  {
    var creator = new LetterBasedLayerCreator(A.Letters(AToZ()), new ProbableAlphabeticPredictor());
    var next = creator.LayerLetters("i");

    Assert.AreEqual("bcdfghjklmnpqrstvwxyz", LettersToString(next));
  }

  private List<string> AToZ()
  {
    return "abcdefghijklmnopqrstuvwxyz".Select(ch => ch.ToString()).ToList();
  }

  private static string LettersToString(List<Letter> next)
  {
    var nextLetters = "";
    foreach (var letter in next)
    {
      nextLetters += letter.letter;
    }

    return nextLetters;
  }
}