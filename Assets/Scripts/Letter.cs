using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{
    public static Letter SPACE = new Letter(" ");
    public string letter;

    public Letter(string letter)
    {
        this.letter = letter;
    }
}
