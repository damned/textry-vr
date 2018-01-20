using System;

public delegate void HandHandler(IHand hand);

public interface IHands
{
    event HandHandler OnHandUpdate;
}