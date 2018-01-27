using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordSuggester : MonoBehaviour
{
    private List<string> words;
    private int count;
    private DateTime startTime;

    void Start()
	{
		words = EnglishWords.AsList();
		count = 0;
		startTime = DateTime.Now;
		SuggestAWord();
	}
    public void Suggest(string lastWord)
    {
		SuggestAWord();
    }

    private void SuggestAWord()
    {
		count ++;
		Display(words[new System.Random().Next(words.Count)]);
    }

	private void Display(string word)
	{
		GetComponent<Text>().text = $"{SecondsSinceStart()}\n{count}: {word}";
	}

    private int SecondsSinceStart()
    {
        return (int) (DateTime.Now - startTime).Duration().TotalSeconds;
    }
}
