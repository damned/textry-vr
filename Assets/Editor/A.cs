using System;
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
  }
}
