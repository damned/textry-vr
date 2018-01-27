using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordSuggester : MonoBehaviour
{
    private List<string> words;

    void Start()
	{
		words = EnglishWords.AsList();
		SuggestAWord();
	}
    public void Suggest(string lastWord)
    {
		SuggestAWord();
    }

    private void SuggestAWord()
    {
		Display(words[new System.Random().Next(words.Count)]);
    }

	private void Display(string word)
	{
		GetComponent<Text>().text = word;
	}
}
