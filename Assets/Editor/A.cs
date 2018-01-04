using System;
using System.Collections.Generic;
using UnityEngine;

namespace Builders
{
  public class A
  {
    public static Letter Letter(string s)
    {
      GameObject go = new GameObject();
      var letter = go.AddComponent<Letter>();
      letter.letter = s;

      return letter;
    }

    internal static Letters Letters(List<string> alphabet)
    {
      GameObject lettersGo = new GameObject();
      var letters = lettersGo.AddComponent<Letters>();
      foreach(var letter in alphabet)
      {
        var letterGo = new GameObject();
        var letterObject = letterGo.AddComponent<Letter>();
        letterObject.letter = letter;
        letterGo.transform.parent = lettersGo.transform;
      }

      return letters;
    }
  }
}
