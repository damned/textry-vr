using UnityEngine;

public interface IHand
{
  bool IsPresent();
  Vector3 Centre();
  double GrabStrength();
  bool IsClosed();
}