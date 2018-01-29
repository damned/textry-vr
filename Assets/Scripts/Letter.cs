using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour, ITextProvider
{
    public string letter;

    public string Text()
    {
        return letter;
    }
}
