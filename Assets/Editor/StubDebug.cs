using UnityEngine;

public class StubDebug : IDebug
{
  void IDebug.Log(string message)
  {
    Debug.Log("[test] " + message);
  }
}