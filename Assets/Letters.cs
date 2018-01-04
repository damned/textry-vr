using UnityEngine;

public class Letters : MonoBehaviour
{
  public delegate void LetterHandler(Letter letter);

  public void ForEach(LetterHandler handler)
  {
    Debug.Log("sdfadddasdfxxxxxxxxxxxxxxxxx");
    foreach (Transform letterTransform in transform)
    {
      // Debug.Log("letter transform: " + letterTransform);
      // Debug.Log("letter go: " + letterTransform.gameObject.name);
      handler(letterTransform.gameObject.GetComponent<Letter>());
    }
  }
}