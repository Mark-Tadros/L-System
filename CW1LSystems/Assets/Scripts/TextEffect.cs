// Holds all the Texts visual effects.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEffect : MonoBehaviour
{
    bool skip = false; public TextMeshProUGUI middleText; string middleString;
    public List<TextMeshProUGUI> leftText; List<string> leftString; public List<TextMeshProUGUI> rightText; List<string> rightString;
    private Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
    // Upon pressing any key skips the texts visual appearance.
    void Update() { if (!skip && Input.anyKey) skip = true; }
    void Start()
    {
        leftString = new List<string>(); rightString = new List<string>();
        // Resets text before inputting it into the type writer.
        middleString = middleText.text; middleText.text = "";
        foreach (TextMeshProUGUI text in leftText) { leftString.Add(text.text); text.text = ""; }
        foreach (TextMeshProUGUI text in rightText) { rightString.Add(text.text); text.text = ""; }
        StartCoroutine(TypeWriter(leftString, leftText));
        StartCoroutine(TypeWriter(rightString, rightText));
        // Transforms the single string into lists to bypass the type writer restrictions.
        List<string> NewString = new List<string>(); List<TextMeshProUGUI> NewText = new List<TextMeshProUGUI>();
        NewString.Add(middleString); NewText.Add(middleText);
        StartCoroutine(TypeWriter(NewString, NewText));
    }
    // Loops through each text variable and slowly adds a character one by one to simulate a type writer effect.
    private IEnumerator TypeWriter(List<string> String, List<TextMeshProUGUI> Text)
    {
        int i = 0; foreach (string text in String)
        {
            foreach (char character in text.ToCharArray())
            {
                if (skip) { Text[i].text = String[i]; break; }
                Text[i].text += character;
                // The small break here is what allows each character to appear.
                yield return new WaitForSeconds(0.01f);
            }
            i++;
        }
    }
}