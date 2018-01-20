using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiveDebug : MonoBehaviour, IDebug {

	public Text displayText;
	public bool copyToConsole = false;

	public void Log(string message)
	{
		displayText.text += message + "\n";
		if (copyToConsole)
		{
			Debug.Log(message);
		}
	}

	public void Clear()
	{
		displayText.text = "";
	}

}
