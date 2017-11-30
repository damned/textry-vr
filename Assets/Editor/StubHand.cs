using System;
using UnityEngine;

public class StubHand : IHand
{
  private Vector3 centre;
  private float grabStrength;
  private bool isPresent;

  public StubHand()
  {
    At(0f, 0f, 0f);
    WithGrabStrength(0f);
  }

  public StubHand At(float x, float y, float z)
  {
    return At(new Vector3(x, y, z));
  }

  public StubHand At(Vector3 position)
  {
    centre = position;
    ThatIsPresent();
    return this;
  }

  public StubHand Open()
  {
    return WithGrabStrength(0f);
  }

  public StubHand Closed()
  {
    return WithGrabStrength(1f);
  }

  public bool IsOpen()
  {
    return grabStrength < 0.5f;
  }

  public bool IsClosed()
  {
    return !IsOpen();
  }

  public StubHand WithGrabStrength(float strength)
  {
    grabStrength = strength;
    ThatIsPresent();
    return this;
  }

  public StubHand ThatIsNotPresent()
  {
    isPresent = false;
    return this;
  }

  public StubHand ThatIsPresent()
  {
    isPresent = true;
    return this;
  }

  public Vector3 Centre()
  {
    return centre;
  }

  public double GrabStrength()
  {
    return grabStrength;
  }

  public bool IsPresent()
  {
    return isPresent;
  }
}