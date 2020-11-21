using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEffect : MonoBehaviour
{
    public TextMeshProUGUI stringText; string stringString; bool skip = false;
    public List<TextMeshProUGUI> leftText; List<string> leftString; public List<TextMeshProUGUI> rightText; List<string> rightString;
    private Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
    //Resets all the text variables.
    void Update() { if (!skip && Input.anyKey) skip = true; }
    void Start()
    {
        leftString = new List<string>(); rightString = new List<string>();
        stringString = stringText.text; stringText.text = "";
        foreach (TextMeshProUGUI text in leftText) { leftString.Add(text.text); text.text = ""; }
        foreach (TextMeshProUGUI text in rightText) { rightString.Add(text.text); text.text = ""; }
        StartCoroutine(TypeWriterLeft()); StartCoroutine(TypeWriterRight()); StartCoroutine(TypeWriterString());
    }
    //Loops through each text variable and slowly adds a character one by one to simulate a type writer effect.
    private IEnumerator TypeWriterRight()
    {
        int i = 0; foreach (string text in rightString)
        {
            foreach (char character in text.ToCharArray())
            {
                if (skip) { rightText[i].text = rightString[i]; break; }
                rightText[i].text += character;
                yield return new WaitForSeconds(0.03f);
            }
            i++;
        }
    }
    private IEnumerator TypeWriterLeft()
    {
        int i = 0; foreach (string text in leftString)
        {
            foreach (char character in text.ToCharArray())
            {
                if (skip) { leftText[i].text = leftString[i]; break; }
                leftText[i].text += character;
                yield return new WaitForSeconds(0.01f);
            }
            i++;
        }
    }
    private IEnumerator TypeWriterString()
    {
        string template = stringText.text;
        foreach (char character in stringString.ToCharArray())
        {
            if (template != stringText.text) { break; }
            else if (skip) { stringText.text = stringString; break; }
            stringText.text += character;
            template += character;
            yield return new WaitForSeconds(0.30f);
        }
    }
}