using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using System;

public class VrtkHands : MonoBehaviour, IHands
{
  public VrtkHand rightHand;
  public VrtkHand leftHand;

  public event HandHandler OnHandUpdate;

  void Update()
  {
    OnHandUpdate(rightHand);
    OnHandUpdate(leftHand);
  }

}