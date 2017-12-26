using UnityEngine;

public interface IHand
{
  HandSide Side();
  bool IsPresent();
  Vector3 Centre();
  double GrabStrength();
  bool IsClosed();
}