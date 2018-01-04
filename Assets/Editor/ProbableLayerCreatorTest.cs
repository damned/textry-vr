using System;
using System.Collections.Generic;
using System.Linq;
using Builders;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class ProbableLayerCreatorTest
{
  [Test]
  public void WithNoPreviousLetterNextLayerHasWholeAlphabet()
  {
    var creator = new ProbableLayerCreator(A.Letters(AToZ()));
    var next = creator.LayerLetters("");

    Assert.AreEqual("abcdefghijklmnopqrstuvwxyz", LettersToString(next));
  }


  [Test]
  public void AfterAVowelIsProbablyAConsonant()
  {
    var creator = new ProbableLayerCreator(A.Letters(AToZ()));
    var next = creator.LayerLetters("i");

    Assert.AreEqual("bcdfghjklmnpqrstvwxyz", LettersToString(next));
  }

  [Test]
  public void AfterAConsonantIsProbablyAVowel()
  {
    var creator = new ProbableLayerCreator(A.Letters(AToZ()));

    var next = creator.LayerLetters("t");

    Assert.AreEqual("aeiou", LettersToString(next));
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